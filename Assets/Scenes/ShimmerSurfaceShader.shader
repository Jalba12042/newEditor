Shader "Custom/ShimmerSurfaceShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _ShimmerColor ("Shimmer Color", Color) = (1,1,1,1)
        _ShimmerStrength ("Shimmer Strength", Range(0,1)) = 0.5
        _ShimmerSpeed ("Shimmer Speed", Float) = 1.0
        _ShimmerWidth ("Shimmer Width", Float) = 0.2
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        float4 _Color;
        float4 _ShimmerColor;
        float _ShimmerStrength;
        float _ShimmerSpeed;
        float _ShimmerWidth;

        struct Input
        {
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Use flat base color
            float3 baseCol = _Color.rgb;

            // Animate shimmer band using sine wave across world X
            float shimmerBand = sin(_Time.y * _ShimmerSpeed + IN.worldPos.x * 4.0);

            // Soft fade edge using smoothstep
            float mask = smoothstep(_ShimmerWidth, 0.0, abs(shimmerBand));

            // Combine shimmer color with base color
            float3 shimmer = _ShimmerColor.rgb * _ShimmerStrength * mask;
            float3 finalColor = baseCol + shimmer;

            o.Albedo = finalColor;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}