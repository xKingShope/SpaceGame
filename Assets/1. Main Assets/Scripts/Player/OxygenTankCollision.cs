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
    public int spawnDuration = 1;
    public float leakCount = 2f;
    public string harpoonTag = "Harpoon";
    public string playerTag = "Player";
    public static Boolean gainOxygen = false;
    public float timer;
    public Boolean startTimer;
    private HarpoonGun harpoonGun;
   
   private void Awake()
   {
        harpoonGun = FindObjectOfType<HarpoonGun>();
   }

    public void OnCollisionEnter(Collision collision)
    {
        // Check if collision was by harpoon
        if (collision.gameObject.CompareTag(harpoonTag))
        {
            ContactPoint firstContact = collision.contacts[0];
            Vector3 firstCollisionPoint = firstContact.point;

            GameObject spawnedObject = Instantiate(oxygenLeak, firstCollisionPoint, Quaternion.Euler(0, -90, 0));
 
            spawnedObject.transform.SetParent(this.transform);

            StartCoroutine(WaitAndDelete());
        }

        // Check if collision was with astronaut
        if (collision.gameObject.CompareTag(playerTag))
        {
            harpoonGun.Detatch();
            harpoonGun.ResetSpringJoint();
            Destroy(this.gameObject);
            gainOxygen = true;
        }
        
    }

    IEnumerator WaitAndDelete()
    {
        yield return new WaitForSeconds(6);
        harpoonGun.Detatch();
        harpoonGun.ResetSpringJoint();
        Destroy(this.gameObject);
        Debug.Log("Oxygen Deleted");
    }

}
