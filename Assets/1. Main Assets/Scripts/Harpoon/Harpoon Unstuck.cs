using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonUnstuck : MonoBehaviour
{
    HarpoonGun harpoonGun;

    private float previousDistance = 0f;
    private float currentDistance = 0f;
    private int stuckCounter = 0;               // Counts when stuck, remains at 0 if not stuck
    private int detatchTimer = 500;             // Amount of frames with no change in distance (stuck)
    private float distanceThreshold = 0.01f;    // Threshold to determine if the distance has changed

    // Initialization
    private void Awake()
    {
        // Find and assign the HarpoonGun instance in the scene
        harpoonGun = FindObjectOfType<HarpoonGun>();
    }

    public void Update()
    {
        if (harpoonGun.isGrappling)
        {
            CheckIfStuck();
        }
    }
    
    public void CheckIfStuck()
    {
        // Checks the distance between the firepoint and grapple point
        currentDistance = Vector3.Distance(harpoonGun.firePoint.position, harpoonGun.grapplePoint);
        
        // Check if the distance has changed within the set threshold
        if (Mathf.Abs(currentDistance - previousDistance) <= distanceThreshold)
        {
            // If it hasn't changed, you're stuck, count.
            stuckCounter++;
            Debug.Log("Getting Stuck");
        }
        else
        {
            // It has changed, not stuck, don't count
            stuckCounter = 0;
            Debug.Log("Not Stuck");
        }

        if (stuckCounter >= detatchTimer && currentDistance > 1f)
        {
            harpoonGun.Detatch();
            Debug.Log("UNSTUCK");
        }

        // Update distance for next frame
        previousDistance = currentDistance;
    }
}
