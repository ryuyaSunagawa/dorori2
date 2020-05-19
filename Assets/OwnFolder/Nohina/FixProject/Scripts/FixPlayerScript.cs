using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPlayerScript : MonoBehaviour
{
	/// <summary>
	/// プレイヤー系変数
	/// </summary>
	//プレイヤースピード
	[SerializeField, HeaderAttribute ("プレイヤー移動"), Range( 0f, 0.5f )] float playerSpeed_Normal;
	[SerializeField, Range( 0f, 0.5f )] float playerSpeed_Under;

	//現在の階層
	[SerializeField, Space( 10 )] int stair = 0;

	/// <summary>
	/// イベント処理
	/// </summary>

    // Start is called before the first frame update
    void Start()
    {
		Vector2 gravity = Physics2D.gravity;
    }

    // Update is called once per frame
    void Update()
    {
    }

	private void FixedUpdate()
	{
		MovePlayer();
	}


	/// <summary>
	/// プレイヤー処理系
	/// </summary>

	//プレイヤー移動関数
	void MovePlayer()
	{
		//変数宣言
		float horizontal = Input.GetAxis( "Horizontal" );

		//if( stair == 2 )
		//{
		//	transform.Translate( horizontal * 0.15f, 0f, 0f );
		//}
		//else if( ( stair == 0 && nowHide == false ) )
		//{
		//	transform.Translate( horizontal * playerSpeed, 0f, 0f );
		//}

		transform.Translate( horizontal * playerSpeed_Normal, 0f, 0f );

		//GameManager.Instance.playerStairState = stair;
	}

	private void OnTriggerStay2D( Collider2D collision )
	{
		
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		Debug.Log( collision.collider.name );
	}
}
