using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	int atk_flm = 0;
	int atk_flg = 0;
	int wlk_flm = 0;
	int wlk_flg = 0;

	[SerializeField] Sprite nowSprite = null;

	[SerializeField] Sprite normalSprite = null;

    [SerializeField] Sprite[] attacked = new Sprite[6];
	[SerializeField] Sprite[] walked = new Sprite[8];

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Attack()
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			atk_flg = 1;
		}
		if( atk_flm == 0 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[ 0 ];
		}
		else if( atk_flm == 10 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[ 1 ];
		}
		else if( atk_flm == 25 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[ 2 ];
		}
		else if( atk_flm == 35 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[ 3 ];
		}
		else if( atk_flm == 40 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[ 4 ];
		}
		else if( atk_flm == 45 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[ 5 ];
		}
		else if( atk_flm == 55 && atk_flg == 1 )
		{
			atk_flg = 0;
			atk_flm = 0;
		}
		if( atk_flg == 1 )
			atk_flm++;
		Debug.Log( atk_flm );
	}


	/// <summary>
	/// 歩くアニメーション
	/// </summary>
	/// <param name="useComp">使用するときに1、使用し終わったら0を代入</param>
	public void Walk( int useComp, out Sprite walkSprite )
	{
		wlk_flg = useComp;

		if( wlk_flm == 0 && wlk_flg == 1 )
		{
			nowSprite = walked[ 0 ];
		}
		else if( wlk_flm == 8 && wlk_flg == 1 )
		{
			nowSprite = walked[ 1 ];
		}
		else if( wlk_flm == 16 && wlk_flg == 1 )
		{
			nowSprite = walked[ 2 ];
		}
		else if( wlk_flm == 24 && wlk_flg == 1 )
		{
			nowSprite = walked[ 3 ];
		}
		else if( wlk_flm == 32 && wlk_flg == 1 )
		{
			nowSprite = walked[ 4 ];
		}
		else if( wlk_flm == 40 && wlk_flg == 1 )
		{
			nowSprite = walked[ 5 ];
		}
		else if( wlk_flm == 48 && wlk_flg == 1 )
		{
			nowSprite = walked[ 6 ];
		}
		else if( wlk_flm == 56 && wlk_flg == 1 )
		{
			nowSprite = walked[ 7 ];
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


		walkSprite = nowSprite;

		//Debug.Log( wlk_flm );
	}
}
