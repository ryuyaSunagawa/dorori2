using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerScript2D : MonoBehaviour
{
	//プレイヤースピード変数
	const float a_PlayerSpeedPre = 0.5f;
	const float a_PlayerSpeed = 0;
	[SerializeField] [Range( 0, 1 )] float playerSpeed;

	/// <summary>
	/// 自分のコンポーネント
	/// </summary>
	//Rigidbody2D
	private Rigidbody2D myRigidbody;
	//Box Collider 2D
	private BoxCollider2D myCollider;
	//Sprite Renderer
	private SpriteRenderer myRenderer;
	//子オブジェクトのEnemyDistanceTriggerのBoxCollider2Dを取得する
	[SerializeField] DistanceTriggerScript dtsChild;

	[SerializeField] Sprite[] player = new Sprite[ 10 ];

	//隠れるフラグ
	public bool _hideFlg;
	public bool hideFlg
	{
		set {
			_hideFlg = value;
		}
	}
	
	//隠れるポジションを取得
	private Vector2 hidePosition;

	//タッチできる範囲
	private bool touchFlg = false;
	//つかんだ時間
	private int grabFrame = 0;

	//テレポート先のVector2 Position
	private Vector2 _nextPosition;
	public Vector2 nextPosition
	{
		set {
			_nextPosition = value;
		}
	}

	//右を向いているとtrue
	bool directionRight = true;

	//敵を吹っ飛ばす力
	[SerializeField] float enemyBlowPower = 3f;

	GameObject nearEnemy;

	//階層( 0=Neutral,1=Higher,2=Lower) 
	[SerializeField] int stair = 0;
	Transform stairComp;

	//現在隠れているか
	bool nowHide = false;
	//Layerを変更するときに使う
	LayerMask maskLayer;
	//隠れたり戻るときに1秒待つためにカウントするやつ
	[SerializeField] float hideWaitTime = 0;

	//隠れるボタンのフラグ
	bool hideButton = false;

	/// <summary>
	/// アニメーション関連
	/// </summary>
	/// 
	//隠れるアニメーションのフラグ
	bool hideAnimation = false;
	//deltaTimeを加算する
	float timeCount = 0f;
	//現在のSpriteの番号
	int texValue = 1;
	//使うAtlasの指定(0=back,1=up,2=down)
	int useAtlasValue = 0;

	//SpriteAtlasの指定(0=back,1=up,2=down)
	[SerializeField] SpriteAtlas[] doronAnime = new SpriteAtlas[ 3 ];


	// Start is called before the first frame update
	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		myRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		//敵を触る
		TouchEnemy();

		//自分の向きの調整(Sprite)
		SpriteDirect();

		if( hideAnimation == false )
		{
			myRenderer.sprite = player[ stair ];
		}

		//隠れる状態になっているか(ボタンが押されているか)確認
		NowHideStateVerify();

		//隠れるポジションに移動する
		//MoveHidePosition();
		HideAnimator();
	}

	private void FixedUpdate()
	{
		MovePlayer();
	}

	/// <summary>
	/// プレイヤーの移動・方向関連
	/// </summary>
	///
	void MovePlayer()
	{
		//変数宣言
		float horizontal = Input.GetAxis( "Horizontal" ) * a_PlayerSpeedPre;

		if( stair == 2 )
		{
			transform.Translate( horizontal * 0.15f, 0f, 0f );
		}
		else if( ( stair == 0 && nowHide == false ) )
		{
			transform.Translate( horizontal * playerSpeed, 0f, 0f );
		}

		GameManager.Instance.playerStairState = stair;
	}

	//Spriteを入力の方向に合わせる
	void SpriteDirect()
	{
		if( Input.GetAxisRaw( "Horizontal" ) == -1 )
		{
			myRenderer.flipX = true;
			directionRight = false;
		}
		else if( Input.GetAxisRaw( "Horizontal" ) == 1 )
		{
			myRenderer.flipX = false;
			directionRight = true;
		}
	}

	/// <summary>
	/// 隠れる処理
	/// ・毎フレームHideButtonが押されているか確認
	/// ・移動はTriggerEventで処理
	/// </summary>
	/// <param name="collision"></param>
	///
	
	//hideWaitTimeを更新とHideボタンを押したかの確認
	void NowHideStateVerify()
	{
		hideWaitTime += Time.deltaTime;

		if( Input.GetButtonDown( "Hide" ) )
		{
			hideButton = true;
		}
		else if( Input.GetButtonUp( "Hide" ) )
		{
			hideButton = false;
		}
	}

	private void OnTriggerStay2D( Collider2D collision )
	{
		Debug.Log( hideAnimation );
		if( hideWaitTime > 1f )
		{
			if( collision.transform.tag == "UnderTrigger" && collision.transform.parent.tag == "HighStair" && hideButton == true )
			{
				Vector3 nextPos = new Vector3( collision.transform.parent.GetChild( 0 ).position.x, collision.transform.parent.GetChild( 0 ).position.y, transform.position.z );
				//transform.position = nextPosition;
				_nextPosition = nextPos;
				stair = 1;
				hideAnimation = true;
				hideWaitTime = 0;
				useAtlasValue = 1;
				Debug.Log( "next" );
			}
			else if( collision.transform.tag == "UnderTrigger" && collision.transform.parent.tag == "NeutralStair" && hideButton == true )
			{
				Vector3 nextPos = new Vector3( collision.transform.parent.GetChild( 0 ).position.x, collision.transform.parent.GetChild( 0 ).position.y, transform.position.z );
				//transform.position = nextPosition;
				_nextPosition = nextPos;
				stair = 0;
				hideAnimation = true;
				hideWaitTime = 0;
				useAtlasValue = 1;
				Debug.Log( "next" );
			}
			else if( collision.transform.tag == "OverTrigger" && collision.transform.parent.tag == "NeutralStair" && hideButton == true )
			{
				Vector3 nextPos = new Vector3( collision.transform.parent.GetChild( 1 ).position.x, collision.transform.parent.GetChild( 1 ).position.y, transform.position.z );
				//transform.position = nextPosition;
				_nextPosition = nextPos;
				stair = 2;
				hideAnimation = true;
				hideWaitTime = 0;
				useAtlasValue = 2;
				Debug.Log( "next" );
			}
			else if( collision.transform.tag == "OverTrigger" && collision.transform.parent.tag == "HighStair" && hideButton == true )
			{
				Vector3 nextPos = new Vector3( collision.transform.parent.GetChild( 1 ).position.x, collision.transform.parent.GetChild( 1 ).position.y, transform.position.z );
				//transform.position = nextPosition;
				_nextPosition = nextPos;
				stair = 0;
				hideAnimation = true;
				hideWaitTime = 0;
				useAtlasValue = 0;
				Debug.Log( "next" );
			}
			else if( collision.tag == "BackHideTrigger" && hideButton == true && nowHide == false )
			{
				nowHide = true;
				//transform.position = new Vector3( collision.transform.position.x, collision.transform.position.y, 3 );
				_nextPosition = new Vector3( collision.transform.position.x, collision.transform.position.y, 3 );
				hideAnimation = true;
				hideWaitTime = 0;
				gameObject.layer = 15;
				useAtlasValue = 0;
				Debug.Log( "next" );
			}
			else if( collision.tag == "BackHideTrigger" && hideButton == true && nowHide == true )
			{
				nowHide = false;
				//transform.position = new Vector3( collision.transform.position.x, collision.transform.position.y, 0 );
				_nextPosition = new Vector3( collision.transform.position.x, collision.transform.position.y, 0 );
				hideAnimation = true;
				hideWaitTime = 0;
				useAtlasValue = 0;
				Debug.Log( "next" );
			}
			else
			{
				hideButton = false;
			}
		}


			Debug.Log( collision.tag.ToString() );
		}

		//隠れるときの処理
	void HideProcess()
	{
		HideAnimator();
	}

	//隠れるときのアニメーション
	void HideAnimator()
	{
		if( hideAnimation == true )
		{
			timeCount += Time.deltaTime;
			if( timeCount >= 0.05f )
			{
				if( ++texValue >= doronAnime[ useAtlasValue ].spriteCount )
				{
					texValue = 1;
					hideAnimation = false;
					MoveHidePosition();
				}
				timeCount = 0;
			}
			myRenderer.sprite = doronAnime[ useAtlasValue ].GetSprite( texValue.ToString() );
		}
	}

	//隠れるポジションへ移動&初期化
	void MoveHidePosition()
	{
			transform.position = _nextPosition;
			hideAnimation = false;
			hideWaitTime = 0;
			gameObject.layer = 13;
	}

	//敵を触った時に使うやつ
	void TouchEnemy()
	{
		if( dtsChild.IsHitEnemy() )
		{
			if( Input.GetButtonDown( "Touch" ) )
			{
				dtsChild.enemyObject.GetComponent<New2DEnemy>().poisonState = 1;
				nearEnemy = dtsChild.enemyObject;
			}
			else if( Input.GetButton( "Touch" ) )
			{
				if( Input.GetAxisRaw( "Horizontal" ) == 1 )
				{
					StartCoroutine( cor1( enemyBlowPower ) );
				}
				else if( Input.GetAxisRaw( "Horizontal" ) == -1 )
				{
					StartCoroutine( cor1( -enemyBlowPower ) );
				}
			}
			else if( Input.GetButtonUp( "Touch" ) )
			{
				nearEnemy.GetComponent<New2DEnemy>().poisonState = 2;
				nearEnemy = null;
			}
		}
	}

	IEnumerator cor1( float blowPower )
	{
		nearEnemy.GetComponent<Rigidbody2D>().AddForce( new Vector2( blowPower, 0.4f ), ForceMode2D.Impulse );
		gameObject.layer = 11;

		yield return new WaitForSeconds( 2f );

		gameObject.layer = 13;
		nearEnemy.GetComponent<New2DEnemy>().poisonState = 2;

		yield break;
	}

}
