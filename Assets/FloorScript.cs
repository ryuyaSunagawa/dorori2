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
			Debug.Log( this.transform.up + transform.position );
			floorTrigger.theObject.GetComponent<PlayerScript2D>().hideFlg = true;
			floorTrigger.theObject.GetComponent<PlayerScript2D>().nextPosition = HidePosition();
		}
	}

	//隠れるポジションを
	public Vector2 HidePosition()
	{
		return transform.up + transform.position;
	}
}
