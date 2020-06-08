// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/ahan2"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

			[Space]
			[Header(Gradient)]
			_GradientAlpha1( "Alpha 1", Range( 0, 1 ) ) = 0
			_GradientAlpha2( "Alpha 2", Range( 0, 1 ) ) = 1
			_GradientScale( "Scale", Range( 0, 2 ) ) = 1
			_GradientAngle( "Angle", Range( 0, 360 ) ) = 0
			_GradientOffset( "Offset", Range( -1, 1 ) ) = 0
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
					#pragma vertex vert
					#pragma fragment frag

					#pragma target 2.0
					#pragma multi_compile_instancing
					#pragma multi_compile _ PIXELSNAP_ON
					#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
					#include "UnitySprites.cginc"

					float _GradientAlpha1;
					float _GradientAlpha2;
					float _GradientScale;
					float _GradientAngle;
					float _GradientOffset;

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

						float theta = _GradientAngle * UNITY_PI / 180;
						OUT.alpha = ( ( dot( float2( cos( theta ), -sin( theta ) ), IN.vertex.xy ) + _GradientOffset * 2 ) / _GradientScale ) + 0.5;

						#ifdef PIXELSNAP_ON
						OUT.vertex = UnityPixelSnap( OUT.vertex );
						#endif

						return OUT;
					}

					fixed4 frag( v2fCustom IN ) : SV_Target
					{
						fixed4 c = SampleSpriteTexture( IN.texcoord ) * IN.color;

						c.a *= saturate( lerp( _GradientAlpha1, _GradientAlpha2, IN.alpha ) );

						c.rgb *= c.a;

						return c;
					}
        ENDCG
        }
    }
}
