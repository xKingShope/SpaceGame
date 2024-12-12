using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public GameObject BloodEffect;
    public string Enemy = "Explosion";
    public Ending gameEnding;

    public void OnCollisionEnter(Collision collision)
    {
        // Check if collision was by harpoon
        if (collision.gameObject.CompareTag(Enemy))
        {
            ContactPoint firstContact = collision.contacts[0];
            Vector3 firstCollisionPoint = firstContact.point;

            GameObject spawnedObject = Instantiate(BloodEffect, firstCollisionPoint, Quaternion.Euler(0, -90, 0));

            spawnedObject.transform.SetParent(this.transform);

            gameEnding.LostPlayer();
        }
    }
}
