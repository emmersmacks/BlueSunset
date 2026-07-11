Shader "Custom/URP/VolumetricSpotCone"
{
    Properties
    {
        [HDR] _Color ("Цвет луча", Color) = (1, 0.95, 0.8, 1)
        _Intensity ("Интенсивность", Range(0, 5)) = 1.0

        _EdgeSoftness ("Мягкость краёв конуса", Range(0.5, 8)) = 2.0
        _LengthFade ("Затухание по длине", Range(0.5, 8)) = 2.0

        _DepthFade ("Мягкость пересечения с геометрией", Range(0.01, 5)) = 1.0
        _CameraFade ("Затухание у камеры", Range(0.01, 5)) = 0.5

        [Header(Noise)]
        _NoiseTex ("Текстура шума (опционально)", 2D) = "white" {}
        _NoiseStrength ("Сила шума", Range(0, 1)) = 0.0
        _NoiseSpeed ("Скорость шума", Vector) = (0.05, 0.1, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Name "VolumetricCone"

            Blend One One      // аддитивное смешивание — свет только добавляется
            ZWrite Off
            Cull Front         // рисуем внутренние грани: луч виден и изнутри конуса

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                half4  _Color;
                half   _Intensity;
                half   _EdgeSoftness;
                half   _LengthFade;
                half   _DepthFade;
                half   _CameraFade;
                float4 _NoiseTex_ST;
                half   _NoiseStrength;
                half4  _NoiseSpeed;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float2 uv         : TEXCOORD2;
            };

            Varyings vert(Attributes input)
            {
                Varyings o;
                o.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                o.positionCS = TransformWorldToHClip(o.positionWS);
                o.normalWS   = TransformObjectToWorldNormal(input.normalOS);
                o.uv         = input.uv;
                return o;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - input.positionWS);
                float3 normal  = normalize(input.normalWS);

                // --- 1. Мягкие края конуса ---
                // На силуэте (взгляд скользит по поверхности) луч тоньше — гасим.
                // В центре (взгляд сквозь толщу конуса) — плотнее.
                half ndotv = abs(dot(normal, viewDir));
                half edge = pow(ndotv, _EdgeSoftness);

                // --- 2. Затухание по длине луча (uv.y: 0 у источника, 1 на конце) ---
                half lengthFade = pow(saturate(1.0 - input.uv.y), _LengthFade);
                // лёгкое смягчение у самой вершины, чтобы не было жёсткой точки
                lengthFade *= smoothstep(0.0, 0.08, input.uv.y);

                // --- 3. Мягкое пересечение с геометрией сцены ---
                float2 screenUV = GetNormalizedScreenSpaceUV(input.positionCS);
                float sceneEyeDepth = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);
                float fragEyeDepth  = input.positionCS.w;
                half depthFade = saturate((sceneEyeDepth - fragEyeDepth) / _DepthFade);

                // --- 4. Плавное исчезание, когда камера входит в конус ---
                half cameraFade = saturate((fragEyeDepth - _ProjectionParams.y) / _CameraFade);

                // --- 5. Шум (пыль в воздухе), опционально ---
                half noise = 1.0;
                if (_NoiseStrength > 0.001)
                {
                    float2 nuv1 = input.uv * _NoiseTex_ST.xy + _Time.y * _NoiseSpeed.xy;
                    float2 nuv2 = input.uv * _NoiseTex_ST.xy * 1.7 - _Time.y * _NoiseSpeed.xy * 0.6;
                    half n1 = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, nuv1).r;
                    half n2 = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, nuv2).r;
                    noise = lerp(1.0, n1 * n2 * 2.0, _NoiseStrength);
                }

                half alpha = edge * lengthFade * depthFade * cameraFade * noise;
                return half4(_Color.rgb * _Intensity * alpha, 1);
            }
            ENDHLSL
        }
    }
    Fallback Off
}
