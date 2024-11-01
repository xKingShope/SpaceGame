using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private Vector3 moveAmount;
    private bool facingRight = true;
    public float rotationSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveAmount = new Vector3(horizontalInput, 0, verticalInput).normalized * speed;

        // Find mouse position and transform it into the world position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane; // Set Z depth for mouse position
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        
        // Calculate the angle to the mouse
        Vector3 directionToMouse = mousePos - transform.position;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;


        
        // Flip based on the mouse position
        if (directionToMouse.x < 0 && facingRight)
        {
            Flip();
        }
        else if (directionToMouse.x > 0 && !facingRight)
        {
            Flip();
        }

        // Rotate towards the mouse
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle -180f)); // Adjust based on initial sprite orientation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void FixedUpdate()
    {
        // Move the player
        Vector3 newPosition = rb.position + moveAmount * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    void Flip()
    {
        // Flip the player
        facingRight = !facingRight;
        Vector3 tmpScale = transform.localScale;
        tmpScale.x = facingRight ? Mathf.Abs(tmpScale.x) : -Mathf.Abs(tmpScale.x);
        transform.localScale = tmpScale;
    }
}
