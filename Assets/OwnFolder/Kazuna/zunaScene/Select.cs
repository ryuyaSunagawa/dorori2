using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;

    GameObject selectObj;

    // Start is called before the first frame update
    void Start()
    {
        Selectable sel = GetComponent<Selectable>();
        sel.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            selectObj = eventSystem.currentSelectedGameObject.gameObject;
            if (selectObj.name == "StartButton")
            {
                ButtonScript s = GetComponent<ButtonScript>();
                s.OnClickStart();
            }
            if (selectObj.name == "EndButton")
            {
                ButtonScript s = GetComponent<ButtonScript>();
                s.OnClickEnd();
            }
        }
    }
}
