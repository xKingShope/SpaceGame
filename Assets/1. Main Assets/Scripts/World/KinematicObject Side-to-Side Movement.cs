using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicPingPong : MonoBehaviour

{
    public float speed = 100f; // Speed of movement
    public float distance = 300f; // Distance to move to the right

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        // Calculate the target position to the right
        float targetX = startPosition.x + distance;

        // Move the object to the right at the specified speed
        transform.position += Vector3.right * speed * Time.deltaTime;

        // Stop moving once the object reaches the target position
        if (transform.position.x >= targetX)
        {
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            enabled = false; // Disable the Update method to stop further movement
        }
    }

}
