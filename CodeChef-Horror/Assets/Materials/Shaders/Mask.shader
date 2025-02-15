Shader "Custom/InvisibleStencilMask"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Geometry-100" "RenderType"="Opaque" }

        ColorMask 0     // Prevents color output (fixes white tint)
        ZWrite Off      // Ensures objects behind remain visible
        Blend Zero One  // Ensures full transparency (no unintended rendering)

        Stencil
        {
            Ref 1
            Pass Replace  // Writes stencil value
        }

        Pass
        {
            Name "MaskPass"
            Tags { "LightMode"="UniversalForward" }

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

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return half4(0, 0, 0, 0);  // Fully transparent (ensures no white tint)
            }
            ENDHLSL
        }
    }
}
