using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OxygenTankCollision : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public float playerOxygen;
    public GameObject oxygenLeak;
    public int spawnDuration = 5;
    public float leakCount = 2f;
    public string harpoonTag = "Harpoon";
    public string playerTag = "Player";
    public static Boolean gainOxygen = false;
    public float timer;
    public Boolean startTimer;
   

    public void OnCollisionEnter(Collision collision)
    {
        // Check if collision was by harpoon
        if (collision.gameObject.CompareTag(harpoonTag))
        {
            ContactPoint firstContact = collision.contacts[0];
            Vector3 firstCollisionPoint = firstContact.point;

            GameObject spawnedObject = Instantiate(oxygenLeak, firstCollisionPoint, transform.rotation);
 
            spawnedObject.transform.SetParent(this.transform);

            startTimer = true;
        }

        // Check if collision was with astronaut
        if (collision.gameObject.CompareTag(playerTag))
        {
            Destroy(this.gameObject);
            gainOxygen = true;
        }
        
    }

    //TODO: CORUTINE ??
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            Debug.Log("Timer Updated: " + timer);

            //if time passed > leakCount... destroy instance
            if (timer >= spawnDuration) ;
            {
                //destroy oxygen
                Destroy(this.gameObject);
                startTimer = false;
                Debug.Log("Oxygen Deleted");
            }
        }
        

    }

}
