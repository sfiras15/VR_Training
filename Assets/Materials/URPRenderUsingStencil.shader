Shader "Custom/VisualizerObjectShader"
{
    Properties
    {
        _CutoffHeight ("Cutoff Height", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            ColorMask 0
            ZWrite Off
            Stencil
            {
                Ref 1                  // Reference value for the stencil buffer
                Comp Always            // Always pass stencil test
                Pass Replace           // Replace stencil buffer value with Ref
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _CutoffHeight;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.worldPos = TransformObjectToWorld(v.positionOS).xyz;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                if (i.worldPos.y < _CutoffHeight)
                {
                    return half4(1, 1, 1, 1);
                }
                else
                {
                    return half4(0, 0, 0, 0);
                }
            }
            ENDHLSL
        }
    }
    FallBack "Universal Forward"
}
