using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveActorClass : MonoBehaviour
{
	//[Range( 0, 1 )]
	//public float actorSpeed;

	//入力方向を返す
	public float ReturnDirectFloat( string direct, KeyCode keyDirect )
	{
		//キー水平(A,D)をA=-1、D=1で返す
		if( direct == "Horizontal" )
		{

			if( keyDirect == KeyCode.A )
			{
				return -1.0f;
			}
			else if( keyDirect == KeyCode.D )
			{
				return 1.0f;
			}

			return 0f;
		}
		//キー(W,S)をS=-1、W=1で返す
		else if( direct == "Vertical" )
		{

			if( keyDirect == KeyCode.S )
			{
				return -1.0f;
			}
			else if( keyDirect == KeyCode.W )
			{
				return 1.0f;
			}

			return 0f;
		}

		return 0f;
	}

	//プレイヤー移動
	public void MoveActor( float horizon, float vertical, float actorSpeed )
	{
		transform.Translate( horizon * actorSpeed, 0f, vertical * actorSpeed );
	}
}
