using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    private HarpoonGun harpoonGun; // Reference to the HarpoonGun script
    public float spawnDuration = 120f;

    // Initialization
    private void Awake()
    {
        // Find and assign the HarpoonGun instance in the scene
        harpoonGun = FindObjectOfType<HarpoonGun>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Explosion"))
        // Fracture object
        FractureObject();
    }


    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject fractured;

    public void FractureObject()
    {
        // Instantiate the fractured object
        GameObject fracturedInstance = Instantiate(fractured, transform.position, transform.rotation);

        // Get all Rigidbody components in the fractured object
        Rigidbody[] rigidbodies = fracturedInstance.GetComponentsInChildren<Rigidbody>();

        // Apply a random force to each Rigidbody in the fractured object
        foreach (Rigidbody rb in rigidbodies)
        {
            // Generate a random direction limited to the X and Y axes (no Z-axis movement)
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
            float randomForce = Random.Range(5f, 15f); // Adjust force range as needed

            // Apply the force in the random direction
            rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
        }

        // Destroy the original object
        Destroy(gameObject);

        Destroy(fracturedInstance, spawnDuration);
    }


}
