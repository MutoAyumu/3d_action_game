Shader "Custom/Test2"
{
    // �v���p�e�B
    Properties{
        // �e�N�X�`��
        _MainTex("Base(RGB)", 2D) = "white" {}
    }

        // Shader�̒��g���L�q
        SubShader{
        // ��ʓI��Shader���g�p
        Tags { "RenderType" = "Opaque + 1" }
        // �X�e���V��
        Stencil {
                // �o�b�t�@�ɏ������ޒl
                Ref 1
                // �o�b�t�@����������
                Comp notequal
        // �o�b�t�@�ɕێ�
        Pass keep
    }

        // cg����L�q
        CGPROGRAM
        // �g�U
        #pragma surface surf Lambert

        // �e�N�X�`��
        sampler2D _MainTex;

    // Input�\����
    struct Input {
        // �e�N�X�`��
        float2 uv_MainTex;
    };

    // surf�֐�
    void surf(Input IN, inout SurfaceOutput o) {
        // �e�N�X�`���̃s�N�Z���̐F��Ԃ�
        half4 c = tex2D(_MainTex, IN.uv_MainTex);
        o.Albedo = c.rgb;
        o.Alpha = c.a;
    }
    // Shader�̋L�q�I��
    ENDCG
    }
        // SubShader�����s�������ɌĂ΂��
        FallBack "Diffuse"
}
