// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/ahan3"
{
	Properties
	{
		[PerRendererData]_MainTex( "Sprite Texture", 2D ) = "white" {}
		_Color( "Tint", Color ) = ( 1,1,1,1 )
		[MaterialToggle]PixelSnap( "Pixel snap", Float ) = 0
		[HideInInspector]_RendererColor( "RendererColor", Color ) = ( 1,1,1,1 )
		[HideInInspector]_Flip( "Flip", Vector ) = ( 1,1,1,1 )
		[PerRendererData]_AlphaTex( "External Alpha", 2D ) = "white" {}
		[PerRendererData]_EnableExternalAlpha( "Enable External Alpha", Float ) = 0
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
				#pragma vertex SpriteVert
				#pragma fragment SpriteFrag

				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnitySprites.cginc"

				struct v2fCustom
				{
					float4 vertex	: SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord	: TEXCOORD0;
					float  alpha : TEXCOORD1;
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

					//c.a *= saturate( length() );

					c.rgb *= c.a;

					return c;
				}

			ENDCG
			}
		}
}
