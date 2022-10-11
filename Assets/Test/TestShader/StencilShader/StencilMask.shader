Shader "Custom/StencilMask"
{
    // プロパティ
    Properties 
    {
        // テクスチャ
        _MainTex ("Base(RGB)", 2D) = "white" {}
        _Alpha ("Alpha", Range(0.0,1.0)) = 0.5
    }

    SubShader 
    {
        // 不透明なオブジェクト
        Tags 
        {
            "Queue" = "Geometry"
            "RenderType" = "Transparent"
            "ForceNoShadowCasting" = "True"
        }

        ZTest always
        LOD 200

        // ステンシル
        Stencil 
        {
            // バッファに書き込む値
            Ref 1
            // 常に
            Comp Gequal
            // バッファに書き込む
            Pass replace
        }

        CGPROGRAM

        #pragma surface surf Lambert alpha

        half _Alpha;

        // Input構造体
        struct Input 
        {
            // テクスチャ
            float2 uv_MainTex;
        };

        // surf関数
        void surf (Input IN, inout SurfaceOutput o)
        {
            //完全に透明だと分かりにくいから少し色をつける
            o.Albedo = fixed3(0, 0, 0);
            o.Alpha = _Alpha;
        }
        // Shaderの記述終了
        ENDCG
    }
    // SubShaderが失敗した時に呼ばれる
    FallBack "Diffuse"
}
