﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyFrontCheck : MonoBehaviour
{
    public bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        check = true;

        if(collision.gameObject.name == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}
