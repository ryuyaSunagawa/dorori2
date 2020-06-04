using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FixPlayerScript : MonoBehaviour
{
	/*
	 * プレイヤーSprite・Speed・Direct系変数
	 */
	/// <summary>
	/// 自分のスプライト
	/// </summary>
	SpriteRenderer myRenderer = null;

	/// <summary>
	/// 現在使用するスプライト
	/// </summary>
	Sprite nowSprite = null;

	//ノーマルの画像
	[SerializeField] Sprite normalSprite = null;

	/// <summary>
	/// プレイヤーの移動スピード
	/// </summary>
	[SerializeField, Header ("プレイヤー移動"), Range( 0f, 0.5f )] float playerSpeed_Normal;

	/// <summary>
	/// プレイヤーの移動先ポジション
	/// </summary>
	Vector3 _nextPosition;

	/// <summary>
	/// 左右どちらを向いているか
	/// </summary>
	bool directionRight = true;

	//小走りが始まるスティック傾斜範囲
	[SerializeField, Range( 0, 1 )] float stickRunRange = 0.5f;

	/*
	 * Animation系変数
	 */

	//隠れるアニメーション再生フラグ
	bool hideAnimation = false;

	/*
	 * HideButton確認系フラグ・モード
	 */
	 /// <summary>
	 /// HideButtonが押されたか&箱隠れモード
	 /// </summary>
	bool hideButton = false;

	/// <summary>
	/// 箱隠れキャンセル
	/// </summary>
	bool hideBackCancelFlg = false;

	/// <summary>
	/// 変化モードが呼び出されたか
	/// </summary>
	bool disguiseFlg = false;

	/// <summary>
	/// 現在の隠れ状態
	/// </summary>
	bool nowHide = false;

	/// <summary>
	/// HideButtonのクールタイム
	/// </summary>
	float hideWaitTime = 0f;

	/// <summary>
	/// HideButtonの長押しフレーム数
	/// </summary>
	float hidePushFrame = 0;

	/*
	 * 瞬歩系変数
	 */
	/// <summary>
	/// 瞬歩のレンジ
	/// </summary>
	[SerializeField, Range( 0, 30f ) ] float moveDelta = 2f;

	/*
	 * 変化系変数
	 */
	/// <summary>
	/// 変化を使用しているときのモード( 0 = 使用していない, 1 = 初期化終了, 2 = 使用終了処理 )
	/// </summary>
	int disguiseMode = 0;

	/// <summary>
	/// 変化制限時間
	/// </summary>
	float disguiseTimeCount = 0f;

	/// <summary>
	/// 変化のタイムリミット
	/// </summary>
	[SerializeField, Range( 0f, 1.5f ), Header("変化制限時間")] float disguiceTimeLimit = 0f;

	/// <summary>
	/// 変化時画像
	/// </summary>
	[SerializeField] Sprite disguiseSprite = null;

	/*
	 * UnityEvent
	 */

	// Start is called before the first frame update
	void Start()
    {
		myRenderer = GetComponent<SpriteRenderer>();
		nowSprite = normalSprite;
    }

    // Update is called once per frame
    void Update()
    {
		//隠れるボタンの押離判定
		HideStateVerify();

		//アニメーションの再生
		HideAnimator();

		//瞬歩
		MomentaryMove();

		//変化
		DisguiseMode();

		//死んだとき
		if( GameManager.Instance.playerDeathFlg )
		{
			DeathProcess();
		}

		//スプライトを管理
		myRenderer.sprite = nowSprite;
	}

	private void FixedUpdate()
	{
		//プレイヤーの移動
		if( nowHide == false )
		{
			//プレイヤーの移動
			MovePlayer();

			//キャラ画像の方向
			SpriteDirect();
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
			//GameManager.Instance.AnimationController.GetComponent<AnimationController>().Walk( 1, out nowSprite );
			transform.Translate( horizontal * playerSpeed_Normal, 0f, 0f );
		}
		else if( horizontal > stickRunRange || horizontal < -( stickRunRange ) )
		{
			//GameManager.Instance.AnimationController.GetComponent<AnimationController>().Walk( 1, out nowSprite );
			transform.Translate( Math.Sign( horizontal ) * playerSpeed_Normal, 0f, 0f );
		}
		else
		{
			//GameManager.Instance.AnimationController.GetComponent<AnimationController>().Walk( 0, out nowSprite );
		}
	}

	/// <summary>
	/// キャラクター画像を向いている方向に合わせる
	/// </summary>
	void SpriteDirect()
	{
		//スティック
		float horizonRaw = Input.GetAxisRaw( "Horizontal" );

		if( horizonRaw < 0 )
		{
			directionRight = false;
		}
		else if( horizonRaw > 0 )
		{
			directionRight = true;
		}
		
		//Spriteのflipを調整
		if( directionRight == false )
		{
			myRenderer.flipX = false;
		}
		else if( directionRight == true )
		{
			myRenderer.flipX = true;
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

		//隠れていない状態でHideButtonフラグを立てる
		if( Input.GetButtonDown( "Hide" ) && nowHide == false )
		{
			hideButton = true;
		}
		//隠れている状態の解除
		else if( Input.GetButtonDown( "Hide" ) && nowHide == true )
		{
			hideButton = true;
			hideBackCancelFlg = true;
		}
		//UpしたときにhideButtonフラグを消す
		else if( Input.GetButtonUp( "Hide" ) )
		{
			hideButton = false;
			hideBackCancelFlg = false;
		}

		//一定フレーム押されていたら変化
		if( Input.GetButton( "Hide" ) && hideButton == true && ( hidePushFrame += Time.deltaTime ) >= 0.7f )
		{
			hideButton = false;
			disguiseFlg = true;
			hidePushFrame = 0f;
		}

		//変化の解除
		if( ( disguiseTimeCount >= 5.0f || disguiseMode == 1 ) && Input.GetButtonDown( "Hide" ) )
		{
			disguiseFlg = true;
			hideButton = false;
			disguiseMode = 2;
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
			//箱隠れ処理
			if( collision.tag == "BackHideTrigger" && hideButton == true && nowHide == false )
			{
				SetHideConfig( collision.transform.position.x, collision.transform.position.y, 3 );
			}
			//箱隠れ解除処理
			else if( collision.tag == "BackHideTrigger" && hideBackCancelFlg == true && nowHide == true )
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
		hideButton = false;
		hideBackCancelFlg = false;
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
		if( Input.GetButtonDown( "Moove" ) )
		{
			StartCoroutine( "MomentaryMoveProcess" );
		}
	}

	/// <summary>
	/// 瞬歩(移動処理)
	/// </summary>
	/// <returns></returns>
	IEnumerator MomentaryMoveProcess()
	{
		float now = transform.position.x;
		float x = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;

		double nowFloor = 1f;
		double nextFloor = 0f;

		if( directionRight == true )
		{
			Vector3 nextMovePosition = new Vector3( now + ( x * 4 ), transform.position.y, transform.position.z );

			while( nowFloor != nextFloor )
			{
				transform.position = Vector2.MoveTowards( transform.position, nextMovePosition, moveDelta );

				nowFloor = Math.Floor( transform.position.x * 1000 ) / 1000;
				nextFloor = Math.Floor( nextMovePosition.x * 1000 ) / 1000;

				yield return new WaitForSeconds( Time.deltaTime );
				Debug.Log( nowFloor + ", " + nextFloor );
			}
		}
		else if( directionRight == false )
		{
			Vector3 nextMovePosition = new Vector3( now - ( x * 4 ), transform.position.y, transform.position.z );

			while( nowFloor != nextFloor )
			{
				transform.position = Vector2.MoveTowards( transform.position, nextMovePosition, moveDelta );

				nowFloor = Math.Floor( transform.position.x * 1000 ) / 1000;
				nextFloor = Math.Floor( nextMovePosition.x * 1000 ) / 1000;

				yield return new WaitForSeconds( Time.deltaTime );
				Debug.Log( nowFloor + ", " + nextFloor );
			}
		}
		yield return null;
	}

	/// <summary>
	/// 変化の術
	/// </summary>
	void DisguiseMode()
	{
		//初期設定
		if( disguiseMode == 0 && disguiseFlg == true )
		{
			nowSprite = disguiseSprite;
			disguiseMode = 1;
		}

		//使用時処理
		if( disguiseMode == 1 )
		{
			disguiseTimeCount += Time.deltaTime;
		}

		//使用終了
		if( disguiseMode == 2 && disguiseFlg == true )
		{
			nowSprite = normalSprite;
			disguiseMode = 0;
			disguiseFlg = false;
			disguiseTimeCount = 0f;
		}
	}

	/// <summary>
	/// 死んだときの処理
	/// </summary>
	void DeathProcess()
	{
		if( GameManager.Instance.playerDeathNum++ >= 3 )
		{
			Destroy( this.gameObject );
			GameManager.Instance.playerDeathFlg = false;
		}
	}
}
