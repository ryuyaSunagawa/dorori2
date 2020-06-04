using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnime : MonoBehaviour
{
    //int atk_flm = 0;
    //int wlk_flm = 0;
    //int run_flm = 0;

    //private New2DEnemy enemyscript;

    //[SerializeField] Sprite nowSprite = null;

    //[SerializeField] Sprite normalSprite = null;

    //[SerializeField] Sprite[] attacked = new Sprite[6];
    //[SerializeField] Sprite[] walked = new Sprite[8];
    //[SerializeField] Sprite[] run = new Sprite[8];


    //// Start is called before the first frame update
    //void Start()
    //{
    //    enemyscript = GetComponent<New2DEnemy>(); 
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (enemyscript.attackflg)
    //    {
    //        AttackOnEnemy(1);
    //    }
    //    else
    //    {
    //        atk_flm = 0;
    //    }

    //    if (enemyscript.walkflg)
    //    {
    //        WalkOnEnemy(1);
    //    }
    //    else
    //    {
    //        wlk_flm = 0;
    //    }

    //    if(enemyscript.runflg)
    //    {
    //        RunOnEnemy(1);
    //    }
    //    else
    //    {
    //        run_flm = 0;
    //    }
    //}
    //void AttackOnEnemy(int atk_flg)
    //{
    //    if (atk_flm == 0 && atk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[0];
    //    }
    //    else if (atk_flm == 10 && atk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[1];
    //    }
    //    else if (atk_flm == 25 && atk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[2];
    //    }
    //    else if (atk_flm == 35 && atk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[3];
    //    }
    //    else if (atk_flm == 40 && atk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[4];
    //    }
    //    else if (atk_flm == 45 && atk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = attacked[5];
    //    }
    //    else if (atk_flm == 55 && atk_flg == 1)
    //    {
    //        atk_flg = 0;
    //        atk_flm = 0;
    //        enemyscript.attackflg = false;
    //        enemyscript.lets_attack = false;
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
    //    }
    //    if (atk_flg == 1)
    //        atk_flm++;
    //}

 
    ///// <summary>
    ///// 歩くアニメーション
    ///// </summary>
    ///// <param name="useComp">使用するときに1、使用し終わったら0を代入</param>
    //void WalkOnEnemy(int wlk_flg)
    //{
    //    if (wlk_flm == 0 && wlk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[0];
    //    }
    //    else if (wlk_flm == 8 && wlk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[1];
    //    }
    //    else if (wlk_flm == 16 && wlk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[2];
    //    }
    //    else if (wlk_flm == 24 && wlk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[3];
    //    }
    //    else if (wlk_flm == 32 && wlk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[4];
    //    }
    //    else if (wlk_flm == 40 && wlk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[5];
    //    }
    //    else if (wlk_flm == 48 && wlk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[6];
    //    }
    //    else if (wlk_flm == 56 && wlk_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[7];
    //    }
    //    else if (wlk_flm == 60 && wlk_flg == 1)
    //    {

    //        wlk_flm = 0;
    //    }
    //    else if (wlk_flg == 0)
    //    {
    //        wlk_flm = 0;
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
    //    }

    //    if (wlk_flg == 1)
    //        wlk_flm++;
    //}

    //void RunOnEnemy(int run_flg)
    //{
    //    if (run_flm == 0 && run_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[0];
    //    }
    //    else if (run_flm == 8 && run_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[1];
    //    }
    //    else if (run_flm == 16 && run_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[2];
    //    }
    //    else if (run_flm == 24 && run_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[3];
    //    }
    //    else if (run_flm == 32 && run_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[4];
    //    }
    //    else if (run_flm == 40 && run_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[5];
    //    }
    //    else if (run_flm == 48 && run_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[6];
    //    }
    //    else if (run_flm == 56 && run_flg == 1)
    //    {
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = walked[7];
    //    }
    //    else if (run_flm == 60 && run_flg == 1)
    //    {

    //        run_flm = 0;
    //    }
    //    else if (run_flg == 0)
    //    {
    //        run_flm = 0;
    //        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
    //    }

    //    if (run_flg == 1)
    //        run_flm++;
    //}
}
