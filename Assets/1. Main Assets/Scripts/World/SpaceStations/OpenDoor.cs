using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject Doors;
    public GameObject Wall;

    // This method is called when something enters the trigger
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Disable the door when the player enters the trigger area
            Doors.SetActive(false);
            Wall.SetActive(false);
        }
    }

    // This method is called when something exits the trigger
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Enable the door when the player exits the trigger area
            Doors.SetActive(true);
            Wall.SetActive(true);
        }
    }
}
