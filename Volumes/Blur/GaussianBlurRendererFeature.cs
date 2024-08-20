using UnityEngine.Rendering.Universal;

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