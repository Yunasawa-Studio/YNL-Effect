Shader "YNL/Effect/BoxBlur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Strength("Strength", Integer) = 1
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MainTex_TexelSize;
            sampler2D _MainTex;
            uint _Strength;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                half2 texelSize = _Strength * _MainTex_TexelSize.xy;

                half4 sum = col;
                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        half2 offset = half2(x, y) * texelSize;
                        sum += tex2D(_MainTex, i.uv + offset);
                    }
                }

                return sum / 9.0;
            }
            ENDCG
        }
    }
}
