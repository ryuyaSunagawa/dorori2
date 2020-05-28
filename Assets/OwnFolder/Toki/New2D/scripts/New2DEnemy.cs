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

    public bool find = false;                      //プレイヤーを見つけたかフラグ

    [SerializeField] private float range1 = 14.0f;　//敵の発見段階lv1の距離 (？マークを出してゆっくり近づく)
    [SerializeField] private float range2 = 10.0f;  //敵の発見段階lv2の距離 (!?マークを出してすごい形相で追っかける)
    [SerializeField] private float range3 = 6.0f;   //敵の発見段階lv3の距離 (ぶっ殺死!!)

    private float start_range1;                     //range1の初期値が入ってるよ
    private float start_range2;                     //range2の初期値が入ってるよ
    private float start_range3;                     //range3の初期値が入ってるよ

    // public bool range1_flg = false;                     //敵の発見段階lv1に入ったよのフラグ

    // public bool range1_5flg = false;                    //range2の状態で逃げた状態のフラグ

    // public bool range2_flg = false;                     //敵の発見段階lv2に入ったよのフラグ

    // public bool range2_5flg = false;                    //range3の状態で逃げた状態のフラグ

    // public bool orange_flg = false;                     //range2_5に一回入ったかフラグ

    // public bool range3_flg = false;                     //敵の発見段階lv3に入ったよのフラグ

    public float range_level = 0;

    private float escape_playerx = 0.0f;                //プレイヤーが視界から脱出したときに座標を保存するとこ

    private float search_count = 0.0f;                  //プレイヤーが視界から脱出したときに探す時間カウント
    
    [SerializeField] private float search_time = 2.5f;  //プレイヤーが視界から脱出したときに探す時間

    private bool settaiflg = false;                 //プレイヤーの攻撃中に待ってくれる接待フラグ

    private float enemy_size_x;                     //敵の初期サイズX
    private float enemy_size_y;                     //敵の初期サイズY

    int layernum;                                   //PlayerLayerのナンバー入れ
    int layerMask;                                  //PlayerLayerをマスクに変化させたやつ入れ

    // Start is called before the first frame update
    void Start()
    {
        layernum = LayerMask.NameToLayer("PlayerLayer");
        layerMask = 1 << layernum;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        enemy_size_x = transform.localScale.x;
        enemy_size_y = transform.localScale.y;

        start_range1 = range1;      //range達の初期値を保存
        start_range2 = range2;
        start_range3 = range3;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(range_level);
        Debug.Log(find);
        Debug.Log(range1);
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
           Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.name == "Player")
            {
                if ((Vector2.Distance(hit.transform.position, transform.position)) < range1 || range_level == 1.5f || range_level == 2.5f)
                {
                    if(direction && transform.position.x > hit.transform.position.x)
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
                    }
                }
                else
                {
                    find = false;
                }
            }
            else
            {
                find = false;
            }
            

            if (find)
            {
                //Debug.Log(Vector2.Distance(hit.transform.position, transform.position));
                Debug.Log(hit.distance);

                if ((Vector2.Distance(hit.transform.position, transform.position)) < range3)
                {
                    range_level = 3f;
                }
                else if (range_level != 2.5f && (Vector2.Distance(hit.transform.position, transform.position)) < range2)
                {
                    reactioncount = 0.0f;
                    range_level = 2f;
                }
                else if ((Vector2.Distance(hit.transform.position, transform.position)) <= range1)
                {
                    range_level = 1f;
                }

               


                if (range_level == 3f)
                {
                    //range3
                    //侵入者じゃけぇ!!

                    if (transform.position.x < hit.transform.position.x)
                    {
                        rb.velocity = new Vector2(speed * 3.0f, rb.velocity.y);
                    }
                    else if (transform.position.x > hit.transform.position.x)
                    {
                        rb.velocity = new Vector2(-speed * 3.0f, rb.velocity.y);
                    }

                    if ((Vector2.Distance(hit.transform.position, transform.position)) > range1)
                    {
                        escape_playerx = hit.transform.position.x;
                        range_level = 2.5f;
                    }
                }
                else if (range_level == 2.5f)
                {
                    //range2.5
                    //プレイヤーが視界から脱出したときにその地点に行って探す
                    if (search_count < search_time && transform.position.x < escape_playerx + 3.0f && transform.position.x > escape_playerx - 3.0f)
                    {
                        search_count += Time.deltaTime;
                        if (search_time < search_count)
                        {
                            range1 = start_range1;

                            range2 = start_range1;

                            range3 = start_range2;

                            find = false;

                        }
                    }
                    else if (transform.position.x < escape_playerx)
                    {
                        rb.velocity = new Vector2(speed * 2.0f, rb.velocity.y);
                    }
                    else if (transform.position.x > escape_playerx)
                    {
                        rb.velocity = new Vector2(-speed * 2.0f, rb.velocity.y);
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
                    if (transform.position.x < hit.transform.position.x)
                    {
                        rb.velocity = new Vector2(speed * 2.0f, rb.velocity.y);
                    }
                    else if (transform.position.x > hit.transform.position.x)
                    {
                        rb.velocity = new Vector2(-speed * 2.0f, rb.velocity.y);
                    }

                    if ((Vector2.Distance(hit.transform.position, transform.position)) > range1)
                    {
                        escape_playerx = hit.transform.position.x;
                        range_level = 1.5f;
                    }
                }
                else if (range_level == 1.5f)
                {
                    //range1.5
                    range3 = start_range3;

                    //プレイヤーが視界から脱出したときにその地点に行って探す
                    if (transform.position.x < escape_playerx + 3.0f && transform.position.x > escape_playerx - 3.0f)
                    {
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
                            
                        }
                    }
                    else if (transform.position.x < escape_playerx)
                    {
                        rb.velocity = new Vector2(speed * 2.0f, rb.velocity.y);
                    }
                    else if (transform.position.x > escape_playerx)
                    {
                        rb.velocity = new Vector2(-speed * 2.0f, rb.velocity.y);
                    }



                }
                else if (range_level == 1f)
                {
                    //range1
                    //ん？、何かいね？
                    //ピンク
                    reactioncount += Time.deltaTime;

                    if (reaction <= reactioncount)
                    {
                        if (range2 < start_range1)
                        {
                            range2 += (5 * Time.deltaTime);
                        }

                    }

                    if ((Vector2.Distance(hit.transform.position, transform.position)) > range1)
                    {
                        reactioncount = 0.0f;

                        range1 = start_range1;
                        range2 = start_range2;

                        range_level = 0;

                        find = false;
                    }
                }

            }
            
        }

		///<summary>
		///投げ待ち時にパトロールを止める
		/// </summary>
		if( _poisonState != 1  && _poisonState != 2)
		{

			//目的地にいる、プレイヤーを見つけていない//
			if(arrive && (!find || range_level == 2.5f))
			{

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
				}
			}//敵を見つけていない
			else if(!find || range_level == 2.5f)
			{
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
    }

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( collision.transform.tag == "Enemy" && ( _poisonState == 1 || _poisonState == 2) )
		{
			collision.transform.GetComponent<New2DEnemy>().poisonState = 2;
		}
        
	}

}
