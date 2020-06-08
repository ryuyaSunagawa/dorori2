using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	int atk_flm = 0;
	int atk_flg = 0;
	int wlk_flm = 0;
	int wlk_flg = 0;
    int runSprite_flm = 0;
    int runSprite_flg = 0;

    [SerializeField] Sprite nowSprite = null;

	[SerializeField] Sprite normalSprite = null;

    [SerializeField] Sprite[] attackSprite = new Sprite[6];
	[SerializeField] Sprite[] walkSprite = new Sprite[8];
    [SerializeField] Sprite[] runSprite = new Sprite[8];

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
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite[ 0 ];
		}
		else if( atk_flm == 10 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite[ 1 ];
		}
		else if( atk_flm == 25 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite[ 2 ];
		}
		else if( atk_flm == 35 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite[ 3 ];
		}
		else if( atk_flm == 40 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite[ 4 ];
		}
		else if( atk_flm == 45 && atk_flg == 1 )
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite[ 5 ];
		}
		else if( atk_flm == 55 && atk_flg == 1 )
		{
			atk_flg = 0;
			atk_flm = 0;
		}
		if( atk_flg == 1 )
			atk_flm++;
	}


	/// <summary>
	/// 歩くアニメーション
	/// </summary>
	/// <param name="useComp">使用するときに1、使用し終わったら0を代入</param>
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


<<<<<<< HEAD
		walkSpriteSprite = nowSprite;
=======
		walkSprite = nowSprite;
>>>>>>> kazuna
	}

    public void Run(int useComp, out Sprite runSpriteSprite)
    {
        runSprite_flg = useComp;

        if (runSprite_flm == 0 && runSprite_flg == 1)
        {
            nowSprite = runSprite[0];
        }
        else if (runSprite_flm == 6 && runSprite_flg == 1)
        {
            nowSprite = runSprite[1];
        }
        else if (runSprite_flm == 12 && runSprite_flg == 1)
        {
            nowSprite = runSprite[2];
        }
        else if (runSprite_flm == 18 && runSprite_flg == 1)
        {
            nowSprite = runSprite[3];
        }
        else if (runSprite_flm == 24 && runSprite_flg == 1)
        {
            nowSprite = runSprite[4];
        }
        else if (runSprite_flm == 30 && runSprite_flg == 1)
        {
            nowSprite = runSprite[5];
        }
        else if (runSprite_flm == 36 && runSprite_flg == 1)
        {
            nowSprite = runSprite[6];
        }
        else if (runSprite_flm == 42 && runSprite_flg == 1)
        {
            nowSprite = runSprite[7];
        }
        else if (runSprite_flm == 60 && runSprite_flg == 1)
        {

            runSprite_flm = -1;
        }
        else if (runSprite_flg == 0)
        {

            runSprite_flm = -1;
            nowSprite = normalSprite;
        }

        if (runSprite_flg == 1)
            runSprite_flm++;


<<<<<<< HEAD
        runSpriteSprite = nowSprite;

        Debug.Log(runSprite_flm);

=======
        runSprite = nowSprite;
>>>>>>> kazuna
    }
}
