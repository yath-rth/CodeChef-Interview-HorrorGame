Shader "Custom/ObjectURPWithOutline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1) // Default black outline
        _OutlineWidth ("Outline Width", Float) = 0.02 // Width of the outline
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 200

        // First Pass (Main object rendering)
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Stencil
            {
                Ref 1
                Comp Equal
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _Glossiness;
            half _Metallic;
            half4 _Color;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, IN.uv) * _Color;
                return texColor;
            }
            ENDHLSL
        }

        // Second Pass (Outline rendering)
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="UniversalForward" }

            ZWrite On
            Cull Front
            Offset 5,5 // Offset the outline geometry to prevent Z-fighting

            Stencil
            {
                Ref 1
                Comp Equal
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex outlineVert
            #pragma fragment outlineFrag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            // Declare _OutlineWidth and _OutlineColor globally
            float _OutlineWidth;
            float4 _OutlineColor;

            // Outline vertex shader, scales object geometry to create an outline effect
            Varyings outlineVert(Attributes IN)
            {
                Varyings OUT;
                // Inflate object by scaling its position in object space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz * (1 + _OutlineWidth));
                return OUT;
            }

            // Outline fragment shader: Color the outline with the outline color
            half4 outlineFrag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}
