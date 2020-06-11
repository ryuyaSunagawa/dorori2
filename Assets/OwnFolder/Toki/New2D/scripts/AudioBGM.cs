using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBGM : MonoBehaviour
{

    private AudioSource[] audio_bgm;

    // Start is called before the first frame update
    void Start()
    {
        audio_bgm = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        BGM_Sound();

        Debug.Log(GameManager.Instance.enemyState);
    }

    private void BGM_Sound()
    {
        if(GameManager.Instance.enemyState == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                
                 audio_bgm[i].Stop();
            
            }
            audio_bgm[1].Play();
        }

        if(GameManager.Instance.enemyState == 3 || GameManager.Instance.enemyState == 4)
        {
            for (int i = 0; i < 5; i++)
            {
            
                audio_bgm[i].Stop();
            
            }
            audio_bgm[3].Play();
        }

        if (GameManager.Instance.enemyState == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                
                audio_bgm[i].Stop();
                
            }
            audio_bgm[0].Play();
        }




    }
}
