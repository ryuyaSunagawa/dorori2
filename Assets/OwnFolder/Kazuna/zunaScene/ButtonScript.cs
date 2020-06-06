﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{  
    public void OnClickStart()
    {
        SceneManager.LoadScene("FixScene");
    }
    public void OnClickEnd()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        UnityEngine.Application.Quit();
    }
}
