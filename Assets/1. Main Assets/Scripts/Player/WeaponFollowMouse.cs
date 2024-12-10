using UnityEngine;

public class WeaponFollowMouse : MonoBehaviour
{
    public Camera Camera;
    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 7;
        mousePosition = Camera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0; // Ensure we are on the same plane
        
        transform.position = mousePosition;
        Debug.Log(mousePosition);
    }
}
