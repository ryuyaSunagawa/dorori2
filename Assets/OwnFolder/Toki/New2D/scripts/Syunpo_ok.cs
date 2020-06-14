using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syunpo_ok : MonoBehaviour
{
    [SerializeField] private Sprite X;

    [SerializeField] private New2DEnemy script;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (script.direction)
        {
            transform.position = new Vector2(script.transform.position.x + 1, script.transform.position.y + 4);
        }
        else
        {
            transform.position = new Vector2(script.transform.position.x - 1, script.transform.position.y + 4);
        }
        if(script.attack_avoid)
        {
            GetComponent<SpriteRenderer>().sprite = X;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
