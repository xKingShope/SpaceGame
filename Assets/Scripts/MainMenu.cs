using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadScene("Main Stage");   
    }

    public void QuitGame(){
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
