﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerScript : MonoBehaviour
{

	//プレイヤーのスピード
	[Range( 0, 1 )]
	[SerializeField] float playerSpeed;

	//PlayerInformation取得
	[SerializeField] TextMeshPro myTmp;

	//最新の押されたキーコード
	KeyCode latestKeyCode;

	//回避スピード
	[Range( 0, 2 )]
	[SerializeField] float speed;

	[Range( 0, 10 )]
	[SerializeField] int evationDistance;

	//回避後のポジション
	Vector3 evationPosition;

	bool evationFlg = false;

	Rigidbody rb;
	Animator anm;

	bool sitFlg = false;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		anm = GetComponent<Animator>();
	}

	// Update is called once per frame
	private void Update()
	{
		//プレイヤーの情報をTMPで描画する
		DrawPlayerInformation();

		EnemyKiller();

		EvationPlayer();

		Jump();

		SitPlayer();

	}

	private void FixedUpdate()
	{
		//プレイヤー移動
		MoveProcessor();

	}

	private void OnGUI()
	{
		if( Input.GetButton( "L1" ) )
		{
			GUI.Box( new Rect( 0, 0, 150, 60 ), "L1" );
		}
		if( Input.GetButton( "R1" ) )
		{
			GUI.Box( new Rect( 0, 65, 150, 60 ), "R1" );
		}
		GUI.Label( new Rect( 0, 130, 150, 60 ), latestKeyCode.ToString() );
	}

	//プレイヤー移動全般
	private void MoveProcessor()
	{
		if( GameManager.Instance.cursorLock == true )
		{
			//パッド操作
			if( GameManager.Instance.padMode == true )
			{
				//パッド移動
				float horizontal = Input.GetAxis( "LeftHorizontal" );
				float vertical = Input.GetAxis( "LeftVertical" );

				//プレイヤー移動
				MovePlayer( horizontal, vertical );
			}
			//キーボード操作
			else if( GameManager.Instance.padMode == false )
			{
				//キーボード移動
				float horizontal = Input.GetAxis( "Horizontal" );
				float vertical = Input.GetAxis( "Vertical" );

				//プレイヤー移動
				MovePlayer( horizontal, vertical );
			}
		}

		
	}

	//ジャンプ処理
	void Jump()
	{
		if( Input.GetKeyDown( KeyCode.Space ) || Input.GetButtonDown( "Fire1" ) )
		{
			rb.AddForce( new Vector3( 0f, 7f, 0f ), ForceMode.Impulse );
		}
	}

	void SitPlayer()
	{
		if( Input.GetKeyDown( KeyCode.LeftControl ) ) {
			if( sitFlg == true )
			{
				anm.SetBool( "Sit", false );
				sitFlg = false;
				GameManager.Instance.playerSitdown = false;
			}
			else if( sitFlg == false )
			{
				anm.SetBool( "Sit", true );
				sitFlg = true;
				GameManager.Instance.playerSitdown = true;
			}
		}
	}

	//プレイヤー移動
	private void MovePlayer( float horizon, float vertical )
	{
		transform.Translate( horizon * playerSpeed, 0f, vertical * playerSpeed );

		//回避するときに動作
		if( evationFlg == true )
		{
			transform.position = Vector3.MoveTowards( transform.position, evationPosition, speed );

			float distance = Vector3.Distance( transform.position, evationPosition );
			if( distance == 0 )
			{
				evationFlg = false;
			}
		}
	}

	//押されたキーの数
	private int GetDownKeyNumber()
	{
		int downKeyNumber = 0;

		if( Input.anyKey )
		{
			foreach( KeyCode code in Enum.GetValues( typeof( KeyCode ) ) )
			{
				if( Input.GetKey( code ) )
				{
					latestKeyCode = code;
					downKeyNumber++;
				}
			}
		}

		return downKeyNumber;
	}

	//プレイヤーの情報表示
	private void DrawPlayerInformation()
	{
		//実験的にRayを作ってみる
		//Ray ray = new Ray( transform.position, transform.forward );
		//Debug.DrawRay( ray.origin, ray.direction * 5, Color.blue );
		//Debug.DrawLine( transform.position, transform.position + transform.forward, Color.green );

		//TextMeshProに描画
		myTmp.GetComponent<TextMeshPro>().text = "Position = " + transform.position + ", \n"
											   + "DownKeyNum = " + GetDownKeyNumber() + "\n"
											   + "Normalize = " + transform.forward + "\n"
											   + "3rd Axis = " + Input.GetAxis( "Trigger" );
	}

	//敵をキルする関数
	private void EnemyKiller()
	{
		if( GameManager.Instance.PlayerDistance <= 25 
			&& ( Input.GetAxis( "Trigger" ) < 0 || Input.GetAxis( "Trigger" ) > 0 ) )
		{
			Transform enemyObj = GameManager.Instance.getenemyObj;
			enemyObj.GetComponent<EnemyScriptToki>().Death();
		}
	}

	//L1,R1でそれぞれの方向の1m先に移動する
	private void EvationPlayer()
	{
		if( evationFlg == false && Input.GetButtonDown( "L1" ) )
		{
			evationPosition = transform.position + ( -( transform.right ) * evationDistance );
			evationFlg = true;
		}
		else if( evationFlg == false && Input.GetButtonDown( "R1" ) )
		{
			evationPosition = transform.position + ( transform.right * evationDistance );
			evationFlg = true;
		}
	}

	//private void OnCollisionEnter( Collision collision )
	//{
	//	Debug.Log( collision.gameObject.name );
	//}

}