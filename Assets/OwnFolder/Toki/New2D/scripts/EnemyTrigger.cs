using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyTrigger : MonoBehaviour
{


    [SerializeField] private GameObject enemy;
    private New2DEnemy script;

    // Start is called before the first frame update
    void Start()
    {
        script = enemy.GetComponent<New2DEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(script.lets_attack)
        {
            if(GameManager.Instance.playerHideFlg)
            {
                GameManager.Instance.playerHideFlg = false;
                GameManager.Instance.playerDeathFlg = true;
            }
        }
    }
   
    private void OnTriggerStay2D(Collider2D other)
    {

        if(other.gameObject.layer == 13 || other.gameObject.layer == 15)
        {
            
            if(script.angryflg)
            {
                
                GameManager.Instance.playerDeathFlg = true;
            }

            if (script.lets_attack)
            {
                
                GameManager.Instance.playerDeathFlg = true;
            }
            
        }
        if (other.gameObject.layer == 16)
        {
           

            if (script.gameObject.layer == 2)
            {
               
                script.gameObject.GetComponent<New2DEnemy>().poisonState = 2;
            }
        }
    }


}
