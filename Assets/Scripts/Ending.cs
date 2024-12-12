using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup WinBackgroundImageCanvasGroup;
    //public AudioSource WinAudio;
    public CanvasGroup LoseBackgroundImageCanvasGroup;
    //public AudioSource LoseAudio;

    bool m_IsPlayerAtEscapePod;
    bool m_IsPlayerDriftedOff;
    float m_timer;
    //bool m_HasAudioPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtEscapePod = true;
        }
    }

    public void LostPlayer()
    {
        m_IsPlayerDriftedOff = true;
    }

    void Update()
    {
        if (m_IsPlayerAtEscapePod)
        {
            EndLevel(WinBackgroundImageCanvasGroup, false);
        }
        else if (m_IsPlayerDriftedOff)
        {
            EndLevel(LoseBackgroundImageCanvasGroup, true);
        }
    }

    void EndLevel(CanvasGroup imageCanvaseGroup, bool doRestart)
    {
       
        m_timer += Time.deltaTime;
        imageCanvaseGroup.alpha = m_timer / fadeDuration;

        if (m_timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
