Shader "Skybox/SunsetLowPolyCloudsSmooth"
{
    Properties
    {
        _TopColor ("Sky Top Color", Color) = (0.13, 0.17, 0.37, 1)         // Deep Indigo
        _MiddleColor ("Sun Glow Color", Color) = (1.0, 0.47, 0.22, 1)       // Bright Orange
        _HorizonColor ("Horizon Color", Color) = (1.0, 0.77, 0.55, 1)       // Peach/Salmon

        _CloudColor ("Cloud Color", Color) = (1.0, 0.9, 0.85, 1)            // Soft Pink-White
        _CloudDensity ("Cloud Density", Range(0, 1)) = 0.5
        _CloudSharpness ("Cloud Sharpness", Range(0.1, 10)) = 3

        _SunPosition ("Sun Position (Y)", Range(-1, 1)) = -0.2
        _BlendSharpness ("Blend Sharpness", Range(0.1, 10)) = 3
    }

    SubShader
    {
        Tags { "Queue" = "Background" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; };
            struct v2f {
                float4 vertex : SV_POSITION;
                float3 direction : TEXCOORD0;
            };

            float4 _TopColor;
            float4 _MiddleColor;
            float4 _HorizonColor;

            float4 _CloudColor;
            float _CloudDensity;
            float _CloudSharpness;

            float _SunPosition;
            float _BlendSharpness;

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float cloudPattern(float2 uv)
            {
                float2 grid = floor(uv * 10);
                float2 f = frac(uv * 10);

                float h = hash(grid);
                float dist = length(f - 0.5 + h * 0.2);
                float shape = saturate(1.0 - pow(dist * 2.5, _CloudSharpness));
                return shape;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = normalize(mul(unity_ObjectToWorld, v.vertex).xyz);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float y = i.direction.y;

                // Three-way smooth gradient blend
                float topBlend = saturate(pow(y * 0.5 + 0.5, _BlendSharpness));
                float horizonBlend = saturate(pow(1.0 - abs(y - _SunPosition), _BlendSharpness));

                float3 skyColor = lerp(_TopColor.rgb, _MiddleColor.rgb, topBlend);
                skyColor = lerp(skyColor, _HorizonColor.rgb, horizonBlend);

                // Add clouds if above horizon
                if (y > 0.0)
                {
                    float2 cloudUV = normalize(i.direction.xz) * 0.5 + 0.5;
                    cloudUV += i.direction.y * 0.5;

                    float cloud = cloudPattern(cloudUV);
                    skyColor = lerp(skyColor, _CloudColor.rgb, cloud * _CloudDensity);
                }

                return float4(skyColor, 1.0);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
