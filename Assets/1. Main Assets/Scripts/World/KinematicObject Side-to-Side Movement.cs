using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicPingPong : MonoBehaviour

{
    public float speed = 2f; // Speed of movement
    public float distance = 5f; // Distance to move left and right

    private Vector3 startPosition;
    //private float direction = 1f; // 1 for right, -1 for left

    void Start()
    {
        startPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        // Calculate new position
        float newX = Mathf.PingPong(Time.time * speed, distance) - (distance / 2);
        transform.position = new Vector3(startPosition.x + newX, transform.position.y, transform.position.z);
    }
}
