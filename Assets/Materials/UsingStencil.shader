Shader "Custom/PrintedObjectShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass
        {
            Tags{"LightMode" = "UniversalForward"}

            Stencil
            {
                Ref 1
                Comp Equal      // Render where stencil buffer equals 1
                Pass Replace    // Replace the stencil value with 1
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
            };

            half4 _BaseColor;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                return _BaseColor;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Forward"
}
