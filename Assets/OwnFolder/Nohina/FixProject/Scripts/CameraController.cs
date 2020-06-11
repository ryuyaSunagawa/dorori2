using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	/// <summary>
	/// カメラがターゲッティングするオブジェクト
	/// </summary>
	[SerializeField] Transform targetObject;
	[SerializeField] Transform enemyObject;

	[SerializeField, Range( 0f, 1f ), Header( "プレイヤーの横ずれ" ) ] float xDistance = 0.5f;

	[SerializeField, Range( 0f, 1f ), Header( "プレイヤーの縦wずれ" ) ] float yDistance = 0.5f;

	[SerializeField, Header( "敵と自分の中間点にカメラを置くか" ) ] bool chaseAllMen = true;
	

	/// <summary>
	/// 遅れる速度(かな?)
	/// </summary>
	[SerializeField, Range( 0f, 0.5f), Header( "カメラの遅延量" ) ] float smoothDegree = 0.2f;

	/// <summary>
	/// 移動速度
	/// </summary>
	Vector3 moveVelocity;

	/// <summary>
	/// 自分のカメラ取得
	/// </summary>
	Camera myCamera = null;

	Vector3 defaultViewportPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
		myCamera = GetComponent<Camera>();
		//defaultViewportPosition = myCamera.WorldToViewportPoint( targetObject.position );
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		Vector3 myPosition = transform.position;
		Vector3 realTargettingPosition = targetObject.position;

		Vector3 cameraRightEndPosition = myCamera.ViewportToWorldPoint( new Vector3( 1.0f, 0.0f, 0.0f ) );

		if( enemyObject )
		{
			Vector3 ahan = enemyObject.position - cameraRightEndPosition;
			//Debug.Log( ahan );

			if( ahan.x < 1 && ahan.x > -38 && chaseAllMen )
			{
				realTargettingPosition = Vector3.Lerp( enemyObject.position, targetObject.position, 0.5f );

				realTargettingPosition.z = -10;
			}
		}

		Vector3 targetViewportPoint = myCamera.WorldToViewportPoint( realTargettingPosition );
		Vector3 delta = realTargettingPosition - myCamera.ViewportToWorldPoint( new Vector3( xDistance, yDistance, targetViewportPoint.z ) );
		Vector3 destination = myPosition + delta;

		transform.position = Vector3.SmoothDamp( myPosition, destination, ref moveVelocity, smoothDegree );
	}
}
