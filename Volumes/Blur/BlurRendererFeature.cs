using UnityEngine.Rendering.Universal;

namespace YNL.Effects.Volumes
{
    public class BlurRendererFeature : ScriptableRendererFeature
    {
        private BlurRenderPass _blurRenderPass;

        public override void Create()
        {
            _blurRenderPass = new();
            name = "Blur RF";
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_blurRenderPass);
        }
    }
}