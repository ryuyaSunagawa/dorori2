using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    public EnemyReflection enemyreflection;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyreflection.reflection)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            enemyreflection.reflection = false;
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
