﻿using System.Collections;
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
	int stair = 1;
	Transform stairComp;
	string tag = "";
	bool nowHide = false;

	public bool nearHide = false;

	LayerMask maskLayer;
	int plusInt = 0;

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
		//HideProcess();

		TouchEnemy();

		//hideComp();

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

		if( stair == 0 )
		{
			transform.Translate( horizontal * 0.15f, 0f, 0f );
		}
		else
		{
			transform.Translate( horizontal * playerSpeed, 0f, 0f );
		}
	}

	private void OnTriggerStay2D( Collider2D collision )
	{
		if( collision.transform.tag == "UnderTrigger" && Input.GetButtonDown( "Hide" ) )
		{
			transform.position = collision.transform.parent.GetChild( 0 ).position;
			GetComponent<SpriteRenderer>().flipY = true;
		}
		else if( collision.transform.tag == "OverTrigger" && Input.GetButtonDown( "Hide" ) )
		{
			transform.position = collision.transform.parent.GetChild( 1 ).position;
			GetComponent<SpriteRenderer>().flipY = false;
		}
		else if( collision.tag == "BackHideTrigger" && nowHide == false )
		{
			nowHide = true;
			transform.position += new Vector3( 0, 0, 2 );
		}
		else if( collision.tag == "BackHideTrigger" && nowHide == true )
		{
			nowHide = true;
			transform.position -= new Vector3( 0, 0, 2 );
		}

		Debug.Log( collision.tag.ToString() );
	}

	//OnCollisionEnterで当たったらEnemy TagをSetActive( false )にする
	//private void OnCollisionEnter2D( Collision2D collision )
	//{
	//	//if( collision.collider.tag == "Finish" )
	//	//{
	//	//	collision.gameObject.SetActive( false );
	//	//}

	//	//if( collision.collider.tag == "OverTrigger" )
	//	//{
	//	//	myCollider[ 0 ].enabled = false;
	//	//}

	//	//if( collision.collider.tag == "UnderTrigger" )
	//	//{
	//	//	Transform parentTransform = collision.transform.root.gameObject.transform;
	//	//	Vector2 parentPosition = parentTransform.position + parentTransform.up;
	//	//	transform.position = parentPosition;
	//	//}
		
	//}

	//OnTriggerStay難しい版!
	private void hideComp()
	{
		//Collider2D[] results = new Collider2D[ 10 ];
		//int collisionCount = myCollider.OverlapCollider( new ContactFilter2D(), results );
		//Debug.Log( collisionCount );

		//if( collisionCount > 0 )
		//{
		//	for( int i = 0; i < collisionCount; i++ )
		//	{
		//		if( results[ i ].transform.tag == "OverTrigger" || results[ i ].transform.tag == "UnderTrigger" || results[ i ].transform.tag == "BackHideTrigger" )
		//		{
		//			nearHide = true;
		//		}
		//		else
		//		{
		//			nearHide = false;
		//		}
		//	}
		//}

		//Collider2D[] overlap = Physics2D.OverlapBox( transform.position, myCollider.size, 0f );
	}

	private void OnTriggerExit2D( Collider2D collision )
	{
		//if( collision.tag == "OverTrigger" || collision.tag == "UnderTrigger" || collision.tag == "BackHideTrigger" )
		//{
		//	_hideFlg = false;
		//	hidePosition = Vector2.zero;
		//	print( "exit" );
		//	stairComp = null;
		//}
	}

	private void OnTriggerEnter2D( Collider2D collision )
	{
		//場所によって移動先を指定する
		//if( collision.tag == "OverTrigger" )
		//{
		//	_hideFlg = true;
		//	//hidePosition = collision.transform.parent.position + Vector3.down;
		//	hidePosition = collision.transform.parent.GetChild( 1 ).position;
		//	stairComp = collision.transform;
		//	tag = collision.tag;
		//}
		//else if( collision.tag == "UnderTrigger" )
		//{
		//	_hideFlg = true;
		//	//hidePosition = collision.transform.parent.position + Vector3.up;
		//	hidePosition = collision.transform.parent.GetChild( 0 ).position;
		//	stairComp = collision.transform;
		//	tag = collision.tag;
		//}
		//if( collision.tag == "BackHideTrigger" )
		//{
		//	_hideFlg = true;
		//	hidePosition = collision.transform.position;
		//	tag = collision.tag;
		//}

		//print( "Enter" );
	}

	//プレイヤーが隠れる処理
	//public void HideProcess()
	//{
	//	if( _hideFlg == true && Input.GetButtonDown( "Hide" ) )
	//	{
	//		if( tag == "OverTrigger" )
	//		{
	//			transform.position = hidePosition;
	//			hidePosition = Vector2.zero;
	//		}
	//		else if( tag == "UnderTrigger" )
	//		{
	//			transform.position = hidePosition;
	//			hidePosition = Vector2.zero;
	//			myRigidbody.simulated = false;
	//		}
	//		else if( tag == "BackHideTrigger" && nowHide == false )
	//		{
	//			gameObject.layer = 15;
	//			nowHide = true;
	//			transform.position += new Vector3( 0, 0, 2 );
	//		}
	//		else if( tag == "BackHideTrigger" && nowHide == true )
	//		{
	//			gameObject.layer = 13;
	//			nowHide = false;
	//			transform.position -= new Vector3( 0, 0, 2 );
	//		}
	//	}
	//}

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
