Shader "Custom/Stencil/StencilMask"
{
    SubShader
    {
        Tags { "RenderType"="Transparent"}
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha

        LOD 200

        //穴を開けるパス
        Pass
        {
            ZTest GEqual

            Stencil
            {
                // ステンシルの番号
                Ref 2
                
                // Always: このシェーダでレンダリングされたピクセルのステンシルバッファを「対象」とするという意味
                Comp Always
                
                // Replace: 「対象」としたステンシルバッファにRefの値を書き込む、という意味
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 透明にする
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }

        //穴を開けないパス
        Pass
        {
            ZTest Lequal

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 透明にする
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}