Shader "Custom/Mask"
{
    Properties
    {
        _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "Queue"="Geometry-1"
        }

        Stencil
        {
            Ref 1
            Comp Never
            Fail Replace
        }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        struct Input
        {
            float4 color:COLOR;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
        }
        ENDCG
    }
    FallBack "Diffuse"
}
