using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New2DEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;

    Rigidbody2D rb;

    private bool direction = false; //false:左, true:右

    public NewEnemyFrontCheck frontcheck;

    private float count = 0.0f;

    [SerializeField] private float waittime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(transform.localScale.x < 0)
        {
            //みぎ
            direction = true;
        }
        else
        {
            //ひだり
            direction = false;
        }

        if (frontcheck.check)
        {
            count += Time.deltaTime;

            if (count >= waittime)
            {
                if (direction)
                {
                    transform.localScale = new Vector2(0.25f, 0.25f);
                }
                else
                {
                    transform.localScale = new Vector2(-0.25f, 0.25f);
                }

                frontcheck.check = false;
                count = 0.0f;
            }
        }
        else
        {
            if(direction)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            
        }
    }
}
