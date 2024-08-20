using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenuForRenderPipeline("YNL - Effect/Gaussian Blur", typeof(UniversalRenderPipeline))]
public class GaussianBlur : VolumeComponent, IPostProcessComponent
{
    [Tooltip("Standard deviation (spread) of the blur. Grid size is approx. 3x larger.")]
    public ClampedFloatParameter Strength = new ClampedFloatParameter(0, 0, 100);

    public bool IsActive() => (Strength.value > 0.0f) && active;
    public bool IsTileCompatible() => false;
}