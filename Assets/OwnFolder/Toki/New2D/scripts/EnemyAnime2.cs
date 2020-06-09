using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnime2 : MonoBehaviour
{

    private New2DEnemy enemyscript;

    int atk_flm = 0;
    int wlk_flm = 0;
    int run_flm = 0;
    int ang_flm = 0;


    [SerializeField] Sprite nowSprite = null;

    [SerializeField] Sprite normalSprite = null;

    [SerializeField] Sprite[] attacked = new Sprite[19];
    [SerializeField] Sprite[] walked = new Sprite[7];
    [SerializeField] Sprite[] angry = new Sprite[7];
    [SerializeField] Sprite[] death = new Sprite[18];
    [Space][SerializeField] Sprite suspiciousSprite;


    // Start is called before the first frame update
    void Start()
    {
        enemyscript = GetComponent<New2DEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        //////////敵の攻撃のアニメーション起動//////////////
        if (enemyscript.attackflg)
        {
            AttackOnEnemy(1);
        }
       

        ///////////敵の歩くアニメーション起動///////////////
        else if (enemyscript.walkflg)
        {
            WalkOnEnemy(1);
        }
        

        ////////////敵の走るアニメーション起動///////////////
        else if (enemyscript.runflg)
        {
            RunOnEnemy(1);
        }
        

        ////////////敵の威嚇アニメーション起動///////////////
        else if(enemyscript.angryflg)
        {
            atk_flm = 0;
            AngryOnEnemy(1);
        }
        

        /////////////////敵の注視の一枚絵/////////////////////
        else if(enemyscript.suspicious)
        {
            SuspiciousOnEnemy();
        }

        ///なんもなければとりあえずデフォ画像へ
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
            atk_flm = 0;
            wlk_flm = 0;
            run_flm = 0;
            ang_flm = 0;
        }

    }
    void AttackOnEnemy(int atk_flg)
    {
        if (atk_flm == 0 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[0];
        }
        else if (atk_flm == 3 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[1];
        }
        else if (atk_flm == 6 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[2];
        }
        else if (atk_flm == 9 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[3];
        }
        else if (atk_flm == 13 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[4];
        }
        else if (atk_flm == 18 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[5];
        }
        else if (atk_flm == 24 && atk_flg == 1)
        {
            enemyscript.attack_avoid = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[6];
        }
        else if (atk_flm == 29 && atk_flg == 1)
        { 
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[7];
        }
        else if (atk_flm == 34 && atk_flg == 1)
        {
            enemyscript.attack_avoid = false;
            enemyscript.lets_attack = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[8];
        }
        else if (atk_flm == 39 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[9];
        }
        else if (atk_flm == 44 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[10];
        }
        else if (atk_flm == 52 && atk_flg == 1)
        {
            enemyscript.lets_attack = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[11];
        }
        else if (atk_flm == 56 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[12];
        }
        else if (atk_flm == 60 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[13];
        }
        else if (atk_flm == 64 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[14];
        }
        else if (atk_flm == 68 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[15];
        }
        else if (atk_flm == 72 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[16];
        }
        else if (atk_flm == 76 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[17];
        }
        else if (atk_flm == 80 && atk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[18];
        }
        else if (atk_flm == 84 && atk_flg == 1)
        {
            atk_flg = 0;
            atk_flm = 0;
            enemyscript.attackflg = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
        }
        if (atk_flg == 1)
            atk_flm++;
    }


    /// <summary>
    /// 歩くアニメーション
    /// </summary>
    /// <param name="useComp">使用するときに1、使用し終わったら0を代入</param>
    void WalkOnEnemy(int wlk_flg)
    {
        if (wlk_flm == 0 && wlk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[0];
        }
        else if (wlk_flm == 6 && wlk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[1];
        }
        else if (wlk_flm == 12 && wlk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[2];
        }
        else if (wlk_flm == 18 && wlk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[3];
        }
        else if (wlk_flm == 24 && wlk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[4];
        }
        else if (wlk_flm == 30 && wlk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[5];
        }
        else if (wlk_flm == 36 && wlk_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[6];
        }
        else if (wlk_flm == 42 && wlk_flg == 1)
        {
            wlk_flm = 0;
        }
        else if (wlk_flg == 0)
        {
            wlk_flm = 0;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
        }

        if (wlk_flg == 1)
            wlk_flm++;
    }

    void RunOnEnemy(int run_flg)
    {
        if (run_flm == 0 && run_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[0];
        }
        else if (run_flm == 4 && run_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[1];
        }
        else if (run_flm == 8 && run_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[2];
        }
        else if (run_flm == 12 && run_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[3];
        }
        else if (run_flm == 16 && run_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[4];
        }
        else if (run_flm == 20 && run_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[5];
        }
        else if (run_flm == 24 && run_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[6];
        }
        else if (run_flm == 28 && run_flg == 1)
        {
            run_flm = 0;
        }
        else if (run_flg == 0)
        {
            run_flm = 0;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
        }

        if (run_flg == 1)
            run_flm++;
    }

    void AngryOnEnemy(int ang_flg)
    {
        if (ang_flm == 0 && ang_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = angry[0];
        }
        else if (ang_flm == 3 && ang_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = angry[1];
        }
        else if (ang_flm == 8 && ang_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = angry[2];
        }
        else if (ang_flm == 12 && ang_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = angry[3];
        }
        else if (ang_flm == 20 && ang_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = angry[4];
        }
        else if (ang_flm == 26 && ang_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = angry[5];
        }
        else if (ang_flm == 32 && ang_flg == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = angry[6];
        }
        else if (ang_flm == 38 && ang_flg == 1)
        {
            ang_flm = 0;
            enemyscript.angryflg = false;
        }
        else if (ang_flg == 0)
        {
            ang_flm = 0;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
        }
        if (ang_flg == 1)
            ang_flm++;
    }

    void SuspiciousOnEnemy()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = suspiciousSprite;
    }

}