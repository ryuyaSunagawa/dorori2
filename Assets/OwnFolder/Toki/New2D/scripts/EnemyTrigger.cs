using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyTrigger : MonoBehaviour
{

    [SerializeField] private New2DEnemy enemyscript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("aiueo");
        if (other.gameObject.layer == 16)
        {
            
            if (enemyscript.gameObject.layer == 2)
            {
               
                enemyscript.gameObject.GetComponent<New2DEnemy>().poisonState = 2;
            }
        }
    }


}
