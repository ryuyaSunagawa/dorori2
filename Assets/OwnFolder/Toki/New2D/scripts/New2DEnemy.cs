using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class New2DEnemy : MonoBehaviour
{
    public bool not_patrol = false;

    public bool patrol_only = false;            //パトロールだけさせる時用

    [SerializeField] private Material normal;   //通常のマテリアル

    [SerializeField] private Material poison;   //紫色マテリアル

    [SerializeField] private ParticleSystem poisonpar;//敵のポイズンパーティクル

    private bool poisonpar_onetime = false;     //一回再生したか

    [SerializeField] private Sprite deadenemy;  //お遊びで作った死体スプライト

    [SerializeField] private float speed = 3.0f;//敵の歩行スピード

    [SerializeField] private GameObject left;   //左側の巡回目的地
    [SerializeField] private GameObject rihgt;  //右側の巡回目的地

    private bool patrollpoint = true;           //巡回目的地　false : 右, true : 左
    private bool arrive = false;                //巡回目的地に到着したか

    Rigidbody2D rb;                             //敵のリジットボデー

    SpriteRenderer sr;                          //敵のスプライトレンダラー

    [HideInInspector] public bool direction = true;              //false:右, true:左

    private float count = 0.0f;                 //巡回地点についた時の待ち時間カウント

    [SerializeField] private float waittime = 0.0f;//巡回地点についた時の待ち時間

    private float reactioncount = 0.0f;         //プレイヤーを見つけた時の反応時間カウント

    [SerializeField] private float reaction = 1.5f;//プレイヤーを見つけた時の反応時間


    public int _poisonState = 0;
    public int poisonState
    {
        set {
            _poisonState = value;
        }
    }

    private float pAmount = 0f;
    [SerializeField] float maxPoisonAmount = 1.5f;


    Ray2D ray;                                      //敵のレイ
    RaycastHit2D hit;                               //レイが当たった奴の保存箱

    [HideInInspector] public bool find = false;                      //プレイヤーを見つけたかフラグ

    [HideInInspector] public bool tomadoi = false;                  //瞬歩でかわされたら少し止まって振り返る処理フラグ

    [SerializeField] private float range1 = 14.0f;　//敵の発見段階lv1の距離 (？マークを出してゆっくり近づく)
    [SerializeField] private float range2 = 10.0f;  //敵の発見段階lv2の距離 (!?マークを出してすごい形相で追っかける)
    [SerializeField] private float range3 = 6.0f;   //敵の発見段階lv3の距離 (ぶっ殺死!!)

    private float start_range1;                     //range1の初期値が入ってるよ
    private float start_range2;                     //range2の初期値が入ってるよ
    private float start_range3;                     //range3の初期値が入ってるよ


    [HideInInspector] public float range_level = 0;                   //プレイヤーの発見段階

    [HideInInspector] public bool r2_5flg = false;                    //rangelevel2.5になったらrange2の距離を常時広がってる状況にするためのフラグ

    private float escape_playerx = 0.0f;                //プレイヤーが視界から脱出したときに座標を保存するとこ

    private float search_count = 0.0f;                  //プレイヤーが視界から脱出したときに探す時間カウント

    [SerializeField] private float search_time = 2.5f;  //プレイヤーが視界から脱出したときに探す時間

    private bool settaiflg = false;                 //プレイヤーの攻撃中に待ってくれる接待フラグ

    [HideInInspector] public bool deathflg;       //死亡フラグ

    private float death_time = 0.5f;
    private float death_count = 0.0f;

    [HideInInspector] public bool meltdowner = false;   //敵が死んだあと溶ける処理

    private float meltdestroy_time = 8.0f;              //敵が溶けるて消えるまでの時間
    private float meltdestroy_count = 0.0f;

    private bool set_poisonflg = true;

    [HideInInspector] public bool attackflg = false;     //攻撃のモーションに入るときのフラグ(アニメーションの引き金)

    [HideInInspector] public bool lets_attack = false;   //攻撃の判定出すときのフラグ

    [HideInInspector] public bool attack_avoid = false; //敵の攻撃を回避できるフレームのフラグ

    [HideInInspector] public bool walkflg = false;        //歩いてる時のフラグ(アニメーションの引き金)

    [HideInInspector] public bool runflg = false;        //走ってる時のフラグ(アニメーションの引き金)

    [HideInInspector] public bool angryflg = false;       //敵の瞬歩弾きのフラグ(アニメーションの引き金)

    private float enemy_size_x;                     //敵の初期サイズX
    private float enemy_size_y;                     //敵の初期サイズY

    int layernum;                                   //PlayerLayerのナンバー入れ
    int layerMask;                                  //PlayerLayerをマスクに変化させたやつ入れ

    [SerializeField] public GameObject Player;             //プレイヤー

    private FixPlayerScript P_script;

    private SpriteRenderer P_Sprite;                //敵のspriterendrer

    [SerializeField] public GameManager game_manager;

    private bool Hide_past = false;        //1フレーム前のhideflg（現在のhideflgと前のhideflgを比べて隠れたタイミングをとらえる）

    private bool Hide_Timeing = true;       //発見された状態で隠れたか、その前に隠れていたか

    [HideInInspector] public bool suspicious = false;       //ピンクでプレイヤーを発見したときの止まって注視のフラグ

    private bool Syunpo_past = false;                       //1フレーム前の瞬歩

    [HideInInspector] public bool Syunpo_Timeing = true;                    //成功する瞬歩か、失敗する瞬歩か

    [SerializeField] private float tomadoi_time = 2.0f;     //敵が瞬歩によってプレイヤーを見失た時とまる時間

    private float tomadoi_count = 0.0f;                     //↑のカウント用

    private bool Disguice_Past = false;                     //1フレーム前の変化

    private bool Disguice_Timing = true;                    //ばれる変化か、ばれない変化か

    [SerializeField] private AudioClip find_se;
    [SerializeField] private AudioClip hit_se;
    [SerializeField] private AudioClip asioto_se;
    [SerializeField] private AudioClip attack_se;
    [SerializeField] private AudioClip angry_se;

    private AudioSource enemy_se;

    private bool find_se_flg;
    private bool hit_se_flg;
    private bool asioto_se_flg;
    private bool attack_se_flg;
    private bool angry_se_flg;


    // Start is called before the first frame update
    void Start()
    {
        
        layernum = LayerMask.NameToLayer("PlayerLayer");
        layerMask = 1 << layernum;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        enemy_size_x = transform.localScale.x;
        enemy_size_y = transform.localScale.y;


        start_range1 = range1 + (Player.transform.localScale.x / 2);      //range達の初期値を保存
        start_range2 = range2 + (Player.transform.localScale.x / 2);
        start_range3 = range3 + (Player.transform.localScale.x / 2);

        range1 = start_range1;
        range2 = start_range2;
        range3 = start_range3;

        P_script = Player.GetComponent<FixPlayerScript>();

        P_Sprite = Player.GetComponent<SpriteRenderer>();

        enemy_se = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(range_level);
        //Debug.Log(find);
        //Debug.Log(range1);
        if ( _poisonState == 2 )
		{
            // speed = 1.0f;
            sr.material = poison;
            transform.localScale = new Vector2(-enemy_size_x, enemy_size_y);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            //下のコメントはずしたらZ区分になるよ
            //sr.sprite = deadenemy;
            //sr.material = normal;
            gameObject.layer = 16;

            pAmount += Time.deltaTime;
			if( pAmount >= maxPoisonAmount )
			{
                //gameObject.SetActive( false );
                _poisonState = 3;
			}
		}
        if(_poisonState == 3)
        {
            Destroy(gameObject);
            //transform.localScale = new Vector2(-1.2f, 1.2f);
            //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            //下のコメントはずしたらZ区分になるよ
            //sr.sprite = deadenemy;
            //sr.material = normal;
            //gameObject.layer = 16;
        }

        //敵の向いている向きのフラグ//
        if (transform.localScale.x > 0)
        {
            //左
            direction = true;
        }
        else
        {
            //右
            direction = false;
        }

        //向いてる向きにレイを設定//
        if (direction)
        {
            ray = new Ray2D(transform.position, Vector2.left);
        }
        else
        {
            ray = new Ray2D(transform.position, Vector2.right);
        }

        //まだプレイヤーを見つけれていならレイをとばす//
        if (!patrol_only && !hit)
        {
            hit = Physics2D.Raycast(ray.origin, ray.direction, range1, layerMask);
        }
        
        Debug.DrawRay(ray.origin, ray.direction * range1, Color.red);

        
        if(deathflg)
        {
            death_count += Time.deltaTime;
            if (death_count > death_time)
            {
                gameObject.layer = 16;

                walkflg = false;
                runflg = false;
                attackflg = false;
                angryflg = false;
                range_level = 0.0f;


                if (meltdowner)
                {
                    if (!poisonpar_onetime && !poisonpar.isPlaying)
                    {
                        poisonpar.Play(true);
                        poisonpar_onetime = true;
                    }

                    sr.material = poison;
                    sr.material.SetFloat("_Start", 1.0f);
                    sr.material.SetFloat("_Timer", meltdestroy_count);
                    if (set_poisonflg)
                    {

                        sr.material.SetFloat("_Down", 5.0f);
                        sr.material.SetFloat("_Alpha", 0.0f);
                        set_poisonflg = false;
                    }



                    meltdestroy_count += Time.deltaTime;
                    if (meltdestroy_count > meltdestroy_time)
                    {
                        Destroy(gameObject.transform.parent.gameObject);
                    }

                }
            }
        }


        //レイが何かに当たったか？//
        if (hit.collider && !deathflg && !GameManager.Instance.playerRespawnFlg)
        {

            
            if (hit.collider.gameObject.layer == 13)
            {

               
                //プレイヤーがrange1内にいるまたは、ranelevelが1.5または、rangelevelが2.5fまたは、敵が攻撃動作にはいってる
                if ((Vector2.Distance(hit.transform.position, transform.position)) < range1 || range_level == 1.5f || range_level == 2.5f || attackflg)
                {
                    suspicious = false;

                    if (attackflg)
                    {
                        walkflg = false;
                        runflg = false;
                        
                    }
                    else
                    {
                        lets_attack = false;
                        walkflg = true;
                    }

                    if(angryflg)
                    {
                        attackflg = false;
                        walkflg = false;
                        runflg = false;
                    }
                    else
                    {
                        walkflg = true;
                    }


                    if (settaiflg)
                    {
                        find = false;
                        walkflg = false;
                        runflg = false;
                        range_level = 0f;
                        
                    }


                    if (direction && transform.position.x > hit.transform.position.x)
                    {
                        find = true;
                    }
                    else if(!direction && transform.position.x < hit.transform.position.x)
                    {
                        find = true;
                    }
                    else
                    {
                        ////瞬歩によって後ろに回られた場合戸惑いフラグをtrueにする
                        if(find && Syunpo_Timeing &&GameManager.Instance.playerMooveFlg)
                        {
                            tomadoi = true;
                        }
                        find = false;
                        range_level = 0;
                    }

                    
                }
                else
                {
                    
                    suspicious = false;
                    if(!attackflg && !angryflg)
                    {
                        if (!not_patrol)
                        {
                            walkflg = true;
                        }
                    }


                    if (range_level == 3f)
                    {
                        escape_playerx = hit.transform.position.x;

                        r2_5flg = true;
                        
                        find = true;
                        range_level = 2.5f;
                    }
                    else if(range_level == 2f)
                    {
                        
                        escape_playerx = hit.transform.position.x;

                        find = true;
                        if (!r2_5flg)
                        {
                            range2 = start_range2;
                            range_level = 1.5f;
                        }
                       
                    }
                    else
                    {
                        suspicious = false;
                        find = false;
                        range_level = 0;
                        reactioncount = 0.0f;
                    }
                    
                }
            }

            ///////隠れるとの連携//////////////////
            if (GameManager.Instance.playerHideFlg)
            {
                HideTimingCheck();

                if(Hide_Timeing)
                {
                    find = false;
                    range_level = 0;
                }

            }
            else
            {
                Hide_past = false;
                Hide_Timeing = true;
            }


            ///////変化との連携////////////////////
            if(GameManager.Instance.playerDisguiceFlg)
            {
                DisguiceTimingCheck();

                Debug.Log("ahan");
                if(Disguice_Timing)
                {
                    find = false;
                    range_level = 0f;
                }
            }
            else
            {
                Disguice_Past = false;
                Disguice_Timing = true;
            }


            ///////瞬歩によってプレイヤを見失ったらしばらくとまる処理//////////
            if(tomadoi && !settaiflg)
            {
                walkflg = false;
                runflg = false;
                tomadoi_count += Time.deltaTime;
                if (tomadoi_count > tomadoi_time)
                {
                    if (direction)
                    {
                        transform.localScale = new Vector2(-enemy_size_x, enemy_size_y);
                        direction = false;
                        patrollpoint = false;
                    }
                    else if (!direction)
                    {
                        transform.localScale = new Vector2(enemy_size_x, enemy_size_y);
                        direction = true;
                        patrollpoint = true;
                    }

                    tomadoi = false;
                    tomadoi_count = 0.0f;
                }
            }


            ////////プレイヤーを発見した場合の遷移処理///////////////////
            if (find)
            {

               

                if ((Vector2.Distance(hit.transform.position, transform.position)) < range3 || attackflg)
                {
                    range_level = 3f;
                }
                else if (range_level != 3f &&(Vector2.Distance(hit.transform.position, transform.position)) < range2)
                {
                    if (!attackflg)
                    {
                        walkflg = false;
                        runflg = true;
                    }
                    suspicious = false;
                    reactioncount = 0.0f;
                    range_level = 2f;
                }
                else if (range_level != 1.5 && range_level != 2f && range_level != 2.5f && range_level != 3f &&(Vector2.Distance(hit.transform.position, transform.position)) <= range1)
                {
                    range_level = 1f;
                }


                //////////瞬歩との連携////////////////
                if (GameManager.Instance.playerMooveFlg)
                {
                    SyunpoTimingCheck();

                    range_level = 0f;
                    walkflg = false;
                    runflg = false;

                    if (!Syunpo_Timeing)
                    {
                        attackflg = false;
                        
                        angryflg = true;
                    }
                }
                else
                {
                    Syunpo_past = false;
                    Syunpo_Timeing = true;
                }




                if (range_level == 3f)
                {
                    //range3
                    //侵入者じゃけぇ!!
                    if (!attackflg)
                    {
                        runflg = true;
                        if (transform.position.x < hit.transform.position.x)
                        {
                            rb.velocity = new Vector2(speed * 3.0f, rb.velocity.y);
                        }
                        else if (transform.position.x > hit.transform.position.x)
                        {
                            rb.velocity = new Vector2(-speed * 3.0f, rb.velocity.y);
                        }
                    }

                    if ((Vector2.Distance(hit.transform.position, transform.position)) < (start_range3 / 1.5))
                    {
                        if (!GameManager.Instance.playerAttackNowFlg)
                        {
                            walkflg = false;
                            runflg = false;
                            attackflg = true;
                        }
                    }
                }
                else if (range_level == 2.5f)
                {
                    //range2.5
                    //プレイヤーが視界から脱出したときにその地点に行って探す
                    if (search_count < search_time && transform.position.x < escape_playerx + 3.0f && transform.position.x > escape_playerx - 3.0f)
                    {
                        if (transform.position.x < escape_playerx + 3.0f && transform.position.x > escape_playerx - 3.0f)
                        {
                            walkflg = false;
                            runflg = false;
                            search_count += Time.deltaTime;
                            if (search_time < search_count)
                            {
                                if(!r2_5flg)
                                {
                                    start_range1 = start_range1 - (start_range1 / 5);
                                }
                                range1 = start_range1;

                                range2 = start_range1;

                                range3 = start_range2;

                                search_count = 0.0f;

                                range_level = 0;

                                r2_5flg = true;

                                find = false;

                                walkflg = true;

                            }
                        }
                    }
                    else if(!attackflg)
                    {
                        walkflg = false;
                        runflg = true;
                        if (transform.position.x < escape_playerx)
                        {
                            rb.velocity = new Vector2(speed * 2.0f, rb.velocity.y);
                        }
                        else if (transform.position.x > escape_playerx)
                        {
                            rb.velocity = new Vector2(-speed * 2.0f, rb.velocity.y);
                        }
                    }
                }
                else if (range_level == 2)
                {
                    //range2
                    //誰かいるぞ!
                    //黄色
                    range2 = start_range1;


                    if (range3 < start_range2)
                    {
                        range3 += (3 * Time.deltaTime);
                    }

                    //プレイヤーの方向に進む//
                    if (!attackflg)
                    {
                        walkflg = false;
                        runflg = true;
                        if (transform.position.x < hit.transform.position.x)
                        {
                            rb.velocity = new Vector2(speed * 2.0f, rb.velocity.y);
                        }
                        else if (transform.position.x > hit.transform.position.x)
                        {
                            rb.velocity = new Vector2(-speed * 2.0f, rb.velocity.y);
                        }
                    }

                }
                else if (range_level == 1.5f)
                {
                    //range1.5
                    range3 = start_range3;

                    //プレイヤーが視界から脱出したときにその地点に行って探す
                    if (transform.position.x < escape_playerx + 3.0f && transform.position.x > escape_playerx - 3.0f)
                    {
                        runflg = false;
                        walkflg = false;
                        search_count += Time.deltaTime;
                        if (search_time < search_count)
                        {
                            range1 = start_range1;

                            range2 = start_range2;

                            range3 = start_range3;

                            escape_playerx = 0.0f;

                            search_count = 0.0f;

                            range_level = 0;

                            find = false;

                            walkflg = true;
                            
                        }
                    }
                    else if (!attackflg)
                    {
                        walkflg = false;
                        runflg = true;
                        if (transform.position.x < escape_playerx)
                        {
                            rb.velocity = new Vector2(speed * 2.0f, rb.velocity.y);
                        }
                        else if (transform.position.x > escape_playerx)
                        {
                            rb.velocity = new Vector2(-speed * 2.0f, rb.velocity.y);
                        }
                    }



                }
                else if (range_level == 1f)
                {
                    //range1
                    //ん？、何かいね？
                    //ピンク
                    //sr.sprite = ;
                    suspicious = true;
                    walkflg = false;
                    reactioncount += Time.deltaTime;
                    if (reaction <= reactioncount)
                    {
                        
                        if (range2 < start_range1)
                        {
                            range2 += (5 * Time.deltaTime);
                        }

                    }
                }

            }

            if(attackflg)
            {
                GameManager.Instance.enemyState = 4;
            }
            else if(range_level == 3)
            {
                GameManager.Instance.enemyState = 3;
            }
            else if(range_level == 2)
            {
                GameManager.Instance.enemyState = 2;
            }
            else if(range_level == 1)
            {
                GameManager.Instance.enemyState = 1;
            }
            else
            {
                GameManager.Instance.enemyState = 0;
            }
            
        }

        //Debug.Log(GameManager.Instance.enemyState);
        
		///<summary>
		///投げ待ち時にパトロールを止める
		/// </summary>
		if(!not_patrol && !settaiflg && !deathflg && !tomadoi && !GameManager.Instance.playerRespawnFlg)
		{

			//目的地にいる、プレイヤーを見つけていない//
			if(arrive && !find && !attackflg && !angryflg)
			{
                walkflg = false;
				count += Time.deltaTime;
				//数秒まつ
				if( count >= waittime )
				{
                    if(direction)
                    {
                        direction = false;
                    }
                    else
                    {
                        direction = true;
                    }
					//プレイヤーの向きに画像を合わせる
					if( direction )
					{
						transform.localScale = new Vector2( enemy_size_x, enemy_size_y );
					}
					else
					{
						transform.localScale = new Vector2( -enemy_size_x, enemy_size_y );
					}

                    if(patrollpoint)
                    {//目的地を右側に
                        patrollpoint = false;
                    }
                    else
                    {//目的地を左側に
                        patrollpoint = true;
                    }

                    arrive = false;
					//frontcheck.check = false;
					count = 0.0f;
                    runflg = false;
                    walkflg = true;
				}
			}//敵を見つけていない
			else if(!find && !attackflg && !angryflg)
			{
                runflg = false;
                walkflg = true;

				//巡回//
				if( direction )
				{
					rb.velocity = new Vector2( -speed, rb.velocity.y );
				}
				else
				{
					rb.velocity = new Vector2( speed, rb.velocity.y );
				}
                
                if(patrollpoint)
                {//目的地が左側の場合

                    if(transform.position.x <= left.transform.position.x)
                        arrive = true;//目的地に到着
                }
                else
                {//目的地が右側の場合

                    if (transform.position.x >= rihgt.transform.position.x)
                        arrive = true;//目的地に到着
                }

			}
		}
        else if(!find && not_patrol)
        {
            walkflg = false;
            runflg = false;
        }

        Enemy_Sound();

        Debug.Log(attackflg);
    }

    /// <summary>
    /// プレイヤーが隠れたタイミングをチェックする
    /// </summary>
    private void HideTimingCheck()
    {
        if(!Hide_past)
        {
            if(range_level <= 1.5f)
            {
                Hide_Timeing = true;
            }
            else
            {
                Hide_Timeing = false;
            }
        }

        Hide_past = GameManager.Instance.playerHideFlg;
    }

    /// <summary>
    /// 瞬歩を発動タイミングをチェック
    /// </summary>
    private void SyunpoTimingCheck()
    {
        if(!Syunpo_past)
        {
            //プレイヤーと対面している瞬歩のみ威嚇する
            if ((direction && P_Sprite.flipX) || (!direction && !P_Sprite.flipX))
            {
                if (range_level == 3 && attack_avoid)
                {
                    Syunpo_Timeing = true;
                }
                else
                {
                    Syunpo_Timeing = false;
                }
            }
        }

        Syunpo_past = GameManager.Instance.playerMooveFlg;
    }

    private void  DisguiceTimingCheck()
    {
        if(!Disguice_Past)
        {
            if(range_level <= 2.5f)
            {
                Disguice_Timing = true;
            }
            else
            {
                Disguice_Timing = false;
            }
        }

        Disguice_Past = GameManager.Instance.playerDisguiceFlg;
    }

    private void Enemy_Sound()
    {

        ////////攻撃中なら攻撃SE鳴らす////////
        if(lets_attack)
        {
            //すでにSEがなってたら終わるまでは鳴らさない
            if(!attack_se_flg)
            {
                enemy_se.PlayOneShot(attack_se);
                attack_se_flg = true;
            }
        }
        else
        {
            attack_se_flg = false;
        }

        /////////歩行のSE/////////////////////
        if((walkflg || runflg) && !suspicious)
        {
            //すでにSEがなってたら終わるまでは鳴らさない
            if (!asioto_se_flg)
            {
                enemy_se.PlayOneShot(asioto_se);
                asioto_se_flg = true;
            }
        }
        else
        {
            asioto_se_flg = false;
        }

        ////////敵の攻撃が当たったSE///////////
        if(GameManager.Instance.playerDeathFlg)
        {
            //すでにSEがなってたら終わるまでは鳴らさない
            if (!hit_se_flg)
            {
                enemy_se.PlayOneShot(hit_se);
                hit_se_flg = true;
            }
        }
        else
        {
            hit_se_flg = false;
        }

        ////////敵の発見SE////////////////////
        if(range_level == 3)
        {
            //すでにSEがなってたら終わるまでは鳴らさない
            if (!find_se_flg)
            {
                enemy_se.PlayOneShot(find_se);
                find_se_flg = true;
            }
        }
        else
        {
            find_se_flg = false;
        }

        ////////敵の威嚇SE/////////////////////
        if(angryflg)
        {
            if(angry_se_flg)
            {
                enemy_se.PlayOneShot(angry_se);
                angry_se_flg = true;
            }
        }
        else
        {
            angry_se_flg = false;
        }
    }


    private void OnCollisionEnter2D( Collision2D collision )
	{
        
		if( collision.transform.tag == "Enemy" && ( _poisonState == 1 || _poisonState == 2) )
		{
			collision.transform.GetComponent<New2DEnemy>().poisonState = 2;
		}
        
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(GameManager.Instance.playerAttackNowFlg)
        {
            settaiflg = true;
            runflg = false;
            walkflg = false;
            find = false;
            range_level = 0f;
        }
    }

}
