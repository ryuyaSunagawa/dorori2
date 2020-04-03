using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
	[SerializeField] FloorTrigger floorTrigger;

    // Update is called once per frame
    void Update()
    {
		if( floorTrigger.collideChecker == true && Input.GetButtonDown( "Hide" ) )
		{
			Debug.Log( this.transform.up );
			floorTrigger.theObject.GetComponent<PlayerScript2D>().hideFlg = true;

		}
	}

	public Vector2 HidePosition()
	{
		return transform.up + transform.position;
	}
}
