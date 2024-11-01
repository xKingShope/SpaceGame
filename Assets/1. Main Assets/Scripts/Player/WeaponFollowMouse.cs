using UnityEngine;

public class WeaponFollowMouse : MonoBehaviour
{
    public float angleMin = -35f; // Minimum angle (in degrees) the object can rotate
    public float angleMax = 35f;  // Maximum angle (in degrees) the object can rotate
    public float offset = 1f;

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0; // Ensure we are on the same plane

        // Calculate the direction vector from the object to the mouse
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        // Calculate the angle between the object's forward vector and the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Clamp the angle
        angle = Mathf.Clamp(angle, angleMin, angleMax);

        // Rotate the object to the clamped angle
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
