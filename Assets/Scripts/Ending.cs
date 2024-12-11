using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Ending : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup WinBackgroundImageCanvasGroup;
    public CanvasGroup LoseBackgroundImageCanvasGroup;
    public CanvasGroup diedCanvasGroup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            EndLevel(WinBackgroundImageCanvasGroup, false);
        }
    }

    public void LostPlayer()
    {
        EndLevel(LoseBackgroundImageCanvasGroup, true);
    }

    public void PlayerDies()
    {
        EndLevel(diedCanvasGroup, true);
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart)
    {
        StartCoroutine(alphaEnum(imageCanvasGroup, doRestart));
    }

    IEnumerator alphaEnum(CanvasGroup  imageCanvasGroup, bool doRestart)
    {
        for (float alpha = 0; alpha < 1; alpha += .05f)
        {
            imageCanvasGroup.alpha = alpha;
            yield return new WaitForSecondsRealtime(.1f);
        }
        yield return new WaitForSecondsRealtime(2);
        if (doRestart)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            Application.Quit();
        }
    }
}
