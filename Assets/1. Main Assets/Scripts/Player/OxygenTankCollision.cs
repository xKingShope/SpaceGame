using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTankCollision : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public float playerOxygen;
    public GameObject oxygenLeak;
    public float spawnDuration = 5f;
    public float leakCount = 2f;
    public string harpoonTag = "Harpoon";


    public void OnCollisionEnter(Collision collision)
    {
        // Check if collision was by harpoon
        if (collision.gameObject.CompareTag(harpoonTag))
        {
            ContactPoint firstContact = collision.contacts[0];
            Vector3 firstCollisionPoint = firstContact.point;

            GameObject spawnedObject = Instantiate(oxygenLeak, firstCollisionPoint, transform.rotation);

            spawnedObject.transform.SetParent(this.transform);

        }
    }   

}
