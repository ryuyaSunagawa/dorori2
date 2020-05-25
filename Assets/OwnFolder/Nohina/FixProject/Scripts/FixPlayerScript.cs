using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPlayerScript : MonoBehaviour
{
	//自分のスプライトを保持する
	SpriteRenderer myRenderer = null;

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

	//現在の階層
	[SerializeField, Space( 10 )] int stair = 0;

	/// <summary>
	/// イベント処理
	/// </summary>

    // Start is called before the first frame update
    void Start()
    {
		myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		//隠れるボタンの押離判定
		HideStateVerify();

		//アニメーションの再生
		HideAnimator();

		//キャラ画像の方向
		SpriteDirect();
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

		transform.Translate( horizontal * playerSpeed_Normal, 0f, 0f );
	}

	/// <summary>
	/// キャラクター画像を向いている方向に合わせる
	/// </summary>
	void SpriteDirect()
	{
		float horizonRaw = Input.GetAxisRaw( "Horizontal" );
		if( horizonRaw < 0 )
		{
			myRenderer.flipX = true;
			directionRight = false;
		}
		else if( horizonRaw > 0 )
		{
			myRenderer.flipX = false;
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

}
