using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using YNL.Extensions.Methods;

public class TintRenderFeature : ScriptableRendererFeature
{
    private TintPass _tintPass;

    public override void Create()
    {
        _tintPass = new TintPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_tintPass);
    }

    private class TintPass : ScriptableRenderPass
    {
        private Material _material;
        private int _tintID = Shader.PropertyToID("_Temp");
        private RenderTargetIdentifier _source, _target;

        public TintPass()
        {
            if (!_material) _material = CoreUtils.CreateEngineMaterial("CustomPost/ScreenTint");
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            _source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            cmd.GetTemporaryRT(_tintID, descriptor, FilterMode.Bilinear);
            _target = new(_tintID);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("TintRenderFeature");
            VolumeStack stack = VolumeManager.instance.stack;
            Tint tint = stack.GetComponent<Tint>();

            if (tint.IsActive())
            {
                _material.SetColor("_OverlayColor", (Color)tint.TintColor);
                _material.SetFloat("_Intensity", (float)tint.TintIntensity);

                Blit(commandBuffer, _source, _target, _material, 0);
                Blit(commandBuffer, _target, _source);
            }

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_tintID);
        }
    }
}