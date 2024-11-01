using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectMovement : MonoBehaviour
{
    // Set rotation in degrees per second
    public float RotationSpeedDegrees;
    public float MovementSpeed;
    private float movementTimer = 0;
    Rigidbody rb;
    Vector2 direction;


    // Start is called before the first frame update
    void Start()
    {
        RotationSpeedDegrees = Random.Range(1f, 8f);
        rb = GetComponent<Rigidbody>();
        // MovementSpeed = Random.Range(0.1f, 0.3f);
        direction = Random.insideUnitCircle.normalized;

        // MOVEMENT
        if (movementTimer == 0)
        {
            rb.velocity = MovementSpeed * direction;
            movementTimer += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        // ROTATION
        // Asign the rototion degrees per second
        float rotationAmount = RotationSpeedDegrees * Time.deltaTime;
        // Set rotation to the Z axis
        float currentRotation = transform.localRotation.eulerAngles.z;
        // Apply the rotation
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, currentRotation + rotationAmount));


    }
}
