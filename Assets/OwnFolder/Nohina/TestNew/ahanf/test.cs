using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

	float hanten = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if( Input.GetButton( "Hide" ) )
		{
			Debug.Log( hanten );
		}
		else
		{
			Debug.Log( hanten );
		}

	}
}
