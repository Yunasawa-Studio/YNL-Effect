using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace YNL.Effects.Volumes
{
    [System.Serializable, VolumeComponentMenuForRenderPipeline("YNL - Effect/Gaussian Blur", typeof(UniversalRenderPipeline))]
    public class Blur : VolumeComponent, IPostProcessComponent
    {
        public enum BlurType { GaussianBlur, BoxBlur, ChannelBlur }


        public BlurTypeParameter Type = new(BlurType.GaussianBlur);

        [Tooltip("Standard deviation (spread) of the blur. Grid size is approx. 3x larger.")]
        public ClampedFloatParameter Strength = new ClampedFloatParameter(0, 0, 30);

        public bool IsActive() => (Strength.value > 0.0f) && active;
        public bool IsTileCompatible() => false;
    
    }

    [System.Serializable]
    public class BlurTypeParameter : VolumeParameter<Blur.BlurType>
    {
        public BlurTypeParameter(Blur.BlurType value, bool overrideState = false) : base(value, overrideState)
        {

        }
    }
}