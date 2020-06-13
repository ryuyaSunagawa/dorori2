using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
	[SerializeField, Range( 0f, 25f ), Header( "リスポーン場所の距離" )] float respawnDistance = 10f;

	bool deathAnimationFlg = false;

	New2DEnemy enemyScript = null;
		
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
	[SerializeField] bool disguiseFlg = false;

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

	bool attackNowFlg = false;

	Sprite attackingSprite = null;

	/*
	 * 瞬歩系変数
	 */
	/// <summary>
	/// 瞬歩移動の速さ
	/// </summary>
	[SerializeField, Range( 0, 30f ) ] float moveDelta = 2f;

	[SerializeField, Range( 0, 25f )] float mooveRange = 10f;

	/// <summary>
	/// 瞬歩のタイマー
	/// </summary>
	float mooveTimer = 0f;

	/// <summary>
	/// 瞬歩クールタイム
	/// </summary>
	[SerializeField, Range( 0, 5f ), Header( "瞬歩クールタイム" )] float mooveCoolTime = 0f;

	[SerializeField] Scrollbar mooveBar = null;

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

	[SerializeField] Scrollbar disguiceBar = null;


	[SerializeField] Transform enemyTrigger = null;

	/// <summary>
	/// 変化可能回数
	/// </summary>
	[SerializeField] int disguiceNumber = 5;

	/// <summary>
	/// 変化待機時間
	/// </summary>
	[SerializeField, Range( 0, 10f ), Header( "変化クールタイム" ) ] float disguiceCoolTime = 5f;

	/// <summary>
	/// 変化待機時間カウンター
	/// </summary>
	float disguiceWaitTime = 5f;
	
	//ディスタンス比較用
	Vector3 distanceComp = new Vector3( 100, 100, 100 );

	//変化のパーティクルプレハブ
	[SerializeField] GameObject disguiceParticle = null;

	float disguiceChangeFrame = 0f;

	//パーティクルの比較
	bool pushFrameAt2 = false;

	//攻撃パーティクル生成場所(プレイヤーDirectionRightと同じ)
	//左
	[SerializeField] Transform attackLeft = null;
	//右
	[SerializeField] Transform attackRight = null;

	[SerializeField] GameObject attackParticle = null;

	//攻撃のパーティクル
	GameObject useAttackParticle = null;
	Transform useAttackParticlePos = null;

	//ローカル内の攻撃パーティクル最終到達場所
	Vector2 localAttackTrail = new Vector2( 1.018f, 0.219f );

	float attackParticleDistance = 0f;
	float attackParticleSpeed = 0f;

	/*
	 * UnityEvent
	 */

	private void Awake()
	{
		gameObject.layer = 13;
		mooveTimer = mooveCoolTime;
	}

	// Start is called before the first frame update
	void Start()
    {
		myRenderer = GetComponent<SpriteRenderer>();
		nowSprite = normalSprite;
		enemyScript = GameManager.Instance.getenemyObj.GetComponent<New2DEnemy>();

		mooveRange = transform.position.x + ( myRenderer.bounds.size.x * 4 );

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
		if( nowHide == false && !GameManager.Instance.playerRespawnFlg && !GameManager.Instance.playerAttackNowFlg && !GameManager.Instance.playerDeathFlg 
			&& !hideButton && disguiseMode != 1 )
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
			if( disguiseMode == 0 && GameManager.Instance.playerAttackNowFlg == false && GameManager.Instance.playerRespawnFlg == false && deathAnimationFlg == false )
			{
				GetComponent<PlayerAnimationScript>().Walk( 1, out nowSprite );
			}
			else if( disguiseMode == 2 )
			{
				nowSprite = disguiseSprite;
			}

			transform.Translate( horizontal * playerSpeed_Normal, 0f, 0f );
		}
		else if( horizontal > stickRunRange || horizontal < -( stickRunRange ) )
		{
			if( disguiseMode == 0 && GameManager.Instance.playerAttackNowFlg == false && GameManager.Instance.playerRespawnFlg == false && deathAnimationFlg == false )
			{
				GetComponent<PlayerAnimationScript>().Walk( 1, out nowSprite );
			}
			else if( disguiseMode == 2 )
			{
				nowSprite = disguiseSprite;
			}

			transform.Translate( Math.Sign( horizontal ) * playerSpeed_Normal, 0f, 0f );
		}
		else
		{
			if( disguiseMode == 0 && GameManager.Instance.playerAttackNowFlg == false && GameManager.Instance.playerRespawnFlg == false && deathAnimationFlg == false )
			{
				GetComponent<PlayerAnimationScript>().Walk( 0, out nowSprite );
			}
			else if( disguiseMode == 2 )
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

	int buttondownnum = 0;

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
		if( Input.GetButtonDown( "Hide" ) && nowHide == false && !GameManager.Instance.playerMooveFlg )
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
			hidePushFrame = 0f;
			pushFrameAt2 = false;
		}

		//一定フレーム押されていたら変化
		if( Input.GetButton( "Hide" ) && hideButton == true && ( hidePushFrame += Time.deltaTime ) >= 0.7f && ( disguiceWaitTime >= disguiceCoolTime ) )
		{
			disguiseFlg = true;
			hideButton = false;
			hidePushFrame = 0f;
			disguiceChangeFrame += 0.001f;
		}
		else if( disguiseFlg == false && pushFrameAt2 && !nowHide && Input.GetButton( "Hide" ) )
		{
			print( ++buttondownnum );
			StartCoroutine( "DisguiceParticle" );
			pushFrameAt2 = false;
		}

		//変化の解除
		if( ( disguiseTimeCount >= 5.0f || disguiseMode == 2 ) && ( Input.GetButtonDown( "Hide" ) || Input.GetButtonDown( "Touch" ) ) )
		{
			disguiseFlg = true;
			hideButton = false;
			disguiseMode = 3;
		}

		if( Input.GetButtonDown( "Hide" ) )
		{
			pushFrameAt2 = true;
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

		//接敵且つ瞬歩していなければ
		if( collision.tag == "Enemy" )
		{
			//敵が右向いてる時にプレイヤーが左にいるか、もしくは敵が左向いてる時にプレイヤーが右にいる状態だとattackFlgをtrueにする
			if( ( enemyScript.direction == false && ( GameManager.Instance.getenemyObj.position.x > transform.position.x ) )
			|| ( enemyScript.direction == true && ( GameManager.Instance.getenemyObj.position.x < transform.position.x ) ) )
			{
				attackFlg = true;
			}
			
		}

	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( !( disguiseMode == 0 ) && collision.collider.tag == "Enemy" )
		{
			disguiseMode = 3;
		}
	}

	/// <summary>
	/// アタック範囲を抜けたら
	/// </summary>
	/// <param name="collision"></param>
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
		int attackFrame = 0;

		if( attackFlg == true && Input.GetButtonDown( "Touch" ) && deathAnimationFlg == false && !GameManager.Instance.playerMooveFlg )
		{
			GameManager.Instance.playerAttackNowFlg = true;
		}
		
		if( GameManager.Instance.playerAttackNowFlg == true )
		{
			attackFrame = GetComponent<PlayerAnimationScript>().Attack( 1, out nowSprite );

			//11フレーム目で炎的なエフェクトを出す
			if( attackFrame == 11 )
			{
				if( directionRight == true )
				{
					useAttackParticle = Instantiate( attackParticle, attackRight.position, Quaternion.identity ) as GameObject;
					print( useAttackParticle.gameObject.name );
				}
				else if( directionRight == false )
				{
					useAttackParticle = Instantiate( attackParticle, attackLeft.position, Quaternion.identity ) as GameObject;
					print( useAttackParticle.gameObject.name );
				}
			}

			//85f~の処理
			if( directionRight == true && attackFrame >= 85 && attackFrame < 95 )
			{
				if( attackFrame == 85 )
				{
					attackParticleDistance = Vector2.Distance( transform.position, transform.root.TransformPoint( new Vector3( 0.591f, 0.281f ) + transform.localPosition ) );
					attackParticleSpeed = attackParticleDistance / 10;
				}
				else if( attackFrame == 94 )
				{
					attackParticleDistance = 0f;
					attackParticleSpeed = 0f;
				}

				Vector3 nowParticlePos = useAttackParticle.GetComponent<Transform>().position;
				Vector3 worldPos = transform.root.TransformPoint( new Vector3( 0.591f, 0.281f ) + transform.localPosition );
				worldPos.z = attackRight.position.z;
				useAttackParticle.GetComponent<Transform>().position = Vector3.MoveTowards( nowParticlePos, worldPos, attackParticleSpeed );
			}
			else if( directionRight == false && attackFrame >= 85 && attackFrame < 95 )
			{
				if( attackFrame == 85 )
				{
					attackParticleDistance = Vector2.Distance( transform.position, transform.root.TransformPoint( new Vector3( -0.591f, 0.281f ) + transform.localPosition ) );
					attackParticleSpeed = attackParticleDistance / 10;
				}
				else if( attackFrame == 94 )
				{
					attackParticleDistance = 0f;
					attackParticleSpeed = 0f;
				}

				Vector3 nowParticlePos = useAttackParticle.GetComponent<Transform>().position;
				Vector3 worldPos = transform.root.TransformPoint( new Vector3( -0.591f, 0.281f ) + transform.localPosition );
				worldPos.z = attackLeft.position.z;
				useAttackParticle.GetComponent<Transform>().position = Vector3.MoveTowards( nowParticlePos, worldPos, attackParticleSpeed );
			}

			//95f~からの処理
			if( directionRight == true && attackFrame >= 95 )
			{
				print( "aaha" );
				if( attackFrame == 95 )
				{
					attackParticleDistance = Vector2.Distance( transform.position, transform.root.TransformPoint( new Vector3( 1.5f, 0.12f ) + transform.localPosition ) );
					attackParticleSpeed = attackParticleDistance / 0.08f;
				}

				Vector3 nowParticlePos = useAttackParticle.GetComponent<Transform>().position;
				Vector3 worldPos = transform.root.TransformPoint( new Vector3( 1.8f, 0.12f ) + transform.localPosition );
				worldPos.z = attackRight.position.z;
				useAttackParticle.GetComponent<Transform>().position = Vector3.MoveTowards( nowParticlePos, worldPos, attackParticleSpeed );
			}
			else if( directionRight == false && attackFrame >= 95 )
			{
				if( attackFrame == 95 )
				{
					attackParticleDistance = Vector2.Distance( transform.position, transform.root.TransformPoint( new Vector3( -1.5f, 0.12f ) + transform.localPosition ) );
					attackParticleSpeed = attackParticleDistance / 0.08f;
				}

				Vector3 nowParticlePos = useAttackParticle.GetComponent<Transform>().position;
				Vector3 worldPos = transform.root.TransformPoint( new Vector3( -1.8f, 0.12f ) + transform.localPosition );
				worldPos.z = attackLeft.position.z;
				useAttackParticle.GetComponent<Transform>().position = Vector3.MoveTowards( nowParticlePos, worldPos, attackParticleSpeed );
			}
		}

		if( attackFrame == ( 55 + GetComponent<PlayerAnimationScript>().attackWaitFrame ) )
		{
			print( attackFrame );
			enemyScript.deathflg = true;

			attackParticleDistance = 0f;
			attackParticleSpeed = 0f;

			attackFlg = false;
			GameManager.Instance.playerAttackNowFlg = false;
			attackFrame = 0;
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
		if( mooveTimer <= mooveCoolTime )
		{
			mooveTimer += Time.deltaTime;
			float mooveTimeSize = mooveTimer / mooveCoolTime;
			mooveBar.size = mooveTimer / mooveCoolTime;
		}

		if( !GameManager.Instance.playerMooveFlg && Input.GetButtonDown( "Moove" ) && mooveTimer >= mooveCoolTime && !( disguiseMode == 2 ) && !nowHide
			&& !GameManager.Instance.playerDeathFlg && !GameManager.Instance.playerRespawnFlg )
		{
			GameManager.Instance.playerMooveFlg = true;
			mooveTimer = 0;

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
			Vector3 nextMovePosition = Vector3.zero;

			if( !enemyScript.attack_avoid )
			{
				nextMovePosition = CollideJudge( mooveRange, directionRight );
			}

			if( enemyScript.attack_avoid || GameManager.Instance.enemyState == 1 || nextMovePosition == distanceComp )
			{
				nextMovePosition = new Vector3( now + ( x * 4 ), transform.position.y, transform.position.z );
			}

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
			Vector3 nextMovePosition = Vector3.zero;

			if( !enemyScript.attack_avoid )
			{
				nextMovePosition = CollideJudge( mooveRange, directionRight );
			}

			if( enemyScript.attack_avoid || nextMovePosition == distanceComp )
			{
				nextMovePosition = new Vector3( now - ( x * 4 ), transform.position.y, transform.position.z );
			}

			while( nowFloor != nextFloor )
			{
				transform.position = Vector2.MoveTowards( transform.position, nextMovePosition, moveDelta );

				nowFloor = Math.Floor( transform.position.x * 1000 ) / 1000;
				nextFloor = Math.Floor( nextMovePosition.x * 1000 ) / 1000;

				yield return new WaitForSeconds( Time.deltaTime );
			}
		}

		GameManager.Instance.playerMooveFlg = false;
		yield return null;
	}

	/// <summary>
	/// 変化の術
	/// </summary>
	void DisguiseMode()
	{
		//ウェイトタイムへ加算
		if( disguiceWaitTime < disguiceCoolTime )
		{
			disguiceWaitTime += Time.deltaTime;
			disguiceBar.size = disguiceWaitTime / disguiceCoolTime;
		}

		//初期設定
		if( disguiseMode == 0 && disguiseFlg == true )
		{
			disguiseMode = 1;
			gameObject.name = "PlayerDisguice";
			GameManager.Instance.playerDisguiceFlg = true;
		}

		if( disguiseMode == 1 )
		{
			disguiceChangeFrame += Time.deltaTime;

			if( disguiceChangeFrame >= 0.8f )
			{
				print( disguiceChangeFrame );
				disguiseMode = 2;
				nowSprite = disguiseSprite;
			}
		}

		//使用時処理
		if( disguiseMode == 2 )
		{
			disguiseTimeCount += Time.deltaTime;
			GameManager.Instance.playerDisguiceFlg = true;
		}

		//使用終了
		if( disguiseMode == 3 && disguiseFlg == true )
		{
			disguiceChangeFrame = 0f;
			nowSprite = normalSprite;
			disguiseMode = 0;
			disguiseFlg = false;
			disguiseTimeCount = 0f;
			gameObject.name = "Player";
			GameManager.Instance.playerDisguiceFlg = false;

			disguiceWaitTime = 0f;

		}
	}

	/// <summary>
	/// 攻撃されたときの処理
	/// </summary>
	void DeathProcess()
	{
		GameManager.Instance.playerDeathNum--;

		GameManager.Instance.DecreaseLife();

		//DeathFlgがたった時のリスポーン処理
		if( GameManager.Instance.playerRespawnFlg == false && GameManager.Instance.playerDeathNum > 0 )
		{
			GameManager.Instance.playerDeathFlg = false;
			GameManager.Instance.playerRespawnFlg = true;
			nowHide = false;
			gameObject.layer = 17;

			StartCoroutine( "RespawnProcess" );
		}
		//DeathFlgがたった時の死亡処理
		else if( GameManager.Instance.playerDeathNum <= 0 )
		{
			//Destroy( this.gameObject );
			GameManager.Instance.playerDeathFlg = false;
			deathAnimationFlg = true;
			nowHide = false;
			gameObject.layer = 17;

			StartCoroutine( "DeadProcess" );
		}
	}

	/// <summary>
	/// リスポーン処理
	/// </summary>
	IEnumerator RespawnProcess()
	{
		int respawnAnimationFrame = 0;

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

		Rigidbody2D rb = GetComponent<Rigidbody2D>();

		if( respawnDirectRight == true )
		{
			rb.AddForce( new Vector2( 7f, 6f ), ForceMode2D.Impulse );
		}
		else if( respawnDirectRight == false )
		{
			rb.AddForce( new Vector2( -7f, 6f ), ForceMode2D.Impulse );
		}

		do
		{
			respawnAnimationFrame = GetComponent<PlayerAnimationScript>().PlayerDeathAnimation( 1, out nowSprite );

			if( respawnAnimationFrame >= 70 )
			{

				//右側リスポーン
				if( respawnDirectRight == true )
				{
					transform.position = new Vector3( now + respawnDistance, transform.position.y, transform.position.z );
					respawnAnimationFrame = GetComponent<PlayerAnimationScript>().PlayerDeathAnimation( 0, out nowSprite );

					//身代わりオブジェクトを作る
					sacrificeObject.GetComponent<sacrifice>().enablePosition = transform.position;
					sacrificeObject.GetComponent<sacrifice>().directRight = true;
					sacrificeObject.SetActive( true );

					rb.velocity = Vector2.zero;
					rb.angularVelocity = 0f;

					gameObject.layer = 13;
					GameManager.Instance.playerRespawnFlg = false;

					yield return null;
				}
				//左側リスポーン
				else if( respawnDirectRight == false )
				{
					//
					transform.position = new Vector3( now - respawnDistance, transform.position.y, transform.position.z );
					respawnAnimationFrame = GetComponent<PlayerAnimationScript>().PlayerDeathAnimation( 0, out nowSprite );

					//身代わりオブジェクトを作る
					sacrificeObject.GetComponent<sacrifice>().enablePosition = transform.position;
					sacrificeObject.GetComponent<sacrifice>().directRight = false;
					sacrificeObject.SetActive( true );

					//速度変更
					rb.velocity = Vector2.zero;
					rb.angularVelocity = 0f;

					gameObject.layer = 13;
					GameManager.Instance.playerRespawnFlg = false;

					yield return null;
				}
			}
			
			yield return new WaitForSeconds( Time.deltaTime );
		} while( respawnAnimationFrame < 96 );
		
		yield return null;
	}

	IEnumerator DeadProcess()
	{
		//Destroy( this.gameObject );
		int deathAnimationFrame = 0;

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

		Rigidbody2D rb = GetComponent<Rigidbody2D>();

		if( respawnDirectRight == true )
		{
			rb.AddForce( new Vector2( 7f, 6f ), ForceMode2D.Impulse );
		}
		else if( respawnDirectRight == false )
		{
			rb.AddForce( new Vector2( -7f, 6f ), ForceMode2D.Impulse );
		}

		do
		{
			deathAnimationFrame = GetComponent<PlayerAnimationScript>().PlayerDeathAnimation( 1, out nowSprite );

			if( deathAnimationFrame >= 96 )
			{
				deathAnimationFrame = GetComponent<PlayerAnimationScript>().PlayerDeathAnimation( 0, out nowSprite );
				GameManager.Instance.playerFinishDeathAnimationFlg = true;
				//Destroy( this.gameObject );
			}

			yield return new WaitForSeconds( Time.deltaTime );
		} while( deathAnimationFrame < 96 );

		yield return null;
	}

	/// <summary>
	/// 瞬歩前の当たり判定
	/// </summary>
	/// <param name="rayDistance">レイキャストの長さ</param>
	Vector3 CollideJudge( float rayDistance, bool direct )
	{
		//レイヤーマスク
		int layerMask = ( 1 << 9 | 1 << 14 );

		RaycastHit2D collideObject;
		float collideObjectDistance = 0f;

		//レイの起点
		Vector2 rayOrigin = new Vector2( transform.position.x, -3f );

		//右の場合と左の場合
		if( direct == true )
		{
			collideObject = Physics2D.Raycast( rayOrigin, Vector2.right, rayDistance, layerMask );

			if( !collideObject )
			{
				return new Vector3( 100, 100, 100 );
			}

			//衝突判定のあるオブジェクトとテレポート先のディスタンスを設ける
			collideObjectDistance = collideObject.transform.position.x - ( collideObject.collider.bounds.size.x / 2 ) - ( myRenderer.bounds.size.x / 2 ) - ( myRenderer.bounds.size.x / 8 );
			Debug.Log( collideObject.transform.name );
		}
		else if( direct == false )
		{
			collideObject = Physics2D.Raycast( transform.position, Vector2.left, rayDistance, layerMask );

			if( !collideObject )
			{
				return new Vector3( 100, 100, 100 );
			}

			collideObjectDistance = collideObject.transform.position.x + ( collideObject.collider.bounds.size.x / 2 ) + ( myRenderer.bounds.size.x / 2 ) + ( myRenderer.bounds.size.x / 8 );
			Debug.Log( collideObject.transform.name );

		}

		return new Vector3( collideObjectDistance, transform.position.y, transform.position.z );
	}

	int coroutine = 0;

	IEnumerator DisguiceParticle()
	{
		Debug.Log( ++coroutine );
		//変化パーティクル再生フレーム
		float disguiceFrame = 0f;

		//ボタンが押されたフレーム
		float iButtonPushFrame = 0f;

		//ボタンが離された場合のフラグ
		bool buttonUp = false;

		//パーティクルDestroy用フラグ
		bool particleDeathFlg = false;

		Quaternion particleRotation = Quaternion.Euler( 90f, 0f, 0f );
		GameObject _disguiceParticleInCoroutine = Instantiate( disguiceParticle, new Vector3( transform.position.x, transform.position.y, transform.position.z - 3f ), particleRotation ) as GameObject;
		DisguiceParticleScript particleScript = _disguiceParticleInCoroutine.GetComponent<DisguiceParticleScript>();

		do
		{
			disguiceFrame += Time.deltaTime;
			iButtonPushFrame += Time.deltaTime;

			if( ( iButtonPushFrame < 0.7f ) && !Input.GetButton( "Hide" ) )
			{
				buttonUp = true;
			}

			yield return null;
		} while( buttonUp == false && disguiceFrame <= 4f );

		do
		{
			particleDeathFlg = particleScript.deathParticle( true );

			yield return null;
		} while( buttonUp == true && !particleDeathFlg );

		Destroy( _disguiceParticleInCoroutine );

		yield return null;
	}
}
