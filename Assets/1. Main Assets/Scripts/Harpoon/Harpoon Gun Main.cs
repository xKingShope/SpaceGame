using UnityEngine;

// Harpoon Main
public class HarpoonGun : MonoBehaviour
{
    // The Rope script reference
    [Header("Scripts Ref:")]
    public HarpoonRope HarpoonRope;

    // Harpoon gun settings header
    [Header("Layers Settings:")]

    // Main Camera reference
    [Header("Main Camera:")]
    public Camera m_camera;

    // The player is the parent object (the gun holder)
    // Within the player object is the gunPivot and firePoint objects
    // These are their references
    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    // References the player objects springjoint and rigidbody components
    [Header("Physics Ref:")]
    public SpringJoint m_springJoint;
    public Rigidbody m_rigidbody;
    public Rigidbody o_rigidbody;

    // References the rope objects line renderer component
    [Header("Line Renderer:")]
    public LineRenderer lineRenderer;

    // References the current projectile (This should be a prefab)
    [Header("Projectile:")]
    public Rigidbody currentProjectile;

    // A reference to track whether the projective is active or not
    [Header("Projectile State - Dont touch")]
    public bool isProjectileActive = false;
    public bool projectileHasCollided = false;

    // The harpoon follows the mouse,
    // this dictates whether it will rotate over time or follow exactly.
    // NOTE: Rotation over time is still buggy.
    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = false;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4f;

    // The max speed variable for the ApplyGrappleForce function
    [Header("Distance Speed:")]
    [SerializeField] private float maxSpeed = 10f;
  
    // The luanch method and speed (This controls how the harpoon reels in)
    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private float launchSpeed = 1f;
    private bool isGrappling = false;

    // The grapple point reference
    [HideInInspector] public Vector3 grapplePoint;

    // Detects if the player has already grappled
    private int grappleCounter = 0;


///////////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        HarpoonRope.enabled = false;
        ResetSpringJoint();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        HandleInput();
        UpdateGrappleLine();
        UpdateGunRotation();

        if (isGrappling)
        {
            UpdateGrappleAnchor(); // Ensure connectedAnchor follows grapplePoint
        }

    }

    // Update to not allow reload when fired and missed.
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isProjectileActive)
            {
                LaunchProjectile();
            }
            else
            {
                StopGrappling();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartGrappling();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            //StopGrappling();
        }
    }

    // Handles firing the harpoon
    private void LaunchProjectile()
    {
        // If there is not a current projectile or an active rope
        if (HarpoonRope.enabled && currentProjectile != null)
        {
            // Makes the harpoon position the new grapplePoint
            // Keeps the gun rotation towrads the grapplePoint
            // Draws the rope line
            // Pracectile is active
            grapplePoint = currentProjectile.position;
            RotateGun(grapplePoint, true);
            DrawGrappleLine();
            isProjectileActive = true;
            Debug.Log("Grapple Active.");
        }
    }

    // Pulls the Grapple function and sets the isGrappling bool to true, 
    // adds debug message for current projctile state
    private void StartGrappling()
    {
        if (isProjectileActive && projectileHasCollided)
        {
            Grapple();
            isGrappling = true;
            Debug.Log("GRAPPLING");
        }
            else
        {
            Debug.Log("Cannot grapple; projectile has not collided yet.");
        }
    }


    private void StopGrappling()
    {
        isGrappling = false;
        HarpoonRope.enabled = false;
        ResetSpringJoint();
        lineRenderer.enabled = false;
        isProjectileActive = false;
        projectileHasCollided = false; // Reset the collision flag
        grappleCounter = 0;
        Debug.Log("Projectile IS NOT Active.");
    }

    private void UpdateGrappleLine()
    {
        if (isProjectileActive)
        {
            grapplePoint = currentProjectile.position;
            DrawGrappleLine();

            if (isGrappling && grappleCounter == 0)
            {
                ApplyGrappleForce();
            }
        }
    }

    // This function will continuously update the connectedAnchor to follow the grapplePoint
    private void UpdateGrappleAnchor()
    {
        if (m_springJoint != null && isGrappling)
        {
            // Update the connected anchor to the current grapple point
            m_springJoint.connectedAnchor = grapplePoint;
        }
    }

    private void ApplyGrappleForce()
    {
        if (launchToPoint)
        {
            // If the player is heavier, move object towards player
            if (m_rigidbody.mass > o_rigidbody.mass)
            {
                Vector3 direction = firePoint.position - grapplePoint;
                float distance = direction.magnitude;
                direction.Normalize();
                float stopDistance = 1f;

                if (distance > stopDistance)
                {
                    float forceMagnitude = launchSpeed * Mathf.Clamp01(distance / maxSpeed);
                    // Clamps max force applied
                    forceMagnitude = Mathf.Min(forceMagnitude, .005f);
                    // Applies the force
                    o_rigidbody.AddForce(direction * forceMagnitude, ForceMode.VelocityChange);
                }
                else
                {
                    grappleCounter = 1;
                }
            }

            // If the player is lighter, move player towards object
            else if (m_rigidbody.mass < o_rigidbody.mass)
            {
                Vector3 direction = grapplePoint - firePoint.position;
                float distance = direction.magnitude;
                direction.Normalize();
                float stopDistance = 1f;

                if (distance > stopDistance)
                {
                    float forceMagnitude = launchSpeed * Mathf.Clamp01(distance / maxSpeed);
                    // Clamps max force applied
                    forceMagnitude = Mathf.Min(forceMagnitude, .7f);
                    // Applies the force
                    m_rigidbody.AddForce(direction * forceMagnitude, ForceMode.VelocityChange);
                }
                else
                {
                    grappleCounter = 1;
                }
            }
        }

        RotateGun(grapplePoint, true);
    }


    private void UpdateGunRotation()
    {
        if (!isProjectileActive)
        {
            Vector3 mousePos = Input.mousePosition;

            // Ray from the camera to the mouse position
            Ray ray = m_camera.ScreenPointToRay(mousePos);

            // Define a plane (for example, the plane at the gun's pivot height)
            Plane plane = new Plane(Vector3.forward, gunPivot.position); // Assuming forward axis is the correct plane

            float distance; 
            if (plane.Raycast(ray, out distance))
            {
                // Get the point of intersection between the ray and the plane
                Vector3 worldMousePos = ray.GetPoint(distance);

                // Rotate gun towards the mouse position
                RotateGun(worldMousePos, true);
            }
        }
        else
        {
            // Rotate gun towards the grapple point
            RotateGun(grapplePoint, true);
        }
    }

    // References for the current projectile
    public void SetCurrentProjectile(Rigidbody projectile)
    {
        currentProjectile = projectile;
        grapplePoint = currentProjectile.position;
        HarpoonRope.enabled = true;
        isProjectileActive = true;
    }

    // Updated OnProjectileCollision method to set grapple point and connect spring joint
    public void OnProjectileCollision()
    {
        projectileHasCollided = true;
        Debug.Log("Projectile Collided.");

        // Set the grapple point to the projectile’s collision position
        grapplePoint = currentProjectile.position;
        
        // Enable the spring joint to connect to the grapple point
        m_springJoint.connectedBody = null; // Ensures it connects to a fixed point in world space
        m_springJoint.connectedAnchor = grapplePoint;

        // Activate the rope visuals
        HarpoonRope.enabled = true;
    }
    
    // Draws the line renderer between the firePoint and the grapplePoint
    void DrawGrappleLine()
    {
        lineRenderer.enabled = true;
        Vector3 startPoint = firePoint.position;
        Vector3 endPoint = grapplePoint;
        startPoint.z = 0;
        endPoint.z = 0;

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    // Same thing as DrawsGrappleLine but in reverse.. for reloading
    void DrawGrappleLineReverse()
    {
        lineRenderer.enabled = true;
        Vector3 startPoint = grapplePoint;
        Vector3 endPoint = firePoint.position;
        startPoint.z = 0;
        endPoint.z = 0;
    }


    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // Modified Grapple function
    public void Grapple()
    {
        // Transforms the spring joint's settings
        Vector3 distanceVector = firePoint.position - grapplePoint;
        m_springJoint.connectedAnchor = grapplePoint; // Connect to the grapple point directly
        m_springJoint.maxDistance = 1; // Maximum length of the "rope"
        m_springJoint.minDistance = 1f; // Prevent compression but allow flexibility
        m_springJoint.spring = 0.1f;       // Higher spring value for stronger pull
        m_springJoint.damper = 50f;        // Moderate damping for smooth movement
        m_springJoint.tolerance = .5f;  // Keep the rope taut with minimal slack
    }


    // Resets the players springJoint component so they detach from the previously grappled object
    private void ResetSpringJoint()
    {
        m_springJoint.connectedAnchor = Vector3.zero;
        m_springJoint.spring = 0;
        m_springJoint.maxDistance = 0;
        m_springJoint.minDistance = 0;
        m_springJoint.damper = 0;
    }


    // Gizmo for testing harpoon accuracy
    void OnDrawGizmos()
    {
        if (lineRenderer.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
            Gizmos.DrawSphere(grapplePoint, 0.1f);
        }
    }

}
