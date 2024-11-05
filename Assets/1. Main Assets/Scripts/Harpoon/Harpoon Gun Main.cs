using UnityEngine;

//Test
public class HarpoonGun : MonoBehaviour
{
    // The Rope reference
    [Header("Scripts Ref:")]
    public HarpoonRope HarpoonRope;

    // The Harpoon gun settings
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
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    // 
    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistance = 20;
  

    // The luanch method and speed (This controls how the harpoon reels in)
    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private float launchSpeed = 1f;
    private bool isGrappling = false;

    // The grapple point reference
    [HideInInspector] public Vector3 grapplePoint;

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

    // Handles firing the harpoon under certain conditions
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
        //m_rigidbody.useGravity = true;
        lineRenderer.enabled = false;
        isProjectileActive = false;
        projectileHasCollided = false; // Reset the collision flag
        Debug.Log("Projectile IS NOT Active.");
    }

    private void UpdateGrappleLine()
    {
        if (isProjectileActive)
        {
            grapplePoint = currentProjectile.position;
            DrawGrappleLine();

            if (isGrappling)
            {
                ApplyGrappleForce();
            }
        }
    }

    private void ApplyGrappleForce()
    {
        if (launchToPoint)
        {
            Vector3 direction = grapplePoint - firePoint.position;
            float distance = direction.magnitude;
            direction.Normalize();

            float forceMagnitude = launchSpeed * Mathf.Clamp01(distance / maxDistance);
            m_rigidbody.AddForce(direction * forceMagnitude, ForceMode.VelocityChange);
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


    public void SetCurrentProjectile(Rigidbody projectile)
    {
        currentProjectile = projectile;
        grapplePoint = currentProjectile.position;
        HarpoonRope.enabled = true;
        isProjectileActive = true;
    }

    // 
    public void OnProjectileCollision()
    {
        projectileHasCollided = true;
        //isProjectileActive = false;
        Debug.Log("Projectile Collided.");
    }

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

    // Grapple function
    public void Grapple()
    {
        // If there is not already a springJoint..
        if (m_springJoint == null) return;

        // Make the spring joint the grapplePoint
        m_springJoint.connectedAnchor = grapplePoint;

        // Transforms the springJoints settings
        {
            Vector3 distanceVector = firePoint.position - gunHolder.position;
            m_springJoint.maxDistance = 4f;
            m_springJoint.minDistance = 0f;
            m_springJoint.spring = 5f;
            m_springJoint.damper = 1;
        }
    }

    // Resets the players springJioint component so he detaches from the previously grappled object
    private void ResetSpringJoint()
    {
        m_springJoint.connectedAnchor = Vector3.zero;
        m_springJoint.spring = 0;
        m_springJoint.maxDistance = 0;
        m_springJoint.minDistance = 0;
        m_springJoint.damper = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

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
