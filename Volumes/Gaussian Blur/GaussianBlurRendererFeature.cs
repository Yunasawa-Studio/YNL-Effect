using UnityEngine.Rendering.Universal;

namespace YNL.Effects.Volumes
{
    public class GaussianBlurRendererFeature : ScriptableRendererFeature
    {
        private GaussianBlurRenderPass _blurRenderPass;

        public override void Create()
        {
            _blurRenderPass = new();
            name = "Gaussian Blur RF";
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_blurRenderPass);
        }
    }
}