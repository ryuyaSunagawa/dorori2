using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
	[SerializeField] Transform targetObj;
	Vector3 normalPos;

	[SerializeField, Range( 0f, 10f )] float xPosDistance = 5f;
	[SerializeField, Range( 0f, 10f )] float yPosDistance = 1.75f;

    // Start is called before the first frame update
    void Start()
    {
		normalPos = this.transform.position;
    }

	// Update is called once per frame
	void Update()
	{
		Vector3 targetPos = new Vector3( targetObj.position.x, targetObj.position.y, this.transform.position.z );

		if( GameManager.Instance.playerStairState == 1 )
		{
			targetPos.x += xPosDistance;
			targetPos.y = normalPos.y;

			this.transform.position = targetPos;
			Debug.Log( targetPos ) ;
		}
		else if ( !( GameManager.Instance.playerStairState == 1 ) )
		{
			targetPos.x += xPosDistance;
			targetPos.y += yPosDistance;

			this.transform.position = targetPos;
		}
    }
}
