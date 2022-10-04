Shader "Custom/Test1"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
            Stencil {
                Ref 2
                Comp always
                Pass replace
                ZFail decrWrap
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			// ���_�V�F�[�_�[
			float4 vert(float4 vertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// �����ɂ���
				return fixed4(0, 0, 0, 0);
			}
			ENDCG
		}
	}
}
