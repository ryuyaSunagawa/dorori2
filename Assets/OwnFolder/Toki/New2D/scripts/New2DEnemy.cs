using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class New2DEnemy : MonoBehaviour
{

    public bool patrol_only = false;            //パトロールだけさせる時用

    [SerializeField] private Material normal;   //通常のマテリアル

    [SerializeField] private Material poison;   //紫色マテリアル

    [SerializeField] private Sprite deadenemy;  //お遊びで作った死体スプライト

    [SerializeField] private float speed = 3.0f;//敵の歩行スピード

    [SerializeField] private GameObject left;   //左側の巡回目的地
    [SerializeField] private GameObject rihgt;  //右側の巡回目的地

    private bool patrollpoint = true;           //巡回目的地　false : 右, true : 左
    private bool arrive = false;                //巡回目的地に到着したか

    Rigidbody2D rb;                             //敵のリジットボデー

    SpriteRenderer sr;                          //敵のスプライトレンダラー

    private bool direction = true;              //false:右, true:左

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

    [HideInInspector]public bool find = false;                      //プレイヤーを見つけたかフラグ

    [SerializeField] private float range1 = 14.0f;　//敵の発見段階lv1の距離 (？マークを出してゆっくり近づく)
    [SerializeField] private float range2 = 10.0f;  //敵の発見段階lv2の距離 (!?マークを出してすごい形相で追っかける)
    [SerializeField] private float range3 = 6.0f;   //敵の発見段階lv3の距離 (ぶっ殺死!!)

    private float start_range1;                     //range1の初期値が入ってるよ
    private float start_range2;                     //range2の初期値が入ってるよ
    private float start_range3;                     //range3の初期値が入ってるよ


    [HideInInspector]public float range_level = 0;                   //プレイヤーの発見段階

    [HideInInspector]public bool r2_5flg = false;                    //rangelevel2.5になったらrange2の距離を常時広がってる状況にするためのフラグ

    private float escape_playerx = 0.0f;                //プレイヤーが視界から脱出したときに座標を保存するとこ

    private float search_count = 0.0f;                  //プレイヤーが視界から脱出したときに探す時間カウント
    
    [SerializeField] private float search_time = 2.5f;  //プレイヤーが視界から脱出したときに探す時間

    private bool settaiflg = false;                 //プレイヤーの攻撃中に待ってくれる接待フラグ

    [HideInInspector]public bool attackflg = false;

    //[HideInInspector]public float atk_motion_time = 0.6f;

    //private float atk_motion_count = 0.0f;

    [HideInInspector]public bool lets_attack = false;

    [HideInInspector]public bool walkflg = true;

    [HideInInspector]public bool runflg = false;

    [HideInInspector] public bool angflg = false;

    private float enemy_size_x;                     //敵の初期サイズX
    private float enemy_size_y;                     //敵の初期サイズY

    int layernum;                                   //PlayerLayerのナンバー入れ
    int layerMask;                                  //PlayerLayerをマスクに変化させたやつ入れ

    [SerializeField] GameObject Player;             //プレイヤー

    private FixPlayerScript P_script;

    [SerializeField] public GameManager game_manager;

    [SerializeField] private bool Hide_past = false;        //1フレーム前のhideflg（現在のhideflgと前のhideflgを比べて隠れたタイミングをとらえる）

    [SerializeField] private bool Hide_Timeng = true;       //発見された状態で隠れたか、その前に隠れていたか

    [HideInInspector] public bool suspicious = false;       //ピンクでプレイヤーを発見したときの止まって注視のフラグ

    [HideInInspector] public bool Angryflg = false;

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


        //レイが何かに当たったか？//
        if (hit.collider && _poisonState != 2)
        {
           Debug.Log(attackflg);

            if (hit.collider.name == "Player")
            {

               
                //プレイヤーがrange1内にいるまたは、ranelevelが1.5または、rangelevelが2.5fまたは、敵が攻撃動作にはいってる
                if ((Vector2.Distance(hit.transform.position, transform.position)) < range1 || range_level == 1.5f || range_level == 2.5f || attackflg)
                {

                    if (attackflg)
                    {
                        walkflg = false;
                        //atk_motion_count += Time.deltaTime;
                        /*if (atk_motion_count >= atk_motion_time)
                        {
                            lets_attack = true;
                        }*/
                    }
                    else
                    {
                        //atk_motion_count = 0.0f;
                        walkflg = true;
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
                        find = false;
                        range_level = 0;
                    }

                    
                }
                else
                {
                    if(!attackflg)
                    {
                        //atk_motion_count = 0.0f;
                        walkflg = true;
                    }

                    if (range_level == 3f)
                    {
                        escape_playerx = hit.transform.position.x;
                        
                        find = true;
                        range_level = 2.5f;
                    }
                    else if(range_level == 2f)
                    {
                        escape_playerx = hit.transform.position.x;

                        find = true;
                        if (!r2_5flg)
                        {
                            range_level = 1.5f;
                        }
                        else
                        {
                            range_level = 2.5f;
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

            if (GameManager.Instance.playerHideFlg)
            {
                HideTimingCheck();

                if(Hide_Timeng)
                {
                    find = false;
                    range_level = 0;
                }

            }
            else
            {
                Hide_past = false;
                Hide_Timeng = true;
            }




            if (find)
            {
                //Debug.Log(Vector2.Distance(hit.transform.position, transform.position));
                //Debug.Log(hit.distance);

                if ((Vector2.Distance(hit.transform.position, transform.position)) < range3 || attackflg)
                {
                    range_level = 3f;
                }
                else if (range_level != 3f &&(Vector2.Distance(hit.transform.position, transform.position)) < range2)
                {
                    if (!attackflg)
                    {
                        walkflg = true;
                    }
                    suspicious = false;
                    reactioncount = 0.0f;
                    range_level = 2f;
                }
                else if (range_level != 2f && range_level != 2.5f && range_level != 3f &&(Vector2.Distance(hit.transform.position, transform.position)) <= range1)
                {
                    range_level = 1f;
                }

               


                if (range_level == 3f)
                {
                    //range3
                    //侵入者じゃけぇ!!
                    if (!attackflg)
                    {
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
                        walkflg = false;
                        attackflg = true;
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
            
        }

        Debug.Log(walkflg);
        
		///<summary>
		///投げ待ち時にパトロールを止める
		/// </summary>
		if( _poisonState != 1  && _poisonState != 2)
		{

			//目的地にいる、プレイヤーを見つけていない//
			if(arrive && !find && !attackflg)
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
                    walkflg = true;
				}
			}//敵を見つけていない
			else if(!find && !attackflg)
			{
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

        //hideflgがfalseの時、アップデートが終わるときに現在のhideflgを保存しておく
        
            
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
                Hide_Timeng = true;
            }
            else
            {
                Hide_Timeng = false;
            }
        }

        Hide_past = GameManager.Instance.playerHideFlg;
    }

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( collision.transform.tag == "Enemy" && ( _poisonState == 1 || _poisonState == 2) )
		{
			collision.transform.GetComponent<New2DEnemy>().poisonState = 2;
		}
        
	}

}
