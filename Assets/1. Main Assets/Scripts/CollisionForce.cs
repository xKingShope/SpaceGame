using UnityEngine;

public class CollisionForce : MonoBehaviour
{
    public float forceMultiplier = 10f; // Factor to scale the force based on speed
    public float moveForce = 5f; // Force to apply after collision
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the Rigidbody of the collided object
        Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();

        // Ensure the target has a Rigidbody
        if (targetRigidbody != null)
        {
            // Calculate the speed of the applying object
            float speed = rb.velocity.magnitude;

            // Calculate the force direction
            Vector3 forceDirection = collision.contacts[0].normal * -1; // Apply force opposite to the collision normal

            // Calculate the force to apply based on speed
            Vector3 forceToApply = forceDirection * speed * forceMultiplier;

            // Apply force to the target object
            targetRigidbody.AddForce(forceToApply, ForceMode.Impulse);
            
            // Apply a continuous force in the desired direction
            rb.AddForce(forceDirection * moveForce, ForceMode.Force);
        }
    }
}
