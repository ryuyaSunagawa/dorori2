using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class New2DEnemy : MonoBehaviour
{
    public bool patrol_only = false;

    [SerializeField] private float speed = 10.0f;

    Rigidbody2D rb;

    private bool direction = false; //false:右, true:左

    public NewEnemyFrontCheck frontcheck;

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
	[SerializeField] float maxPoisonAmount = 3f;

    [SerializeField] private float waittime = 0.0f;
    [SerializeField] private float reaction = 0.0f;

    Ray2D ray;
    RaycastHit2D hit;

    [SerializeField] private float range = 5.0f;

    public bool find = false;

    public bool attack = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

		if( _poisonState == 2 )
		{
			pAmount += Time.deltaTime;
			if( pAmount >= maxPoisonAmount )
			{
				gameObject.SetActive( false );
			}
		}

        //敵の向いている向きのフラグ//
        if (transform.localScale.x < 0)
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
            hit = Physics2D.Raycast(ray.origin, ray.direction, range);
        }

        
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);


        //レイが何かに当たったか？//
        if (hit.collider)
        {
           //Debug.Log(hit.collider.gameObject.name);

            //レイが当たったのはプレイヤー？//
            if(hit.collider.gameObject.name == "Player")
            {
                //プレイヤーをみつけた//
                find = true;

                Debug.Log(hit.distance);


                if(Vector2.Distance(hit.transform.position, transform.position) > range)
                {
                    find = false;
                    reactioncount = 0.0f;
                    attack = false;
                }
                else if (Vector2.Distance(hit.transform.position, transform.position) > range / 2 && attack == false)
                { 
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
                else if(Vector2.Distance(hit.transform.position, transform.position) <= range / 2)
                {
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
                
            }
        }

		///<summary>
		///投げ待ち時にパトロールを止める
		/// </summary>
		if( _poisonState != 1 )
		{
			//前方にオブジェクトがある、プレイヤーを見つけていない//
			if( frontcheck.check && find == false )
			{

				count += Time.deltaTime;
				//数秒まつ
				if( count >= waittime )
				{
					//プレイヤーの向きに画像を合わせる
					if( direction )
					{
						transform.localScale = new Vector2( 0.25f, 0.25f );
					}
					else
					{
						transform.localScale = new Vector2( -0.25f, 0.25f );
					}


					frontcheck.check = false;
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

			}
		}
    }

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( collision.transform.tag == "Enemy" && ( _poisonState == 1 || _poisonState == 2 ) )
		{
			collision.transform.GetComponent<New2DEnemy>().poisonState = 2;
		}
	}

}
