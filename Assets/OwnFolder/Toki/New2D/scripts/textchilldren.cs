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

        if(script._poisonState != 3)
        {
            if (script.range_level == 3f)
            {
                changetext.text = "!!";
                changetext.color = Color.red;
            }


            else if (script.range_level == 2f || script.range_level == 2.5f)
            {
                changetext.text = "!?";
                changetext.color = Color.yellow;
            }
            else if (script.range_level == 1f || script.range_level == 1.5f)
            {
                changetext.text = "?";
                changetext.color = Color.yellow;
            }
            else
            {
                changetext.text = "";
            }
        }
        else
        {
            changetext.text = "";
        }
    }
}
