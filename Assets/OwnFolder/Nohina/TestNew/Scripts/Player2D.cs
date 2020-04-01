using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		float horizontal = Input.GetAxis( "Horizontal" ) * 0.4f;

		Vector3 horizonPos = new Vector3( horizontal, 0f, 0f );

		Vector3 rightPos = new Vector3( transform.position.x + ( GetComponent<SpriteRenderer>().bounds.size.x / 2f ) + horizontal, transform.position.y, 0 );
		Vector3 leftPos = new Vector3( transform.position.x - ( GetComponent<SpriteRenderer>().bounds.size.x / 2f ) + horizontal, transform.position.y, 0 );
	}

	bool CompBothCorner( Vector3 position )
	{
		if( position.x <= 0 || position.x >= Screen.width )
		{
			return false;
		}

		return true;
	}
}