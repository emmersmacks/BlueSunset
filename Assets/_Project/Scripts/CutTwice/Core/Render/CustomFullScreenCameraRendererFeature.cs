using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CutTwice.Core.Render
{
    public class CustomFullScreenCameraRendererFeature : FullScreenPassRendererFeature
    {
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            Camera camera = renderingData.cameraData.camera;
            if (!camera.gameObject.TryGetComponent(out FogCameraRenderer fogCameraRenderer))
            {
                return;
            }
            
            passMaterial = fogCameraRenderer.fogMaterial;
            
            base.AddRenderPasses(renderer, ref renderingData);
        }
    }
}