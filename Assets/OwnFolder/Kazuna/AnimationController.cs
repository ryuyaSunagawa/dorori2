using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	int atk_flm = 0;
	int atk_flg = 0;
	int wlk_flm = 0;
	int wlk_flg = 0;
    int run_flm = 0;
    int run_flg = 0;
    int wlk_speed_flg=0;
    float StickTilt;

    [SerializeField] Sprite nowSprite = null;

	[SerializeField] Sprite normalSprite = null;

    [SerializeField] Sprite[] attacked = new Sprite[6];
	[SerializeField] Sprite[] walked = new Sprite[8];
    [SerializeField] Sprite[] run = new Sprite[8];

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
	}


	/// <summary>
	/// 歩くアニメーション
	/// </summary>
	/// <param name="useComp">使用するときに1、使用し終わったら0を代入</param>
	public void Walk( int useComp, out Sprite walkSprite )
	{
		wlk_flg = useComp;
        StickTilt=Input.GetAxis("Horizontal");
        if(StickTilt >= 0.3f && StickTilt <= 0.4f)
        {
            wlk_speed_flg = 1;
        }
        else
        {
            wlk_speed_flg = 0;
        }
        switch (wlk_speed_flg)
        {
            case 0:
                if (wlk_flm == 0 && wlk_flg == 1)
                {
                    nowSprite = walked[0];
                }
                else if (wlk_flm == 8 && wlk_flg == 1)
                {
                    nowSprite = walked[1];
                }
                else if (wlk_flm == 16 && wlk_flg == 1)
                {
                    nowSprite = walked[2];
                }
                else if (wlk_flm == 24 && wlk_flg == 1)
                {
                    nowSprite = walked[3];
                }
                else if (wlk_flm == 32 && wlk_flg == 1)
                {
                    nowSprite = walked[4];
                }
                else if (wlk_flm == 40 && wlk_flg == 1)
                {
                    nowSprite = walked[5];
                }
                else if (wlk_flm == 48 && wlk_flg == 1)
                {
                    nowSprite = walked[6];
                }
                else if (wlk_flm == 56 && wlk_flg == 1)
                {
                    nowSprite = walked[7];
                }
                else if (wlk_flm == 60 && wlk_flg == 1)
                {

                    wlk_flm = -1;
                }
                else if (wlk_flg == 0)
                {
                    wlk_flm = -1;
                    nowSprite = normalSprite;
                }
                break;

            case 1:
                if (wlk_flm == 0 && wlk_flg == 1)
                {
                    nowSprite = walked[0];
                }
                else if (wlk_flm == 6 && wlk_flg == 1)
                {
                    nowSprite = walked[1];
                }
                else if (wlk_flm == 12 && wlk_flg == 1)
                {
                    nowSprite = walked[2];
                }
                else if (wlk_flm == 18 && wlk_flg == 1)
                {
                    nowSprite = walked[3];
                }
                else if (wlk_flm == 24 && wlk_flg == 1)
                {
                    nowSprite = walked[4];
                }
                else if (wlk_flm == 30 && wlk_flg == 1)
                {
                    nowSprite = walked[5];
                }
                else if (wlk_flm == 36 && wlk_flg == 1)
                {
                    nowSprite = walked[6];
                }
                else if (wlk_flm == 42 && wlk_flg == 1)
                {
                    nowSprite = walked[7];
                }
                else if (wlk_flm == 48 && wlk_flg == 1)
                {

                    wlk_flm = -1;
                }
                else if (wlk_flg == 0)
                {
                    wlk_flm = -1;
                    nowSprite = normalSprite;
                }
                break;
        }
        
        if (wlk_flg == 1)
            wlk_flm++;
        walkSprite = nowSprite;
    }

    public void Run(int useComp, out Sprite runSprite)
    {
        run_flg = useComp;

        if (run_flm == 0 && run_flg == 1)
        {
            nowSprite = run[0];
        }
        else if (run_flm == 6 && run_flg == 1)
        {
            nowSprite = run[1];
        }
        else if (run_flm == 12 && run_flg == 1)
        {
            nowSprite = run[2];
        }
        else if (run_flm == 18 && run_flg == 1)
        {
            nowSprite = run[3];
        }
        else if (run_flm == 24 && run_flg == 1)
        {
            nowSprite = run[4];
        }
        else if (run_flm == 30 && run_flg == 1)
        {
            nowSprite = run[5];
        }
        else if (run_flm == 36 && run_flg == 1)
        {
            nowSprite = run[6];
        }
        else if (run_flm == 42 && run_flg == 1)
        {
            nowSprite = run[7];
        }
        else if (run_flm == 60 && run_flg == 1)
        {

            run_flm = -1;
        }
        else if (run_flg == 0)
        {

            run_flm = -1;
            nowSprite = normalSprite;
        }

        if (run_flg == 1)
            run_flm++;


        runSprite = nowSprite;
    }
}
