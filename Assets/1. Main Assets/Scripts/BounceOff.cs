using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOff : MonoBehaviour

{
    public float bounceForce = 10f; // Adjust this value for bounce strength

    private void OnCollisionEnter(Collision collision)
    {
        // Get the Rigidbody of the collided object
        Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();

        // Check if colliding with the kinematic object
        if (targetRigidbody != null) // Ensure you tag your kinematic object
        {
            // Get the normal of the collision surface
            Vector3 bounceDirection = collision.contacts[0].normal;

            // Apply a force in the opposite direction
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
