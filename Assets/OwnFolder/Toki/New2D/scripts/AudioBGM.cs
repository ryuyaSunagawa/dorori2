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
        ////敵の発見状態黄色
        if(GameManager.Instance.enemyState == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i != 1)
                {
                    audio_bgm[i].Stop();
                }
            
            }
            if (!audio_bgm[1].isPlaying)
            {
                audio_bgm[1].Play();
            }
        }

        ////敵の発見状態赤または攻撃中
        if(GameManager.Instance.enemyState == 3 || GameManager.Instance.enemyState == 4)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i != 3)
                {
                    audio_bgm[i].Stop();
                }
            
            }
            if (!audio_bgm[3].isPlaying)
            {
                audio_bgm[3].Play();
            }
        }

        ////敵に発見されていない通常BGM
        if (GameManager.Instance.enemyState == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i != 0)
                {
                    audio_bgm[i].Stop();
                }
                
            }
            if (!audio_bgm[0].isPlaying)
            {
                audio_bgm[0].Play();
            }
        }



    }
}
