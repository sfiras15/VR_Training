Shader "Custom/PrintingEffectShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _ConstructY ("Construct Y", Float) = 0.5
        _ConstructGap ("Construct Gap", Float) = 0.1
        _Metallic ("Metallic", Range(0, 1)) = 0.5
        _Glossiness ("Smoothness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Cull Off

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _ConstructY;
            float _ConstructGap;
            float _Metallic;
            float _Glossiness;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = mul(UNITY_MATRIX_MVP, v.positionOS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.positionOS).xyz;
                o.viewDir = GetWorldSpaceViewDir(o.worldPos);
                o.normalWS = normalize(mul((float3x3)unity_ObjectToWorld, v.normalOS));
                return o;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Wobbly Effect
                float s = sin((IN.worldPos.x * IN.worldPos.z) * 60 + _Time.y + dot(IN.normalWS, IN.viewDir)) / 120.0;

                // Cutting the Geometry, discard pixels above the build line
                if (IN.worldPos.y > _ConstructY + s + _ConstructGap)
                {
                    discard;
                }

                // Determine surface lighting
                half3 albedo = tex2D(_MainTex, IN.uv).rgb * _Color.rgb;
                half alpha = tex2D(_MainTex, IN.uv).a * _Color.a;

                // Get main light direction and color
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);
                half3 lightColor = mainLight.color;

                // Calculate lighting (Lambert + Blinn-Phong specular)
                half3 normal = normalize(IN.normalWS);
                half3 viewDir = normalize(IN.viewDir);
                half3 halfDir = normalize(lightDir + viewDir);

                half NdotL = saturate(dot(normal, lightDir));
                half3 diffuse = NdotL * albedo * lightColor;

                half NdotH = saturate(dot(normal, halfDir));
                half3 specular = pow(NdotH, _Glossiness * 128.0) * _Metallic;

                // Incorporate ambient light for better visibility
                half3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo;
                half3 color = ambient + diffuse + specular;

                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Standard"
}
