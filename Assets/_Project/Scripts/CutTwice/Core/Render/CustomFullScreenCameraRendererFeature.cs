using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CutTwice.Core.Render
{
    public class CustomFullScreenCameraRendererFeature : FullScreenPassRendererFeature
    {
        public string targetTag;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            Camera camera = renderingData.cameraData.camera;
            if (camera.gameObject.tag != targetTag)
            {
                return;
            }
            
            base.AddRenderPasses(renderer, ref renderingData);
        }
    }
}