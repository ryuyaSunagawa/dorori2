using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class textchilldren : MonoBehaviour
{

    [SerializeField] private GameObject enemy;

    [SerializeField] private New2DEnemy script;

    private Text changetext;

    // Start is called before the first frame update
    void Start()
    {
        changetext = GetComponent<Text>();
        changetext.fontSize = 35;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 2);

        if(script.find && script._poisonState != 3)
        {
            
            if(script.attack)
            {
                changetext.text = "!?";
                changetext.color = Color.yellow;
            }
            else
            {
                changetext.text = "?";
                changetext.color = Color.yellow;
            }
        }
        else
        {
            changetext.text = "";
        }
    }
}
