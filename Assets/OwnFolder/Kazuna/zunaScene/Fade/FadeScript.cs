using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public float speed =0.1f;
    float al;
    float re, gr, bl;
    // Start is called before the first frame update
    void Start()
    {
        re = GetComponent<Image>().color.r;
        gr = GetComponent<Image>().color.g;
        bl = GetComponent<Image>().color.b;
        al = GetComponent<Image>().color.a;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = new Color(re, gr, bl, al);
        al -= speed;
    }
}
