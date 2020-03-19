using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
		DrawPlayerInformation();
    }

	private void FixedUpdate()
	{
		//プレイヤー移動
		MoveProcessor();


	}

	//プレイヤー移動全般
	void MoveProcessor()
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
			float horizontal = ReturnDirectFloat( "Horizontal" );
			float vertical = ReturnDirectFloat( "Vertical" );

			//プレイヤー移動
			MovePlayer( horizontal, vertical );
		}

	}

	//入力方向を返す
	float ReturnDirectFloat( string direct )
	{
		//キー水平(A,D)をA=-1、D=1で返す
		if( direct == "Horizontal" )
		{
			if( Input.GetKey( KeyCode.A ) )
			{
				return -1.0f;
			}
			else if( Input.GetKey( KeyCode.D ) )
			{
				return 1.0f;
			}

			return 0f;
		}
		//キー(W,S)をS=-1、W=1で返す
		else if( direct == "Vertical" )
		{
			if( Input.GetKey( KeyCode.S ) )
			{
				return -1.0f;
			}
			else if( Input.GetKey( KeyCode.W ) )
			{
				return 1.0f;
			}

			return 0f;
		}

		return 0f;
	}

	//プレイヤー移動
	void MovePlayer( float horizon, float vertical )
	{
		transform.Translate( horizon * playerSpeed, 0f, vertical * playerSpeed );
	}

	//押されたキーの数
	int GetDownKeyNumber()
	{
		int downKeyNumber = 0;

		if( Input.anyKey )
		{
			foreach( KeyCode code in Enum.GetValues( typeof( KeyCode ) ) )
			{
				if( Input.GetKey( code ) )
				{
					downKeyNumber++;
				}
			}
		}

		return downKeyNumber;
	}

	//プレイヤーの情報表示
	void DrawPlayerInformation()
	{
		//実験的にRayを作ってみる
		Ray ray = new Ray( transform.position, transform.forward );
		Debug.DrawRay( ray.origin, ray.direction * 5, Color.blue );
		Debug.DrawLine( transform.position, transform.position + transform.forward, Color.green );

		//TextMeshProに描画
		myTmp.GetComponent<TextMeshPro>().text = "Position = " + transform.position + ", \n"
											   + "DownKeyNum = " + GetDownKeyNumber() + "\n"
											   + "Normalize = " + transform.forward + "\n"
											   + "ray = " + ray.origin + ", " + ray.direction;
	}
}
