using UnityEngine.Rendering.Universal;
using YNL.Extensions.Methods;

public class BlurRendererFeature : ScriptableRendererFeature
{
    BlurRenderPass blurRenderPass;

    public override void Create()
    {
        blurRenderPass = new BlurRenderPass();
        name = "Blur";
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (blurRenderPass.Setup(renderer))
        {
            renderer.EnqueuePass(blurRenderPass);
        }
    }
}