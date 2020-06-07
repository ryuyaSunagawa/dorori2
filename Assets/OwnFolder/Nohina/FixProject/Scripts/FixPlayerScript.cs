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

	//身代わりオブジェクト
	[SerializeField, Header( "身代わり木" )] GameObject sacrificeObject = null;

	//リスポーン場所
	[SerializeField, Range( 0f, 15f ), Header( "リスポーン場所の距離" )] float respawnDistance = 5f;

	/*
	 * 攻撃系変数
	 */
	BoxCollider2D[] b2d = new BoxCollider2D[ 2 ];
		
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

	bool attackFlg = false;

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

		b2d = GetComponents<BoxCollider2D>();
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

		//攻撃
		AttackProcess();

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
			if( disguiseMode == 0 )
			{
				GetComponent<PlayerAnimationScript>().Walk( 1, out nowSprite );
			}
			else if( disguiseMode == 1 )
			{
				nowSprite = disguiseSprite;
			}

			transform.Translate( horizontal * playerSpeed_Normal, 0f, 0f );
		}
		else if( horizontal > stickRunRange || horizontal < -( stickRunRange ) )
		{
			if( disguiseMode == 0 )
			{
				GetComponent<PlayerAnimationScript>().Walk( 1, out nowSprite );
			}
			else if( disguiseMode == 1 )
			{
				nowSprite = disguiseSprite;
			}

			transform.Translate( Math.Sign( horizontal ) * playerSpeed_Normal, 0f, 0f );
		}
		else
		{
			if( disguiseMode == 0 )
			{
				GetComponent<PlayerAnimationScript>().Walk( 0, out nowSprite );
			}
			else if( disguiseMode == 1 )
			{
				nowSprite = disguiseSprite;
			}
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
			disguiseFlg = true;
			hideButton = false;
			hidePushFrame = 0f;
		}

		//変化の解除
		if( ( disguiseTimeCount >= 5.0f || disguiseMode == 1 ) && ( Input.GetButtonDown( "Hide" ) || Input.GetButtonDown( "Touch" ) ) )
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
				gameObject.layer = 15;
				GameManager.Instance.playerHideFlg = true;
			}
			//箱隠れ解除処理
			else if( collision.tag == "BackHideTrigger" && hideBackCancelFlg == true && nowHide == true )
			{
				SetHideConfig( collision.transform.position.x, collision.transform.position.y, 0 );
				GameManager.Instance.playerHideFlg = false;
				gameObject.layer = 13;
			}
		}

		if( collision.tag == "Enemy" )
		{
			attackFlg = true;
		}

	}

	private void OnTriggerExit2D( Collider2D collision )
	{
		if( collision.tag == "Enemy" )
		{
			attackFlg = false;
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
	/// 攻撃処理
	/// </summary>
	void AttackProcess()
	{
		if( attackFlg == true && Input.GetButtonDown( "Touch" ) )
		{
			Destroy( GameManager.Instance.getenemyObj.gameObject );
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
			GameManager.Instance.playerMooveFlg = true;
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
			}
		}

		yield return null;
		GameManager.Instance.playerMooveFlg = false;
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
	/// 攻撃されたときの処理
	/// </summary>
	void DeathProcess()
	{
		//DeathFlgがたった時のリスポーン処理
		if( GameManager.Instance.playerRespawnFlg == false && GameManager.Instance.playerDeathNum-- > 0 )
		{
			GameManager.Instance.playerDeathNum--;
			GameManager.Instance.playerDeathFlg = false;
			GameManager.Instance.playerRespawnFlg = true;
			gameObject.layer = 17;
			print( "ahan" );
		}
		//DeathFlgがたった時の死亡処理
		else if( GameManager.Instance.playerRespawnFlg == false && GameManager.Instance.playerDeathNum-- == 0 )
		{
			Destroy( this.gameObject );
			GameManager.Instance.playerDeathFlg = false;
			gameObject.layer = 13;
		}

		if( GameManager.Instance.playerRespawnFlg == true )
		{
			StartCoroutine( "RespawnProcess" );
		}
	}

	/// <summary>
	/// リスポーン処理
	/// </summary>
	IEnumerator RespawnProcess()
	{
		//身代わりオブジェクトを作る
		sacrificeObject.GetComponent<sacrifice>().enablePosition = transform.position;
		sacrificeObject.SetActive( true );

		//敵ポジションとの差分によりリスポーン場所を決定する
		float enemyPosition = GameManager.Instance.getenemyObj.position.x;
		float distance = transform.position.x - enemyPosition;

		bool respawnDirectRight = false;

		if( distance >= 0 )
		{
			respawnDirectRight = true;
		}

		float now = transform.position.x;
		float x = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;

		//右側リスポーン
		if( respawnDirectRight == true )
		{
			transform.position = new Vector3( now + respawnDistance, transform.position.y, transform.position.z );
		}
		//左側リスポーン
		else if( respawnDirectRight == false )
		{
			transform.position = new Vector3( now - respawnDistance, transform.position.y, transform.position.z );
		}
		
		gameObject.layer = 13;
		GameManager.Instance.playerRespawnFlg = false;

		Debug.Log( "null" );

		yield return null;
	}
}
