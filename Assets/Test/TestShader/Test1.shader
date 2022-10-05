Shader "Custom/Test1"
{
    // �v���p�e�B
    Properties{
        // �e�N�X�`��
        _MainTex("Base(RGB)", 2D) = "white" {}
    }

        // Shader�̒��g���L�q
        SubShader{
        // �s�����ȃI�u�W�F�N�g
        Tags { "RenderType" = "Transparent" }
        // �X�e���V��
        Stencil {
                // �o�b�t�@�ɏ������ޒl
                Ref 1
                // ���
                Comp notequal
        // �o�b�t�@�ɏ�������
        Pass replace
    }

        // cg����L�q
        CGPROGRAM
        // �g�U�A����
        #pragma surface surf Lambert alpha

        // Input�\����
        struct Input {
        // �e�N�X�`��
        float2 uv_MainTex;
    };

    // surf�֐�
    void surf(Input IN, inout SurfaceOutput o) {
        o.Albedo = fixed3(0, 0, 0);
        o.Alpha = 0.5f;
    }
    // Shader�̋L�q�I��
    ENDCG
    }
        // SubShader�����s�������ɌĂ΂��
        FallBack "Diffuse"
}
