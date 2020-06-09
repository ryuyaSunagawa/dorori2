using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sacrifice : MonoBehaviour
{

	[HideInInspector] public Vector3 enablePosition = Vector3.zero;
	[HideInInspector] public bool directRight = false;
	float enableTime = 0f;

	// Start is called before the first frame update
	private void OnEnable()
	{
		transform.position = enablePosition;
		if( directRight == true )
		{
			GetComponent<Rigidbody2D>().AddForce( new Vector2( 3f, -2f ), ForceMode2D.Impulse );
		}
		else if( directRight == false )
		{
			GetComponent<Rigidbody2D>().AddForce( new Vector2( -3f, -2f ), ForceMode2D.Impulse );
		}
	}

	// Update is called once per frame
	void Update()
    {
		enableTime += Time.deltaTime;

		if( enableTime >= 3f )
		{
			enablePosition = Vector3.zero;
			enableTime = 0f;
			gameObject.SetActive( false );
		}
	}
}
