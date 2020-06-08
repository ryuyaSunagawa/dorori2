using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyTrigger : MonoBehaviour
{

     [SerializeField] private New2DEnemy Enemyscript;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemyscript.lets_attack)
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
            if(Enemyscript.angryflg)
            {
                GameManager.Instance.playerMooveFlg = false;
            }
            if (Enemyscript.lets_attack)
            {
                
                GameManager.Instance.playerDeathFlg = true;
            }
            
        }
        if (other.gameObject.layer == 16)
        {
           

            if (Enemyscript.gameObject.layer == 2)
            {
               
                Enemyscript.gameObject.GetComponent<New2DEnemy>().poisonState = 2;
            }
        }
    }


}
