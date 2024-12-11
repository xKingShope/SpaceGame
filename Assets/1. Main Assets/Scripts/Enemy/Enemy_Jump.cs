using UnityEngine;

public class Enemy_Jump : MonoBehaviour
{
    private HarpoonGun harpoonGun;
    private Collider jump_Trigger;
    private Rigidbody Rb;
    private Ending gameEnding;

    public int jumpForce;

    private void Awake()
    {
        jump_Trigger = gameObject.GetComponentInChildren<CapsuleCollider>();
        Rb = gameObject.GetComponent<Rigidbody>();
        harpoonGun = FindObjectOfType<HarpoonGun>();
        gameEnding = FindFirstObjectByType<Ending>();
    }

    private void Start()
    {
        Rb.AddTorque(0, 0, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Harpoon"))
        {
            harpoonGun.Detatch();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            gameEnding.PlayerDies();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rb.AddForce(gameObject.transform.up * jumpForce);
        }
    }
}