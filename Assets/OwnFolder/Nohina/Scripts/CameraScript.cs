using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	//プレイヤーオブジェクト取得
	[SerializeField] Transform player;

	//パッドの感度
	[Range( 0, 20 )]
	[SerializeField] float padSensitivity;

	//マウスの感度
	[Range( 0, 20 )]
	[SerializeField] float mouseSensitivity;

	/// <summary>
	/// ヨー・ピッチ
	/// ヨーは中心をY軸とした回転(プレイヤーが回転する)
	/// ピッチは中心をX軸とした回転
	/// </summary>
	float pitchClamp = 0;

	float yawblend = 0;
	float pitchblend = 0;

	// Update is called once per frame
	void Update()
	{

		//マウスロック判定
		GameManager.Instance.MouseLockComp();

		//パッド入力かマウス入力かを判定
		GetInputAxis();
	}

	void GetInputAxis()
	{
		if( GameManager.Instance.cursorLock == true )
		{
			if( GameManager.Instance.padMode == true )
			{
				//パッド操作

				float yaw = Input.GetAxis( "RightHorizontal" ) * padSensitivity;
				float pitch = Input.GetAxis( "RightVertical" ) * padSensitivity;
				yawblend = yaw;
				pitchblend = pitch;

				//カメラ回転
				RotateCamera( yaw, pitch );
			}
			else
			{
				//マウス操作

				float yaw = Input.GetAxis( "Mouse X" ) * mouseSensitivity;
				float pitch = Input.GetAxis( "Mouse Y" ) * mouseSensitivity;

				//カメラ回転
				RotateCamera( yaw, -pitch );
			}
		}
	}

	//カメラ回転(ヨーはプレイヤー回転)
	void RotateCamera( float yaw, float pitch )
	{
		pitchClamp += pitch;

		//ヨーでプレイヤーを回転させる
		player.Rotate( 0, yaw, 0 );

		pitch = Mathf.Clamp( pitch, -80, 60 );

		if( pitchClamp > -80 && pitchClamp < 60 )
		{
			transform.RotateAround( player.position, transform.right, pitch );
		}
	}

	//private void OnGUI()
	//{
	//	GUI.Box( new Rect( 0, 0, 150, 60 ), "Yaw = " + yawblend.ToString() );
	//	GUI.Box( new Rect( 0, 60, 150, 60 ), "Pit = " + pitchblend.ToString() );
	//}

}
