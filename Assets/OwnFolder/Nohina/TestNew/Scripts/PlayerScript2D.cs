using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	//階層
	[SerializeField] int stair = 0;
	Transform stairComp;

	bool nowHide = false;

	public bool nearHide = false;

	LayerMask maskLayer;

	bool backFlg = false;

	[SerializeField] float hideWaitTime = 0;

	bool hideButton = false;

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

		TouchEnemy();

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

		myRenderer.sprite = player[ stair ];

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

	private void FixedUpdate()
	{
		MovePlayer();
	}

	/// <summary>
	/// プレイヤーの移動f
	/// </summary>
	/// 
	void MovePlayer()
	{
		//変数宣言
		float horizontal = Input.GetAxis( "Horizontal" ) * a_PlayerSpeedPre;

		if( stair == 3 )
		{
			transform.Translate( horizontal * 0.15f, 0f, 0f );
		}
		else if( ( stair == 0 && nowHide == false ) || ( stair == 1 && nowHide == false ) )
		{
			transform.Translate( horizontal * playerSpeed, 0f, 0f );
		}
	}

	/// <summary>
	/// TriggerEvent(今回は隠れるときに使う)
	/// </summary>
	/// <param name="collision"></param>
	/// 
	private void OnTriggerStay2D( Collider2D collision )
	{
		if( hideWaitTime > 1f )
		{
			if( collision.transform.tag == "UnderTrigger" && collision.transform.parent.tag == "HighStair" && hideButton == true )
			{
				Vector3 nextPosition = new Vector3( collision.transform.parent.GetChild( 0 ).position.x, collision.transform.parent.GetChild( 0 ).position.y, transform.position.z );
				transform.position = nextPosition;
				stair = 2;
				hideWaitTime = 0;
			}
			else if( collision.transform.tag == "UnderTrigger" && collision.transform.parent.tag == "NeutralStair" && hideButton == true )
			{
				Vector3 nextPosition = new Vector3( collision.transform.parent.GetChild( 0 ).position.x, collision.transform.parent.GetChild( 0 ).position.y, transform.position.z );
				transform.position = nextPosition;
				stair = 0;
				hideWaitTime = 0;
			}
			else if( collision.transform.tag == "OverTrigger" && collision.transform.parent.tag == "NeutralStair" && hideButton == true )
			{
				Vector3 nextPosition = new Vector3( collision.transform.parent.GetChild( 1 ).position.x, collision.transform.parent.GetChild( 1 ).position.y, transform.position.z );
				transform.position = nextPosition;
				stair = 3;
				hideWaitTime = 0;
			}
			else if( collision.transform.tag == "OverTrigger" && collision.transform.parent.tag == "HighStair" && hideButton == true )
			{
				Vector3 nextPosition = new Vector3( collision.transform.parent.GetChild( 1 ).position.x, collision.transform.parent.GetChild( 1 ).position.y, transform.position.z );
				transform.position = nextPosition;
				GetComponent<SpriteRenderer>().flipY = false;
				stair = 0;
				hideWaitTime = 0;
			}
			else if( collision.tag == "BackHideTrigger" && hideButton == true && nowHide == false )
			{
				Debug.Log( "in" );
				nowHide = true;
				transform.position = new Vector3( collision.transform.position.x, collision.transform.position.y, 3 );
				hideWaitTime = 0;
				gameObject.layer = 15;
			}
			else if( collision.tag == "BackHideTrigger" && hideButton == true && nowHide == true )
			{
				Debug.Log( "out" );
				nowHide = false;
				transform.position = new Vector3( collision.transform.position.x, collision.transform.position.y, 0 );
				hideWaitTime = 0;
				gameObject.layer = 13;
			}
			else
			{
				hideButton = false;
			}
		}
			

		Debug.Log( collision.tag.ToString() );
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
		nearEnemy.GetComponent<Rigidbody2D>().AddForce( new Vector2( blowPower, 5f ), ForceMode2D.Impulse );
		gameObject.layer = 11;

		yield return new WaitForSeconds( 2f );

		gameObject.layer = 13;
		nearEnemy.GetComponent<New2DEnemy>().poisonState = 2;

		yield break;
	}

}
