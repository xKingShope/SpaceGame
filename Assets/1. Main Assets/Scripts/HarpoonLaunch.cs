using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchHarpoon : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public float velocity = 100f;
    private float distanceToDecelerate = 4f; // Distance at which to start losing velocity
    private float decelerationRate = 30f; // Rate at which the harpoon loses velocity

    // Reference to the HarpoonGun script
    public HarpoonGun harpoonGun;
    private GameObject currentHarpoon; // Track the current harpoon

    void Update()
    {
        // Check for firing or deleting the projectile
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentHarpoon == null) // If there is no active harpoon
            {
                // Launch a new harpoon
                currentHarpoon = Instantiate(projectilePrefab, transform.position, transform.rotation);
                Rigidbody rb = currentHarpoon.GetComponent<Rigidbody>();
                rb.AddRelativeForce(new Vector3(velocity, 0, 0));

                // Inform HarpoonGun about the new projectile
                if (harpoonGun != null)
                {
                    harpoonGun.SetCurrentProjectile(rb);
                }

                // Start the coroutine to handle deceleration
                StartCoroutine(DecelerateHarpoon(rb));
            }
            else // If there is an active harpoon, destroy it
            {   
                Destroy(currentHarpoon);
                currentHarpoon = null; // Reset the reference
            }
        }
    }

    private IEnumerator DecelerateHarpoon(Rigidbody rb)
    {
        Vector3 initialPosition = rb.position;

        while (true)
        {
            // Calculate the distance traveled
            float distanceTraveled = Vector3.Distance(initialPosition, rb.position);

            // If the distance traveled exceeds the deceleration distance, start reducing velocity
            if (distanceTraveled >= distanceToDecelerate)
            {
                // Reduce the velocity
                rb.velocity -= rb.velocity.normalized * decelerationRate * Time.deltaTime;

                // If the harpoon has stopped moving, exit the loop
                if (rb.velocity.magnitude < 0.1f)
                {
                    rb.velocity = Vector3.zero; // Stop the harpoon
                    break;
                }
            }

            yield return null; // Wait for the next frame
        }
    }
}
