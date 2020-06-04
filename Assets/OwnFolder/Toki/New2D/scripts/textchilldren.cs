using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class textchilldren : MonoBehaviour
{

    [SerializeField] private GameObject enemy;

    [SerializeField] private New2DEnemy script;

    [SerializeField] private Sprite red_reaction;

    [SerializeField] private Sprite red_question;

    [SerializeField] private Sprite yellow_question;

    SpriteRenderer reaction_renderer;

    private Text changetext;

    // Start is called before the first frame update
    void Start()
    {
        reaction_renderer = GetComponent<SpriteRenderer>();
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
                reaction_renderer.sprite = red_reaction;
            }
            else if (script.r2_5flg || script.range_level == 2.5f)
            {
                reaction_renderer.sprite = red_reaction;
            }
            else if (script.range_level == 2f)
            {
                reaction_renderer.sprite = red_question;
            }
            else if(script.range_level == 1.5f)
            {
                reaction_renderer.sprite = red_question;
            }
            else if (script.range_level == 1f)
            {
                reaction_renderer.sprite = yellow_question;
            }
            else
            {
                reaction_renderer.sprite = null;
            }
        }
        else
        {
            changetext.text = "";
        }
    }
}
