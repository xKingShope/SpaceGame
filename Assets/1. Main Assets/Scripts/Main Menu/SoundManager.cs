using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip mainButton, otherButton;
    static AudioSource audioSource;
    void Start()
    {
        mainButton = Resources.Load<AudioClip>("Button 3");
        otherButton = Resources.Load<AudioClip>("Button 2");
        audioSource = GetComponent<AudioSource>();
    }
    public static void playMainSound(){
        audioSource.PlayOneShot(mainButton);
    }
    public static void playOtherSound(){
        audioSource.PlayOneShot(otherButton);
    }
}
