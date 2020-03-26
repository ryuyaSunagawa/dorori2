using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.LookAt( GameManager.Instance.getplayerObj.GetChild( 0 ) );
		transform.Rotate( 0f, 180f, 0f, Space.Self );
    }
}
