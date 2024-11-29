using UnityEngine;

public class DarkenSpotlightOverTime : MonoBehaviour
{
    // The spotlight light component
    private Light spotlight;

    // The target intensity to reach (the minimum brightness level)
    public float targetIntensity = 0f;

    // The time it will take to reach the target intensity (in seconds)
    public float duration = 5f;

    // A timer to track the duration
    private float timer = 0f;

    // Flag to check if the spotlight should be dimming
    private bool isDimming = false;

    void Start()
    {
        // Get the Light component on the GameObject this script is attached to
        spotlight = GetComponent<Light>();

        // Ensure the light is of type spotlight
        if (spotlight.type != LightType.Spot)
        {
            Debug.LogError("This script requires a spotlight light!");
        }
    }

    void Update()
    {
        // If the spotlight should be dimming, update the intensity
        if (isDimming)
        {
            timer += Time.deltaTime;

            // Calculate the current intensity based on the time elapsed
            float t = Mathf.Clamp01(timer / duration);
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, targetIntensity, t);

            // Check if we've reached the target intensity
            if (t >= 1f)
            {
                isDimming = false;
            }
        }
    }

    // Call this method to start dimming the spotlight
    public void StartDimming()
    {
        isDimming = true;
        timer = 0f;
    }
}
