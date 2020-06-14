using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
	int atk_flm = 0;
	int atk_flg = 0;
	int wlk_flm = 0;
	int wlk_flg = 0;
	int runSprite_flm = 0;
	int runSprite_flg = 0;

	int disgue_flm = 0;
	int disSprite_flg = 0;

	int deathAnimationFrame = 0;

	//アタックの時に溜めるフレーム
	public int attackWaitFrame = 60;


	/// <summary>
	/// 現在のスプライト
	/// </summary>
	[SerializeField] Sprite nowSprite = null;

	/// <summary>
	/// ノーマル(立ち絵)のスプライト
	/// </summary>
	[SerializeField] Sprite normalSprite = null;
	[SerializeField] Sprite disguiceNormalSprite = null;

	[SerializeField, Header( "攻撃アニメーション" )] Sprite[] attackSprite = new Sprite[6];
	[SerializeField, Header( "歩行アニメーション" )] Sprite[] walkSprite = new Sprite[8];
	[SerializeField, Header( "走行アニメーション" )] Sprite[] runSprite = new Sprite[8];
	[SerializeField, Header( "死亡アニメーション" )] Sprite[] deathSprite = new Sprite[ 12 ];
	[SerializeField, Header( "変化歩行アニメーション" )] Sprite[] disguiceSprite = new Sprite[ 7 ];

	/// <summary>
	/// プレイヤーのアタックアニメーション
	/// </summary>
	public int Attack( int useComp, out Sprite outAttackprite )
	{

		atk_flg = useComp;

		if( atk_flm == 0 && atk_flg == 1 )
		{
			nowSprite = attackSprite[ 0 ];
		}
		else if( atk_flm == 10 && atk_flg == 1 )
		{
			nowSprite = attackSprite[ 1 ];
		}
		else if( atk_flm == 25 + attackWaitFrame && atk_flg == 1 )
		{
			nowSprite = attackSprite[ 2 ];
		}
		else if( atk_flm == 35 + attackWaitFrame && atk_flg == 1 )
		{
			nowSprite = attackSprite[ 3 ];
		}
		else if( atk_flm == 40 + attackWaitFrame && atk_flg == 1 )
		{
			nowSprite = attackSprite[ 4 ];
		}
		else if( atk_flm == 45 + attackWaitFrame && atk_flg == 1 )
		{
			nowSprite = attackSprite[ 5 ];
		}
		else if( atk_flm == 55 + attackWaitFrame && atk_flg == 1 )
		{
			nowSprite = normalSprite;
			atk_flg = 0;
			atk_flm = 0;
		}
		else if( atk_flg == 0 )
		{
			nowSprite = normalSprite;
		}

		if( atk_flg == 1 )
			atk_flm++;

		outAttackprite = nowSprite;

		return atk_flm;
	}

	/// <summary>
	/// プレイヤー歩行アニメーション
	/// </summary>
	/// <param name="useComp">使用中か</param>
	/// <param name="walkSpriteSprite">代入するSprite変数</param>
	public void Walk( int useComp, out Sprite walkSpriteSprite )
	{
		wlk_flg = useComp;

		if( wlk_flm == 0 && wlk_flg == 1 )
		{
			nowSprite = walkSprite[ 0 ];
		}
		else if( wlk_flm == 8 && wlk_flg == 1 )
		{
			nowSprite = walkSprite[ 1 ];
		}
		else if( wlk_flm == 16 && wlk_flg == 1 )
		{
			nowSprite = walkSprite[ 2 ];
		}
		else if( wlk_flm == 24 && wlk_flg == 1 )
		{
			nowSprite = walkSprite[ 3 ];
		}
		else if( wlk_flm == 32 && wlk_flg == 1 )
		{
			nowSprite = walkSprite[ 4 ];
		}
		else if( wlk_flm == 40 && wlk_flg == 1 )
		{
			nowSprite = walkSprite[ 5 ];
		}
		else if( wlk_flm == 48 && wlk_flg == 1 )
		{
			nowSprite = walkSprite[ 6 ];
		}
		else if( wlk_flm == 56 && wlk_flg == 1 )
		{
			nowSprite = walkSprite[ 7 ];
		}
		else if( wlk_flm == 60 && wlk_flg == 1 )
		{

			wlk_flm = -1;
		}
		else if( wlk_flg == 0 )
		{
			wlk_flm = -1;
			nowSprite = normalSprite;
		}

		if( wlk_flg == 1 )
			wlk_flm++;


		walkSpriteSprite = nowSprite;
	}

	/// <summary>
	/// プレイヤー走行アニメーション
	/// </summary>
	/// <param name="useComp">使用中であれば1</param>
	/// <param name="runSpriteSprite">代入するスプライト変数</param>
	public void Run( int useComp, out Sprite runSpriteSprite )
	{
		runSprite_flg = useComp;

		if( runSprite_flm == 0 && runSprite_flg == 1 )
		{
			nowSprite = runSprite[ 0 ];
		}
		else if( runSprite_flm == 6 && runSprite_flg == 1 )
		{
			nowSprite = runSprite[ 1 ];
		}
		else if( runSprite_flm == 12 && runSprite_flg == 1 )
		{
			nowSprite = runSprite[ 2 ];
		}
		else if( runSprite_flm == 18 && runSprite_flg == 1 )
		{
			nowSprite = runSprite[ 3 ];
		}
		else if( runSprite_flm == 24 && runSprite_flg == 1 )
		{
			nowSprite = runSprite[ 4 ];
		}
		else if( runSprite_flm == 30 && runSprite_flg == 1 )
		{
			nowSprite = runSprite[ 5 ];
		}
		else if( runSprite_flm == 36 && runSprite_flg == 1 )
		{
			nowSprite = runSprite[ 6 ];
		}
		else if( runSprite_flm == 42 && runSprite_flg == 1 )
		{
			nowSprite = runSprite[ 7 ];
		}
		else if( runSprite_flm == 60 && runSprite_flg == 1 )
		{

			runSprite_flm = -1;
		}
		else if( runSprite_flg == 0 )
		{

			runSprite_flm = -1;
			nowSprite = normalSprite;
		}

		if( runSprite_flg == 1 )
			runSprite_flm++;


		runSpriteSprite = nowSprite;

		Debug.Log( runSprite_flm );

	}

	public int PlayerDeathAnimation( int useComp, out Sprite outDeathSprite )
	{
		deathAnimationFrame += 2;
		if( ( deathAnimationFrame % 8 ) == 0 )
		{
			nowSprite = deathSprite[ ( deathAnimationFrame / 8 ) ];
		}

		if( deathAnimationFrame >= 96 || useComp == 0 )
		{
			outDeathSprite = normalSprite;

			deathAnimationFrame = 0;

			return 96;
		}

		outDeathSprite = nowSprite;

		return deathAnimationFrame;
	}

	public void disgueMode( int useComp, out Sprite runSpriteSprite )
	{

		disSprite_flg = useComp;

		if( disgue_flm == 0 && disSprite_flg == 1 )
		{
			nowSprite = disguiceSprite[ 0 ];
		}
		else if( disgue_flm == 6 && disSprite_flg == 1 )
		{
			nowSprite = disguiceSprite[ 1 ];
		}
		else if( disgue_flm == 12 && disSprite_flg == 1 )
		{
			nowSprite = disguiceSprite[ 2 ];
		}
		else if( disgue_flm == 18 && disSprite_flg == 1 )
		{
			nowSprite = disguiceSprite[ 3 ];
		}
		else if( disgue_flm == 24 && disSprite_flg == 1 )
		{
			nowSprite = disguiceSprite[ 4 ];
		}
		else if( disgue_flm == 30 && disSprite_flg == 1 )
		{
			nowSprite = disguiceSprite[ 5 ];
		}
		else if( disgue_flm == 36 && disSprite_flg == 1 )
		{
			nowSprite = disguiceSprite[ 6 ];
		}
		else if( disgue_flm == 42 && disSprite_flg == 1 )
		{
			disgue_flm = 0;
		}
		else if( disSprite_flg == 0 )
		{
			disgue_flm = 0;
			nowSprite = disguiceNormalSprite;
		}

		if( disSprite_flg == 1 )
			disgue_flm++;

		runSpriteSprite = nowSprite;
	}
}
