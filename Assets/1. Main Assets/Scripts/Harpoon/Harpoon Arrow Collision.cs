using UnityEngine;

public class harpoonCollision : MonoBehaviour

   {
    private HarpoonGun harpoonGun; // Reference to the HarpoonGun script

    // Find the Harpoon gun
    private void Awake()
    {
        harpoonGun = FindObjectOfType<HarpoonGun>();
    }

    // Harpoon Collision
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with an object
        if (collision.gameObject)
            {
            // Notify the HarpoonGun script that the projectile has collided
            if (harpoonGun != null)
            {
                harpoonGun.OnProjectileCollision();
                Debug.Log("COLLIDED");
            }

            // Get the Rigidbody component
            Rigidbody rb = GetComponent<Rigidbody>();

            // Make rigidbody freeze
            if (rb != null)
            {
                // Freeze rotation on all axes
                rb.constraints = RigidbodyConstraints.FreezeRotation;

                // Set velocities to zero to stop any movement
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero; // Stop any rotational movement

                // Make the Rigidbody kinematic to prevent further physics interactions
                rb.isKinematic = true;
                rb.detectCollisions = false; // Disable further collision detection if desired
            }

            // Make the object a child of the collided object
            transform.SetParent(collision.transform);
            Rigidbody parentRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            // Get the Parent objects rigidbody
            harpoonGun.o_rigidbody = parentRigidbody;

            // Compare the mass of the object and player
            if (harpoonGun.o_rigidbody != null)
            {
                Debug.LogWarning("Object Mass: " + harpoonGun.o_rigidbody.mass);
                Debug.LogWarning("Player Mass: " + harpoonGun.m_rigidbody.mass);
            }

        }
    }
}
