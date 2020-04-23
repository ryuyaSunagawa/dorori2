using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPlayerScript : MonoBehaviour
{
	[SerializeField] private ContactFilter2D filter;

    // Start is called before the first frame update
    void Start()
    {
		Vector2 gravity = Physics2D.gravity;
    }

    // Update is called once per frame
    void Update()
    {
		if( Input.GetKeyDown( KeyCode.F ) )
		{
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

		}
    }

	//private void FixedUpdate()
	//{
	//	//transform.Translate( new Vector2( 0f, 0.05f ) );
	//	Collider2D[] result = new Collider2D[ 5 ];
	//	GetComponent<BoxCollider2D>().OverlapCollider( filter, result );

	//	for( int i = 0; i < 5; i++ )
	//	{
	//		if( result[ i ] == null )
	//		{
	//			break;
	//		}
	//		Debug.Log( result[ i ].ToString() );
	//	}
	//}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		Debug.Log( collision.collider.name );
	}
}
