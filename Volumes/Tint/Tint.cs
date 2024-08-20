using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenuForRenderPipeline("YNL/Tint", typeof(UniversalRenderPipeline))]
public class Tint : VolumeComponent, IPostProcessComponent
{
    public FloatParameter TintIntensity = new(1);
    public ColorParameter TintColor = new(Color.white); 

    public bool IsActive() => true;

    public bool IsTileCompatible() => true;
}