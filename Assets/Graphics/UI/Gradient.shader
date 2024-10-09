Shader "Custom/ThreeColorGradientShader"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1) // Red
        _Color2 ("Color 2", Color) = (0, 1, 0, 1) // Green
        _Color3 ("Color 3", Color) = (0, 0, 1, 1) // Blue
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Get UV value for the gradient (typically, use x or y depending on desired gradient direction)
                float gradient = i.uv.x;

                // Interpolate between the three colors
                if (gradient < 0.5)
                {
                    // Lerp between Color1 and Color2
                    return lerp(_Color1, _Color2, gradient * 2.0);
                }
                else
                {
                    // Lerp between Color2 and Color3
                    return lerp(_Color2, _Color3, (gradient - 0.5) * 2.0);
                }
            }
            ENDCG
        }
    }
}
