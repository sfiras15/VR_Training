Shader "Custom/URPStencilSetBottom"
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
                Ref 1                  // Reference value
                Comp always            // Always pass stencil test
                Pass replace           // Replace stencil buffer value with Ref
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
                // Make sure to use the correct float4 type
                float4 positionOS = float4(v.positionOS.xyz, 1.0);
                o.positionHCS = TransformObjectToHClip(positionOS); // Explicitly use a float4 vector
                o.worldPos = TransformObjectToWorld(positionOS).xyz; // Use .xyz to extract the float3
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // Only write to stencil if the fragment is below the cutoff height
                if (i.worldPos.y < _CutoffHeight)
                {
                    return half4(1, 1, 1, 1);  // Color doesn't matter
                }
                else
                {
                    return half4(0, 0, 0, 0);  // Return black if discarded
                }
            }
            ENDHLSL
        }
    }
}
