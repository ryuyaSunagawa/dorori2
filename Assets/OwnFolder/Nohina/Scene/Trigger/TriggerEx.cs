using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEx : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter( Collider other )
	{
		Debug.Log( "OK" );
	}

	private void OnCollisionEnter( Collision collision )
	{
		Debug.Log( "OK!" );
	}
	private void OnCollisionStay( Collision collision )
	{
		Debug.Log( "OK!!" );
	}
}
