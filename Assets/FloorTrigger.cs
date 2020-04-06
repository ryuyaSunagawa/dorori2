using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
	public bool collideChecker;
	public GameObject theObject;

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D( Collider2D collision )
	{
		collideChecker = true;
		//Debug.Log( "in" );
		theObject = collision.gameObject;
	}

	private void OnTriggerExit2D( Collider2D collision )
	{
		collideChecker = false;
		//Debug.Log( "out" );
		theObject = null;
	}
}
