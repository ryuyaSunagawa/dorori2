using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTriggerScript : MonoBehaviour
{
	private BoxCollider2D myCollider;
	private Collider2D[] results = new Collider2D[ 5 ];

	private GameObject _enemyObject;
	public GameObject enemyObject
	{
		get {
			return _enemyObject;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		myCollider = GetComponent<BoxCollider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	public bool IsHitEnemy()
	{
		int hitCount = myCollider.OverlapCollider( new ContactFilter2D(), results );

		if( hitCount > 0 )
		{
			for( int i = 0; i < hitCount; i++ )
			{
				if( results[ i ].tag == "Enemy" )
				{
					_enemyObject = results[ i ].gameObject;
					return true;
				}
			}
		}
		return false;
	}
}
