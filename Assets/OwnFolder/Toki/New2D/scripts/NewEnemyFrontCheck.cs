using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyFrontCheck : MonoBehaviour
{
    public New2DEnemy enemyscript;
    public bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
		if( collision.tag == "Border" )
		{
			check = true;
		}

		if(!enemyscript.patrol_only && collision.gameObject.name == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}
