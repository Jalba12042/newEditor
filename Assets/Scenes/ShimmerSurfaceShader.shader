Shader "Custom/ScrollShimmerFixed"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Scroll Base Color", Color) = (0.9, 0.8, 0.6, 1)
        _ShimmerColor ("Shimmer Line Color", Color) = (1,1,1,1)
        _ShimmerWidth ("Shimmer Width", Range(0.01, 0.5)) = 0.1
        _ShimmerSpeed ("Shimmer Speed", Float) = 1.0
        _ShimmerIntensity ("Shimmer Intensity", Range(0, 2)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _BaseColor;
            float4 _ShimmerColor;
            float _ShimmerWidth;
            float _ShimmerSpeed;
            float _ShimmerIntensity;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // If texture is fully white, assume no texture was provided and use base color
                bool useBaseColor = all(texColor.rgb == 1.0);
                fixed4 base = useBaseColor ? _BaseColor : texColor;

                // Diagonal shimmer logic
                float shimmerPos = frac(_Time.y * _ShimmerSpeed);
                float dist = abs(i.uv.x + i.uv.y - shimmerPos * 2.0); // Diagonal line

                float shimmer = smoothstep(_ShimmerWidth, 0.0, dist);
                shimmer *= _ShimmerIntensity;

                fixed4 finalColor = base + (_ShimmerColor * shimmer);
                finalColor.a = base.a;

                return finalColor;
            }
            ENDCG
        }
    }
    Fallback Off
}
