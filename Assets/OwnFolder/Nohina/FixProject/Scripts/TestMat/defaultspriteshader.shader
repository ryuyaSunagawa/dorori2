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
				// バーテックスシェーダー、フラグメントシェーダーを独自仕様に差し替え
				#pragma vertex vert
				#pragma fragment frag

				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnitySprites.cginc"

				// 追加したプロパティのための変数を追加
				float _GradientAlpha1;
				float _GradientAlpha2;
				float _GradientScale;
				float _GradientAngle;
				float _GradientOffset;

				// v2fもカスタマイズする
				struct v2fCustom
				{
					float4 vertex    : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float  alpha : TEXCOORD1; // グラデーション描画のためのアルファ情報を追加
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

					// プロパティから設定したアルファ1とアルファ2をバーテックスシェーダーで求めた比率で
					// 混合し、0～1におさめてアルファに乗算する
					c.a *= saturate( _GradientAlpha1 - ( length( 0.5 - IN.texcoord ) * _GradientAlpha2 ) );

					c.rgb *= c.a;

				return c;
			}
		ENDCG
		}
			}
}