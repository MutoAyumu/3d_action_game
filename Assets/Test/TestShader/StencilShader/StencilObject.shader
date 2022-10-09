Shader "Custom/StencilObject"
{
    Properties
    {
        _MainTex ("Base(RGB)", 2D) = "white" {}
    }

    SubShader
    {
        //サーフェスシェーダーはPassを入れない

        //Tagの指定
        Tags
        {
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
        }

        LOD 200

        //ステンシル
        Stencil
        {
            //バッファに書き込む値
            Ref 1

            //バッファ値が等しくないピクセルだけを表示
            Comp Notequal

            //Passは初期状態で
        }

        //cg言語(シェーダーの記述開始)
        CGPROGRAM

        //Surfaceで使用する関数の定義
        #pragma surface surf Lambert

        //テクスチャ変数の定義
        sampler2D _MainTex;

        //Input構造体
        struct Input
        {
            float2 uv_MainTex;
        };

        //surf関数
        void surf(Input IN, inout SurfaceOutput o)
        {
            //テクスチャのピクセルの色を返す
            half4 color = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = color.rgb;
            o.Alpha = color.a;
        }

        //（シェーダーの記述終了）
        ENDCG
    }

    //SubShaderの処理が正しく無かった時に使われる
    FallBack "Diffuse"
}
