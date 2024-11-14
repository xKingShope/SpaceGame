using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float smoothing = 5f; // Smoothing factor for camera movement
    public Vector3 offset; // Offset to maintain distance from the player

    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        // Get the screen width and height in world units
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize;
    }

    void Update()
    {
        // Follow the player with smoothing
        Vector3 desiredPosition = player.position + offset;

        // Smooth camera movement towards the player's position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothing * Time.deltaTime);

        // Ensure the camera doesn't move out of the screen bounds
       // ClampCameraPosition();
    }

   // private void ClampCameraPosition()
    //{
        // Calculate the camera's edge limits based on the player’s position
      //  float clampedX = Mathf.Clamp(transform.position.x, -screenWidth, screenWidth);
      //  float clampedY = Mathf.Clamp(transform.position.y, -screenHeight, screenHeight);

        // Apply clamped position to the camera
      //  transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    //}
}
