﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript2D : MonoBehaviour
{
	//プレイヤーのスピード
	const float a_PlayerSpeed = 0;
	[SerializeField] [Range( 0, 5 )] float playerSpeed;

	//ジャンプ力
	const float a_JumpForce = 0;
	[SerializeField] [Range( 0, 10 )] float jumpForce;

	private Rigidbody2D myRigidbody;

	// Start is called before the first frame update
	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		PlayerMotion();
	}

	//プレイヤーを移動させる関数
	void PlayerMotion()
	{
		MovePlayer();

		JumpPlayer();
	}

	//プレイヤー移動(水平方向だけの移動)
	void MovePlayer()
	{
		//変数宣言
		float horizontal = 0;

		//傾き取得
		if( GameManager.Instance.padMode == true )
		{
			horizontal = Input.GetAxis( "LeftHorizontal" );
		}
		else if( GameManager.Instance.padMode == false )
		{
			horizontal = Input.GetAxis( "Horizontal" );
		}

		//移動
		transform.Translate( horizontal * playerSpeed, 0f, 0f );
	}

	//JumpPlayer
	void JumpPlayer()
	{
		if( Input.GetButtonDown( "Jump" ) )
		{
			myRigidbody.AddForce( new Vector2( 0f, jumpForce ), ForceMode2D.Impulse );
		}
	}
}
