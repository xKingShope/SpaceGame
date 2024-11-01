using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 0.05f;
    public float maxSpeed = 1f; // Maximum speed limit
    private float currentVelocity = 0f;
    private float boostSpeed = 0f;
    public float boostPower = 5f;
    private bool grab = false;
    private FixedJoint joint = null;
    private Rigidbody grabbedObject = null;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update() // Fixed the method name
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boostSpeed = boostPower; // Set boost power
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            boostSpeed = 0f; // Reset boost when the key is released
        }

        {
            // Release the object if the F key is released and an object is grabbed
            if (grab && Input.GetKeyUp(KeyCode.F))
            {
                ReleaseObject();
            }
        }
    }
    
    //Player Movement
    void FixedUpdate()
    {
        // Increase velocity over time
        currentVelocity += acceleration * Time.fixedDeltaTime;
        // Get input for horizontal (A/D) and vertical (W/S) movement
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D keys
        float verticalInput = Input.GetAxis("Vertical"); // W/S keys
        // Create a movement direction vector based on input
        Vector3 movementDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        // Apply movement force if there is any input (WASD)
        if (movementDirection.magnitude > 0)
        {
            rb.AddForce(movementDirection * (currentVelocity + boostSpeed), ForceMode.Force);
        }
        // Limit the speed
        Vector3 velocity = rb.velocity;
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed; // Clamp to max speed
            rb.velocity = velocity; // Set the new clamped velocity
        }
            // Check for releasing the object in Update to ensure responsiveness
        if (grab && Input.GetKeyUp(KeyCode.F))
        {
            FixedJoint joint = GetComponent<FixedJoint>();
            if (joint != null)
            {
                Destroy(joint);
                grab = false;
                Debug.Log("Released object");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision has a rigidbody and the F key is held down
        if (collision.rigidbody != null && Input.GetKey(KeyCode.F) && !grab)
        {
            // Add FixedJoint to grab the object
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = collision.rigidbody;
            grabbedObject = collision.rigidbody;
            grab = true;
            Debug.Log("Grabbed object");
        }
    }

    private void ReleaseObject()
    {
        if (joint != null)
        {
            // Remove the FixedJoint to release the object
            Destroy(joint);
            grabbedObject = null;
            grab = false;
            Debug.Log("Released object");
        }
    }
}
