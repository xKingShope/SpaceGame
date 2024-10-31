using UnityEngine;

public class harpoonCollision : MonoBehaviour

   {
    private HarpoonGun harpoonGun; // Reference to the HarpoonGun script

    private void Awake()
    {
        // Attempt to find the HarpoonGun in the scene
        harpoonGun = FindObjectOfType<HarpoonGun>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has a specific tag or is of a certain type
        if (collision.gameObject)
            {   
            // Notify the HarpoonGun that the projectile has collided
            if (harpoonGun != null)
            {
                harpoonGun.OnProjectileCollision();
                Debug.Log("COLLIDED");
            }

            // Make the object a child of the collided object
            transform.SetParent(collision.transform);

            // Get the Rigidbody component
            Rigidbody rb = GetComponent<Rigidbody>();

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
        }
    }
}
