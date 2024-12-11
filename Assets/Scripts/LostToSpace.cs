using UnityEngine;

public class LostToSpace : MonoBehaviour
{
    public Transform player;
    public Ending gameEnding;

    void OnTriggerEnter(Collider other)
    {
        Vector3 direction = player.position - transform.position + Vector3.up;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.transform == player)
            {
                gameEnding.LostPlayer();
            }
        }
    }
}
