using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MimicSpace
{
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 0.8f;
        public float speed = 5f;
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        Mimic myMimic;

        public Light specificLight;  // Reference to the specific spotlight to move away from
        public LayerMask lightLayer;  // Layer for light sources
        public float lightDetectionRange = 10f;  // Range to detect the light source
        public Transform target;  // Reference to the target object (the player)

        private bool isInPlayerTrigger = false;  // Flag to track if inside player trigger

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            // Optional: You can assign the target object from the Inspector in Unity
            if (target == null)
            {
                Debug.LogWarning("Target object is not assigned!");
            }
        }

        void Update()
        {
            Vector3 direction = Vector3.zero;

            // If inside the player trigger, check if we are inside the light cone before moving towards the player
            if (isInPlayerTrigger)
            {
                if (target != null)
                {
                    // If the mimic is inside the light cone, move away from the light
                    if (IsInsideSpotlightCone(specificLight))
                    {
                        direction = transform.position - specificLight.transform.position;  // Move away from the light
                        Debug.Log("Inside light cone, moving away.");
                    }
                    else
                    {
                        // Otherwise, move towards the target (player)
                        direction = target.position - transform.position;
                        Debug.Log("Inside trigger, moving towards target. Direction: " + direction);
                    }
                }
                else
                {
                    Debug.LogWarning("Target object is not assigned, cannot move towards it.");
                }
            }
            else
            {
                // Otherwise, follow the light detection or random movement
                if (specificLight != null)
                {
                    if (IsInsideSpotlightCone(specificLight))
                    {
                        // Move away from the light if inside the spotlight's cone
                        direction = transform.position - specificLight.transform.position;
                        Debug.Log("Inside light cone, moving away.");
                    }
                    else
                    {
                        // If out of range or not in the light's cone, move randomly
                        direction = new Vector3(Random.Range(-1f, 1f), 0, 0); // No Z movement
                        Debug.Log("Outside light cone, moving randomly.");
                    }
                }
                else
                {
                    // If no specific light is assigned, move randomly
                    direction = new Vector3(Random.Range(-1f, 1f), 0, 0); // No Z movement
                    Debug.Log("No specific light assigned, moving randomly.");
                }
            }

            // Debugging: Check if direction is really towards the player and not zero
            if (direction.magnitude > 0)
            {
                Debug.Log("Direction towards target is non-zero. Moving.");
            }
            else
            {
                Debug.Log("Direction is zero, not moving.");
            }

            // Normalize the direction and apply speed (no movement on Z axis)
            direction = direction.normalized * speed;

            // Ensure no movement on the Z-axis (constrain Z to 0)
            direction = new Vector3(direction.x, direction.y, 0f);

            // Apply the velocity directly to the position without smoothing (for debugging)
            transform.position += direction * Time.deltaTime;

            // Adjust height based on the ground (ensure the object is not floating)
            //RaycastHit hit;
            //Vector3 destHeight = transform.position;

            //if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out hit))
            //    destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);

            // Smoothly adjust the height
            //transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);

        }

        // Method to check if the object is inside the spotlight's cone
        bool IsInsideSpotlightCone(Light light)
        {
            if (light == null)
                return false;

            Vector3 directionToLight = transform.position - light.transform.position;
            float distanceToLight = directionToLight.magnitude;

            // If the object is outside the light's range, it's not in the cone
            if (distanceToLight > light.range)
            {
                Debug.Log("Outside light range.");
                return false;
            }

            // Normalize the direction vectors
            directionToLight.Normalize();
            Vector3 lightDirection = light.transform.forward;

            // Calculate the dot product to see if the object is inside the spotlight's cone
            float dotProduct = Vector3.Dot(directionToLight, lightDirection);

            // Calculate the angle of the cone
            float coneAngle = light.spotAngle / 2f;
            float maxDot = Mathf.Cos(coneAngle * Mathf.Deg2Rad);

            // Debug to show if the object is inside the cone
            if (dotProduct > maxDot)
            {
                Debug.DrawLine(transform.position, light.transform.position, Color.green);  // Line to show light interaction
                Debug.Log("Inside light cone.");
                return true;
            }
            else
            {
                Debug.DrawLine(transform.position, light.transform.position, Color.red);  // Line to show light interaction
                Debug.Log("Outside light cone.");
                return false;
            }
        }

        // Trigger events
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyZone"))
            {
                isInPlayerTrigger = true;
                Debug.Log("Entered trigger zone.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("EnemyZone"))
            {
                isInPlayerTrigger = false;
                Debug.Log("Exited trigger zone.");
            }
        }
    }
}
