Shader "YNL/Effect/ChannelBlur"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Spread("Standard Deviation (Spread)", Float) = 0
		_Strength("Grid Size", Integer) = 1
    }
    SubShader
    {
        Tags
		{
			"RenderType" = "Opaque"
			"RenderPipeline" = "UniversalPipeline"
		}

		HLSLINCLUDE

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		#define E 2.71828f

		sampler2D _MainTex;

		CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_TexelSize;
			uint _Strength;
			float _Spread;
		CBUFFER_END

		float gaussian(int x)
		{
			float sigmaSqu = _Spread * _Spread;
			return (1 / sqrt(TWO_PI * sigmaSqu)) * exp(-(x * x) / (2 * sigmaSqu));
		}

		struct appdata
		{
			float4 positionOS : Position;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 positionCS : SV_Position;
			float2 uv : TEXCOORD0;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
			o.uv = v.uv;
			return o;
		}

		ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag_vertial

            float4 frag_vertial(v2f i) : SV_Target
			{
				float3 col = float3(0.0f, 0.0f, 0.0f);
				float gridSum = 0.0f;

				int upper = ((_Strength - 1) / 2);
				int lower = -upper;

				half2 texelSize = _Strength * _MainTex_TexelSize.xy;

				for (int x = lower; x <= upper; ++x)
				{
					float gauss = gaussian(x);
					gridSum += gauss;

					
					float2 uvRed = i.uv + float2(_MainTex_TexelSize.x * x, 0.0f);
					col.r += gauss * tex2D(_MainTex, uvRed).r;

					float2 uvGreen = i.uv + float2(0.0f, texelSize.y * x);
					col.g += gauss * tex2D(_MainTex, uvGreen).g;

					float2 uvBlue = i.uv + float2(texelSize.x * x, texelSize.y * x);
					col.b += gauss * tex2D(_MainTex, uvBlue).b;
				}

				for (int y = lower; y <= upper; ++y)
				{
					float gauss = gaussian(y);
					gridSum += gauss;

					float2 uvRed = i.uv + float2(0.0f, _MainTex_TexelSize.y * y);
					col.r += gauss * tex2D(_MainTex, uvRed).r;

					float2 uvGreen = i.uv + float2(texelSize.x * y, 0.0f);
					col.g += gauss * tex2D(_MainTex, uvGreen).g;

					float2 uvBlue = i.uv + float2(texelSize.x * y, _MainTex_TexelSize.y * y);
					col.b += gauss * tex2D(_MainTex, uvBlue).b;
				}

				col /= gridSum;

				return float4(col, 1.0f);
			}

            ENDHLSL
        }
    }
}