using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChg : MonoBehaviour
{
    public void PlayerDie()
    {
        SceneManager.LoadScene("GameOver");
    }
}
