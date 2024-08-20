using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRenderPass : ScriptableRenderPass
{
    private Material _material;
    private int _blurID = Shader.PropertyToID("_Blur");
    private RenderTargetIdentifier _source, _target;
    private Blur _blur;

    public BlurRenderPass()
    {
        if (!_material)
        {
            _material = CoreUtils.CreateEngineMaterial("YNL/Post Processing/Blur");
        }
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        _source = renderingData.cameraData.renderer.cameraColorTargetHandle;
        cmd.GetTemporaryRT(_blurID, descriptor);
        _target = new(_blurID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer commandBuffer = CommandBufferPool.Get("Blur Render Feature");

        if (_blur == null) _blur = VolumeManager.instance.stack.GetComponent<Blur>();

        if (_blur.IsActive())
        {
            int gridSize = Mathf.CeilToInt(_blur.Strength.value * 3.0f);

            if (gridSize % 2 == 0)
            {
                gridSize++;
            }

            _material.SetInteger("_GridSize", gridSize);
            _material.SetFloat("_Spread", _blur.Strength.value);

            commandBuffer.Blit(_source, _target, _material, 0);
            commandBuffer.Blit(_target, _source);
        }

        context.ExecuteCommandBuffer(commandBuffer);
        CommandBufferPool.Release(commandBuffer);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(_blurID);
    }
}