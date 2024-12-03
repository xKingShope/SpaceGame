// ChatGPT prompt "how to animate letters to float up and down in unity on a canvas"
// used 10/15/24

using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    public float floatSpeed = 1f; // Speed of the floating
    public float floatHeight = 5f; // Height to float up and down

    private RectTransform rectTransform;
    private Vector3 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition; // Save the original position
    }

    void Update()
    {
        // Calculate new Y position based on sine wave
        float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        rectTransform.anchoredPosition = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
