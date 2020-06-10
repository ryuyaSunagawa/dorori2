Shader "Sprites/Gradient_Alpha"
{
	Properties
	{
		_MainTex( "Sprite Texture", 2D ) = "white" {}
		_Color( "Tint", Color ) = ( 1,1,1,1 )
		PixelSnap( "Pixel snap", Float ) = 0
		_RendererColor( "RendererColor", Color ) = ( 1,1,1,1 )
		_Flip( "Flip", Vector ) = ( 1,1,1,1 )
		_AlphaTex( "External Alpha", 2D ) = "white" {}
		_EnableExternalAlpha( "Enable External Alpha", Float ) = 0

			_GradientAlpha1( "Alpha 1", Range( 0, 1 ) ) = 0
			_GradientAlpha2( "Alpha 2", Range( 0, 1 ) ) = 1
	}

		SubShader
			{
				Tags
				{
					"Queue" = "Transparent"
					"IgnoreProjector" = "True"
					"RenderType" = "Transparent"
					"PreviewType" = "Plane"
					"CanUseSpriteAtlas" = "True"
				}

				Cull Off
				Lighting Off
				ZWrite Off
				Blend One OneMinusSrcAlpha

				Pass
				{
				CGPROGRAM
				// �o�[�e�b�N�X�V�F�[�_�[�A�t���O�����g�V�F�[�_�[��Ǝ��d�l�ɍ����ւ�
				#pragma vertex vert
				#pragma fragment frag

				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnitySprites.cginc"

				// �ǉ������v���p�e�B�̂��߂̕ϐ���ǉ�
				float _GradientAlpha1;
				float _GradientAlpha2;
				float _GradientScale;
				float _GradientAngle;
				float _GradientOffset;

				// v2f���J�X�^�}�C�Y����
				struct v2fCustom
				{
					float4 vertex    : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float  alpha : TEXCOORD1; // �O���f�[�V�����`��̂��߂̃A���t�@����ǉ�
					UNITY_VERTEX_OUTPUT_STEREO
				};

				v2fCustom vert( appdata_t IN )
				{
					v2fCustom OUT;

					UNITY_SETUP_INSTANCE_ID( IN );
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( OUT );

					OUT.vertex = UnityFlipSprite( IN.vertex, _Flip );
					OUT.vertex = UnityObjectToClipPos( OUT.vertex );
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color * _RendererColor;

					return OUT;
				}

				fixed4 frag( v2fCustom IN ) : SV_Target
				{
					fixed4 c = SampleSpriteTexture( IN.texcoord ) * IN.color;

					// �v���p�e�B����ݒ肵���A���t�@1�ƃA���t�@2���o�[�e�b�N�X�V�F�[�_�[�ŋ��߂��䗦��
					// �������A0�`1�ɂ����߂ăA���t�@�ɏ�Z����
					c.a *= saturate( _GradientAlpha1 - ( length( 0.5 - IN.texcoord ) * _GradientAlpha2 ) );

					c.rgb *= c.a;

				return c;
			}
		ENDCG
		}
			}
}