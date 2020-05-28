using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FixPlayerScript : MonoBehaviour
{
	//自分のスプライトを保持する
	SpriteRenderer myRenderer = null;

	//自分のスプライト
	[SerializeField] Sprite nowSprite = null;

	Sprite[] mySprite = new Sprite[ 6 ];

	//プレイヤースピード
	[SerializeField, HeaderAttribute ("プレイヤー移動"), Range( 0f, 0.5f )] float playerSpeed_Normal;

	//プレイヤーの隠れ先ポジション
	Vector3 _nextPosition;

	//右向いてるか
	bool directionRight = false;

	//隠れるアニメーション再生フラグ
	bool hideAnimation = false;

	//隠れるボタンが押されたか
	bool hideButton = false;

	//現在は隠れているか
	bool nowHide = false;

	//隠れるボタンのクールタイム
	float hideWaitTime = 0f;

	//瞬歩で移動できる距離
	[SerializeField, Range( 0, 30f ) ] float momentaryRange = 0f;

	//現在の階層
	[SerializeField, Space( 10 )] int stair = 0;

	//小走りが始まるスティック傾斜範囲
	[SerializeField, Range( 0, 1 )] float stickRunRange = 0.5f;

	// Start is called before the first frame update
	void Start()
    {
		myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		//myRenderer.sprite = nowSprite;

		//隠れるボタンの押離判定
		HideStateVerify();

		//アニメーションの再生
		HideAnimator();

		//キャラ画像の方向
		SpriteDirect();

		//瞬歩
		MomentaryMove();

		//スプライトを管理
		myRenderer.sprite = nowSprite;
	}

	private void FixedUpdate()
	{
		//プレイヤーの移動
		if( nowHide == false )
		{
			MovePlayer();
		}
	}


	/// <summary>
	/// プレイヤー移動
	/// </summary>
	void MovePlayer()
	{
		float horizontal = Input.GetAxis( "Horizontal" );

		if( ( horizontal >= 0.1f && horizontal <= stickRunRange ) || ( horizontal <= -( 0.1f ) && horizontal >= -( stickRunRange ) ) )
		{
			GameManager.Instance.AnimationController.GetComponent<AnimationController>().Walk( 1, out nowSprite );
			transform.Translate( horizontal * playerSpeed_Normal, 0f, 0f );
		}
		else if( horizontal > stickRunRange || horizontal < -( stickRunRange ) )
		{
			GameManager.Instance.AnimationController.GetComponent<AnimationController>().Walk( 1, out nowSprite );
			transform.Translate( Math.Sign( horizontal ) * playerSpeed_Normal, 0f, 0f );
		}
		else
		{
			GameManager.Instance.AnimationController.GetComponent<AnimationController>().Walk( 0, out nowSprite );
		}
	}

	/// <summary>
	/// キャラクター画像を向いている方向に合わせる
	/// </summary>
	void SpriteDirect()
	{
		float horizonRaw = Input.GetAxisRaw( "Horizontal" );
		if( horizonRaw < 0 )
		{
			myRenderer.flipX = false;
			directionRight = false;
		}
		else if( horizonRaw > 0 )
		{
			myRenderer.flipX = true;
			directionRight = true;
		}
	}

	/// <summary>
	/// hideWaitTimeの更新とHideボタンを押したかの確認
	/// </summary>
	void HideStateVerify()
	{
		if( hideWaitTime < 1.1f )
		{
			hideWaitTime += Time.deltaTime;
		}

		if( Input.GetButtonDown( "Hide" ) )
		{
			hideButton = true;
		}
		else if( Input.GetButtonUp( "Hide" ) )
		{
			hideButton = false;
		}
	}

	/// <summary>
	/// 隠れる・隠れ解除の当たり判定
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerStay2D( Collider2D collision )
	{

		if( hideWaitTime > 1f )
		{
			if( collision.tag == "BackHideTrigger" && hideButton == true && nowHide == false )
			{
				SetHideConfig( collision.transform.position.x, collision.transform.position.y, 3 );
			}
			else if( collision.tag == "BackHideTrigger" && hideButton == true && nowHide == true )
			{
				SetHideConfig( collision.transform.position.x, collision.transform.position.y, 0 );
			}
		}

	}

	/// <summary>
	/// 隠れるor隠れ解除時のPositionとAnimationの設定
	/// </summary>
	/// <param name="nextPosX">隠れ先X</param>
	/// <param name="nextPosY">隠れ先Y</param>
	/// <param name="nextPosZ">隠れ先Z</param>
	void SetHideConfig( float nextPosX, float nextPosY, float nextPosZ )
	{
		_nextPosition = new Vector3( nextPosX, nextPosY, nextPosZ );
		hideWaitTime = 0f;
		hideAnimation = true;
		nowHide = !nowHide;
	}

	/// <summary>
	/// アニメーションの再生
	/// </summary>
	void HideAnimator()
	{
		if( hideAnimation == true )
		{
			MoveToNextPosition();
			hideAnimation = false;
		}
	}

	/// <summary>
	/// 次の隠・現座標に移動
	/// </summary>
	void MoveToNextPosition()
	{
		transform.position = _nextPosition;
	}

	/// <summary>
	/// 瞬歩(xUnit移動し、10f後に出現する)の処理
	/// </summary>
	void MomentaryMove()
	{
		float now = transform.position.x;
		float x = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
		if( Input.GetButtonDown( "Moove" ) )
		{
			StartCoroutine( "MomentaryMoveProcess" );
		}
	}

	IEnumerator MomentaryMoveProcess()
	{
		float now = transform.position.x;
		float x = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
		if( directionRight == true )
		{
			Vector3 nextMovePosition = new Vector3( now + ( x * 4 ), transform.position.y, transform.position.z );

			while( transform.position.x != nextMovePosition.x )
			{
				transform.position = Vector2.MoveTowards( transform.position, nextMovePosition, 2 );
				yield return new WaitForSeconds( Time.deltaTime );
			}
		}
		else if( directionRight == false )
		{
			Vector3 nextMovePosition = new Vector3( now - ( x * 4 ), transform.position.y, transform.position.z );

			while( transform.position.x != nextMovePosition.x )
			{
				transform.position = Vector2.MoveTowards( transform.position, nextMovePosition, 2 );
				yield return new WaitForSeconds( Time.deltaTime );
			}
		}
		yield return null;
	}

}
