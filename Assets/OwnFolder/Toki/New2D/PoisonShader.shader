Shader "Unlit/PoisonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PoisonTex("Texture", 2D) = "white"{}
		_Color("Color", Color) = (1,1,1,1)
		_ScrollY("Scrool Y", float) = 0
		_Down("Down", float) = 2

		_Disolve("Texture(RGB)", 2D) = "white"{}
		_Alpha("Threshold", Range(0, 1)) = 0.0

    }
    SubShader
    {
		Tags {"Queue" = "Transparent"}// "RenderType" = "Transparent" }
        LOD 100
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM	//開始の合図
            #pragma vertex vert		//頂点シェーダの関数名
            #pragma fragment frag	//フラグメントシェーダの関数名
            //make fog work
            #pragma multi_compile_fog//コンパイル時にフォグがオンになっているかで自動で切り替える

            #include "UnityCG.cginc"

			//頂点一つのデータ
            struct appdata
            {
                float4 vertex : POSITION;	//頂点のポジションをfloat4型に入れる
                float2 uv_main : TEXCOORD0;		//テクスチャUV、敵のテクスチャ
				float2 uv_poison :TEXCOORD1;	//テクスチャUV、毒のテクスチャ
            };

			//頂点をラスタライズしてフラグメントに渡すデータの構造体
            struct v2f
            {
                float2 uv_main : TEXCOORD0;		//テクスチャUVゼロ(TEXCOORDは万能だけど8個までにしとけ)
				float2 uv_poison : TEXCOORD1;	//テクスチャUV1
                UNITY_FOG_COORDS(2)				//float1 fogCord : TEXCOORD1;になるらしい
                float4 vertex : SV_POSITION;	//座標変換後の値
            };

			


            sampler2D _MainTex;	//テクスチャ

			sampler2D _Disolve;	//敵の消すところのテクスチャ

			half _Alpha;	//透明が適用される強度

            float4 _MainTex_ST;	//TilingとOffsetのxyが入ってる

			sampler2D _PoisonTex;

			float4 _PoisonTex_ST;

			fixed4 _Color;		//カラー

			float _ScrollY;

			float _Down;

            v2f vert (appdata v)	//頂点シェーダからフラグメントシェーダにデータを渡す
            {
                v2f o;

				if (_Down > 0)
				{
					_Down -= _Time.y * 0.45;
				}
				if (v.vertex.y > _Down)
				{
					v.vertex.y += _Down * 0.45;
				}

				
                o.vertex = UnityObjectToClipPos(v.vertex);	//座標変換する処理
				
                //o.uv_main = TRANSFORM_TEX(v.uv_main, _MainTex);			//タイリングとオフセットをUVに反映(o.uv = v.uv.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;)
				o.uv_main = v.uv_main;
				o.uv_poison = TRANSFORM_TEX(v.uv_poison, _PoisonTex);	//poisonのuvに反映
                UNITY_TRANSFER_FOG(o,o.vertex);				//o.fogCoord.x = o.vertex.z;になるらしい
                return o;
            }

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 m = tex2D(_Disolve, i.uv_main);
				half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
				_Alpha += _Time.y * 0.15;

				if (g < _Alpha)
				{
					discard;
				}

				float2 scroll = float2(0, _ScrollY) * _Time.y;
                // sample the texture
				fixed4 color_main = tex2D(_MainTex, i.uv_main);	//場所とテクスチャ

				if(color_main.a >= 1.0f)
				{
					color_main = fixed4(1,1,1,1);
				}

				fixed4 color_poison = tex2D(_PoisonTex, i.uv_poison + scroll);

				fixed4 col = color_main * color_poison;
				//fixed4 col = color_poison;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				return col * _Color;
            }
            ENDCG	//終了合図
        }
    }
}
