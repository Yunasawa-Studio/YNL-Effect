using UnityEngine.Rendering.Universal;

public class BlurRenderFeature : ScriptableRendererFeature
{
    private BlurRenderPass _blurRenderPass;

    public override void Create()
    {
        _blurRenderPass = new();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_blurRenderPass);
    }
}