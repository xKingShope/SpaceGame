using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateExplosion : MonoBehaviour
{
    private HarpoonGun harpoonGun;
    public GameObject explosion;
    public bool StartTimer = false;
    public float spawnDuration = 1f;
    public float steamCount = 2f;
    public string harpoonTag = "Harpoon";
    private bool stopExplosion = false;
    public GameObject gasLeak;

    // Initialization
    private void Awake()
    {
        // Find and assign the HarpoonGun instance in the scene
        harpoonGun = FindObjectOfType<HarpoonGun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartTimer && !stopExplosion)
        {
            steamCount -= Time.deltaTime;
        }

        // Check if the steam count has reached 0 and stopExplosion is false
        if (steamCount <= 0 && !stopExplosion)
        {
            TriggerExplosion();
            stopExplosion = true; // Prevent further explosions
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only start the timer if the harpoon collides and explosion hasn't been triggered yet
        if (collision.gameObject.CompareTag(harpoonTag) && !stopExplosion)
        {
            StartTimer = true;
            ContactPoint firstContact = collision.contacts[0];
            Vector3 firstCollisionPoint = firstContact.point;

            GameObject spawnedEffect = Instantiate(gasLeak, firstCollisionPoint, transform.rotation);
 
            spawnedEffect.transform.SetParent(this.transform);
        }
    }

    private void TriggerExplosion()
    {
        // Ensure that the explosion only happens once
        if (!stopExplosion)
        {
            // Instantiate explosion
            GameObject spawnedObject = Instantiate(explosion, transform.position, transform.rotation);

            harpoonGun.Detatch();
            harpoonGun.ResetSpringJoint();

            // Destroy the current object (this script's GameObject)
            Destroy(gameObject);

            // Destroy the explosion after the specified duration
            Destroy(spawnedObject, spawnDuration);

            // Set stopExplosion to true to prevent any future explosions
            stopExplosion = true;
        }
    }
}


