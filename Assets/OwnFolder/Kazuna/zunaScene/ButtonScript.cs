using System.Collections;
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
		UnityEngine.Application.Quit();
    }
    public void OnClickTitleBack()
    {
        SceneManager.LoadScene("Starting");
    }
}
