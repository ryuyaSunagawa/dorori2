﻿using System.Collections;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (script.direction)
        {
            transform.position = new Vector2(enemy.transform.position.x - 1.7f, enemy.transform.position.y + 3);
        }
        else
        {
            transform.position = new Vector2(enemy.transform.position.x + 1.7f, enemy.transform.position.y + 3);
        }

        if(!script.deathflg)
        {

            if (script.range_level == 3f)
            {
                reaction_renderer.sprite = red_reaction;//
            }
            else if (script.range_level == 2.5f)
            {
                reaction_renderer.sprite = red_reaction;//
            }
            else if (script.range_level == 2f)
            {
                reaction_renderer.sprite = red_question;//
            }
            else if(script.range_level == 1.5f)
            {
                reaction_renderer.sprite = red_question;//
            }
            else if (script.range_level == 1f || script.tomadoi)
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
            reaction_renderer.sprite = null;
        }
    }
}
