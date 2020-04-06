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
		HideProcess();

		if( directionRight == true && Input.GetAxisRaw( "Horizontal" ) == -1 )
		{
			myRenderer.flipX = true;
			directionRight = false;
		}
		else if( directionRight == false && Input.GetAxisRaw( "Horizontal" ) == 1 )
		{
			myRenderer.flipX = false;
			directionRight = true;
		}
	}

	private void FixedUpdate()
	{
		PlayerMotion();
	}

	//プレイヤーを移動させる関数
	void PlayerMotion()
	{
		MovePlayer();
	}

	//プレイヤー移動(水平方向だけの移動)
	void MovePlayer()
	{
		//変数宣言
		float horizontal = Input.GetAxis( "Horizontal" ) * a_PlayerSpeedPre;
		
		transform.Translate( horizontal * playerSpeed, 0f, 0f );
	}

	//OnCollisionEnterで当たったらEnemy TagをSetActive( false )にする
	private void OnCollisionEnter2D( Collision2D collision )
	{
		//if( collision.collider.tag == "Finish" )
		//{
		//	collision.gameObject.SetActive( false );
		//}

		//if( collision.collider.tag == "OverTrigger" )
		//{
		//	myCollider[ 0 ].enabled = false;
		//}

		//if( collision.collider.tag == "UnderTrigger" )
		//{
		//	Transform parentTransform = collision.transform.root.gameObject.transform;
		//	Vector2 parentPosition = parentTransform.position + parentTransform.up;
		//	transform.position = parentPosition;
		//}
	}

	private void OnTriggerEnter2D( Collider2D collision )
	{
		//場所によって移動先を指定する
		if( collision.tag == "OverTrigger" )
		{
			_hideFlg = true;
			hidePosition = collision.transform.parent.position + Vector3.down;
		}
		else if( collision.tag == "UnderTrigger" )
		{
			_hideFlg = true;
			hidePosition = collision.transform.parent.position + Vector3.up;
		}
		print( "Enter" );
	}

	private void OnTriggerExit2D( Collider2D collision )
	{
		if( collision.tag == "OverTrigger" || collision.tag == "UnderTrigger" )
		{
			_hideFlg = false;
			hidePosition = Vector2.zero;
			print( "exit" );
		}
	}

	//プレイヤーが隠れる処理
	public void HideProcess()
	{
		if( _hideFlg == true && Input.GetButtonDown( "Hide" ) )
		{
			transform.position = hidePosition;
			hidePosition = Vector2.zero;
			_hideFlg = false;
		}
	}

	void GrabEnemy()
	{
		if( !( Input.GetButton( "Touch" ) ) && grabFrame <= 10 )
		{

		}
	}

}
