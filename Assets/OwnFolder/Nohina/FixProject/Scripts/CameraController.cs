using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	/// <summary>
	/// カメラがターゲッティングするオブジェクト
	/// </summary>
	[SerializeField] Transform targetObject;

	/// <summary>
	/// 遅れる速度(かな?)
	/// </summary>
	[SerializeField, Range( 0f, 0.5f)] float SmoothDegree = 0.2f;

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
		defaultViewportPosition = myCamera.WorldToViewportPoint( targetObject.position );
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		Vector3 myPosition = transform.position;
		Vector3 targetViewportPoint = myCamera.WorldToViewportPoint( targetObject.position );
		Vector3 delta = targetObject.position - myCamera.ViewportToWorldPoint( new Vector3( defaultViewportPosition.x, defaultViewportPosition.y, targetViewportPoint.z ) );
		Vector3 destination = myPosition + delta;

		transform.position = Vector3.SmoothDamp( myPosition, destination, ref moveVelocity, SmoothDegree );
	}
}
