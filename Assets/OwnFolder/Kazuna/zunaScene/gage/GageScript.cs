using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageScript : MonoBehaviour
{
    Image gage;
    float meter;
    // Start is called before the first frame update
    void Start()
    {
        gage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        meter += 0.001f;
        gage.fillAmount = meter;
        if (meter >= 1) meter = 0;
    }
}
