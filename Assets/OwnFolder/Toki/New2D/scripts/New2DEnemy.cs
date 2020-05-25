using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class New2DEnemy : MonoBehaviour
{
    public bool patrol_only = false;
    [SerializeField] private Material normal;

    [SerializeField] private Material poison;

    [SerializeField] private Sprite deadenemy;

    [SerializeField] private float speed = 2.0f;

    [SerializeField] private GameObject left;//左側の巡回目的地
    [SerializeField] private GameObject rihgt;//右側の巡回目的地

    private bool patrollpoint = true; //巡回目的地　false : 右, true : 左
    private bool arrive = false;//巡回目的地に到着したか

    Rigidbody2D rb;

    SpriteRenderer sr;

    private bool direction = true; //false:右, true:左

    private float count = 0.0f;

    private float reactioncount = 0.0f;

	public int _poisonState = 0;
	public int poisonState
	{
		set {
			_poisonState = value;
		}
	}

	private float pAmount = 0f;
	[SerializeField] float maxPoisonAmount = 1.5f;

    [SerializeField] private float waittime = 0.0f;
    [SerializeField] private float reaction = 0.0f;

    Ray2D ray;
    RaycastHit2D hit;

    [SerializeField] private float range1 = 6.0f;
    [SerializeField] private float range2 = 4.0f;
    [SerializeField] private float range3 = 2.0f;

    public bool find = false;

    public bool attack = false;

    private bool settaiflg = false; //プレイヤーの攻撃中と待ってくれる接待フラグ

    int layernum;
    int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layernum = LayerMask.NameToLayer("PlayerLayer");
        layerMask = 1 << layernum;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

		if( _poisonState == 2 )
		{
            // speed = 1.0f;
            sr.material = poison;
            transform.localScale = new Vector2(-0.47f, 0.47f);
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
        if (!patrol_only && find == false)
        {
            hit = Physics2D.Raycast(ray.origin, ray.direction, range1, layerMask);
        }

        
        Debug.DrawRay(ray.origin, ray.direction * range1, Color.red);


        //レイが何かに当たったか？//
        if (hit.collider && _poisonState != 2)
        {
           Debug.Log(hit.collider.gameObject.name);

            //レイが当たったのはプレイヤー？//
            if(hit.collider.gameObject.name == "Player")
            {
                //プレイヤーをみつけた//
                find = true;

                Debug.Log(hit.distance);


                if(Vector2.Distance(hit.transform.position, transform.position) > range1)
                {
                    //視認距離からでたら追跡をやめる
                    find = false;
                    reactioncount = 0.0f;
                    attack = false;
                }
                else if (Vector2.Distance(hit.transform.position, transform.position) > range2 && attack == false)
                { 
                    //視認距離に入ったらゆっくり近づく
                    reactioncount += Time.deltaTime;

                    if (reaction <= reactioncount)
                    {
                        //プレイヤーの方向に進む//
                        if (transform.position.x < hit.transform.position.x)
                        {
                            rb.velocity = new Vector2(speed / 2, rb.velocity.y);
                        }
                        else if (transform.position.x > hit.transform.position.x)
                        {
                            rb.velocity = new Vector2(-speed / 2, rb.velocity.y);
                        }
                    }
                }
                else if(Vector2.Distance(hit.transform.position, transform.position) >= range3)
                {
                    //完全に見つけたら追いかける
                    attack = true;
                    //プレイヤーの方向に進む//
                    if (transform.position.x < hit.transform.position.x)
                    {
                        rb.velocity = new Vector2(speed * 1.5f, rb.velocity.y);
                    }
                    else if (transform.position.x > hit.transform.position.x)
                    {
                        rb.velocity = new Vector2(-speed * 1.5f, rb.velocity.y);
                    }
                }
                else if(Vector2.Distance(hit.transform.position, transform.position) < range3)
                {
                    Destroy(hit.collider.gameObject);
                }
                
            }
        }

		///<summary>
		///投げ待ち時にパトロールを止める
		/// </summary>
		if( _poisonState != 1  && _poisonState != 2)
		{

			//目的地にいる、プレイヤーを見つけていない//
			if( arrive && find == false )
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
						transform.localScale = new Vector2( 0.47f, 0.47f );
					}
					else
					{
						transform.localScale = new Vector2( -0.47f, 0.47f );
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
			else if( find == false )
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
