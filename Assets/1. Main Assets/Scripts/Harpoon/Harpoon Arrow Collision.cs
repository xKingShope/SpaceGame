using UnityEngine;

public class HarpoonCollision : MonoBehaviour
{
    private HarpoonGun harpoonGun; // Reference to the HarpoonGun script

    // ---------------------------------------------------------------------------------------
    // Initialization
    private void Awake()
    {
        // Find and assign the HarpoonGun instance in the scene
        harpoonGun = FindObjectOfType<HarpoonGun>();
    }

    // ---------------------------------------------------------------------------------------
    // Collision Handling
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
        {
            HandleProjectileCollision();
            FreezeRigidbody();
            AttachToCollidedObject(collision.transform);
            UpdateObjectRigidbody(collision);
        }
    }

    // ---------------------------------------------------------------------------------------
    // Helper Methods
    private void HandleProjectileCollision()
    {
        if (harpoonGun != null)
        {
            harpoonGun.OnProjectileCollision();
            Debug.Log("COLLIDED");
        }
    }

    private void FreezeRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }

    private void AttachToCollidedObject(Transform collidedTransform)
    {
        transform.SetParent(collidedTransform);
    }

    private void UpdateObjectRigidbody(Collision collision)
    {
        Rigidbody parentRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        harpoonGun.objectRigidbody = parentRigidbody;
        harpoonGun.collidedObject = collision.gameObject;

        if (parentRigidbody != null)
        {
            Debug.LogWarning("Object Mass: " + parentRigidbody.mass);
            Debug.LogWarning("Player Mass: " + harpoonGun.playerRigidbody.mass);
        }
    }
}
