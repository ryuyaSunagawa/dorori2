using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript2D : MonoBehaviour
{
	//プレイヤーのスピード
	const float a_PlayerSpeedPre = 0.5f;
	const float a_PlayerSpeed = 0;
	[SerializeField] [Range( 0, 1 )] float playerSpeed;

	//ジャンプ力
	const float a_JumpForce = 0;
	[SerializeField] [Range( 0, 10 )] float jumpForce;

	private Rigidbody2D myRigidbody;
	private BoxCollider2D myCollider;
	
	public string verifyObject;

	bool downFlg = false;

	int aho = 0;

	private bool _hideFlg;
	public bool hideFlg
	{
		set {
			_hideFlg = value;
		}
	}


	// Start is called before the first frame update
	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		HideFlag();
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
		float horizontal = 0;

		//傾き取得
		horizontal = Input.GetAxis( "Horizontal" ) * a_PlayerSpeedPre;

		//移動
		transform.Translate( horizontal * playerSpeed, 0f, 0f );
	}

	//OnCollisionEnterで当たったらEnemy TagをSetActive( false )にする
	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( collision.collider.tag == "Finish" )
		{
			collision.gameObject.SetActive( false );
		}
	}

	//CollisionStay2D
	private void OnCollisionStay2D( Collision2D collision )
	{
		//if( collision.gameObject.tag == "DownFloor" && Input.GetButton( "Hide" ) && downFlg == false )
		//{
		//	downFlg = true;
		//	collision.gameObject.SetActive( false );
		//}

		//if( collision.gameObject.tag == "DownFloor" )
		//{

		//}
		//print( aho );
		//aho++;
		//verifyObject = collision.transform.name;

		//myCollider.isTrigger = true;
	}

	//CollisionExit2D
	private void OnCollisionExit2D( Collision2D collision )
	{
		if( collision.gameObject.tag == "DownFloor" )
		{
			downFlg = false;
			collision.gameObject.SetActive( true );
		}
	}

	private void OnTriggerExit2D( Collider2D collision )
	{
		myCollider.isTrigger = false;
	}

	//プレイヤーが隠れる処理
	public bool HideFlag()
	{
		return true;
	}
}
