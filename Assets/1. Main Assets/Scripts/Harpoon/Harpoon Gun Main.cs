using UnityEngine;

// Harpoon Gun Main Script
public class HarpoonGun : MonoBehaviour
{
    // References the Rope Script
    [Header("Script References")]
    public HarpoonRope harpoonRope;
    public HarpoonUnstuck harpoonUnstuck;

    // The main camera
    [Header("Camera")]
    public Camera mainCamera;

    // References the player (gunholder), the gun pivot, and firepoint
    [Header("Transform References")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    // References the springJoint and the rigidbodies for the player and the object hit by the harpoon
    [Header("Physics Components")]
    public SpringJoint springJoint;
    public Rigidbody playerRigidbody;
    public Rigidbody objectRigidbody;
    public GameObject collidedObject;

    // The line renderer compenent of the Rope and harpoon arrow object
    [Header("Visuals")]
    public LineRenderer lineRenderer;
    public Rigidbody currentProjectile;

    // References the states of the arrow
    [Header("State Control")]
    public bool isProjectileActive = false;
    public bool projectileHasCollided = false;

    // Gun rotation
    [Header("Rotation Settings")]
    [SerializeField] private bool rotateOverTime = false;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4f;

    // maxSpeed is the speed being pulled by grappling, launch to point is always true, launchspeed,
    [Header("Grappling Settings")]
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private float launchSpeed = 1f;
    public bool isGrappling = false;

    // Grapple point is the harpoon arrow collision point
    [HideInInspector] public Vector3 grapplePoint;
    // If the arrow misses
    [HideInInspector] public bool missed = false;

    // Prevents multiple grappling
    public int grappleCounter = 0;
    public bool harpoonDestroy = false;

    // ---------------------------------------------------------------------------------------
    // Initialization
    private void Start()
    {
        harpoonRope.enabled = false;
        ResetSpringJoint();
        lineRenderer.enabled = false;
    }

   private void Update()
    {
        HandleInput();
        UpdateGrappleLine();
        UpdateGunRotation();

        if (isProjectileActive)
        {
            UpdateGrappleAnchor();
        }

        // Reset `harpoonDestroy` and `grappleCounter` if needed
        if (missed)
        {
            Reload();
            ResetSpringJoint();
        }
    }

    // ---------------------------------------------------------------------------------------
    // Input Handling
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isProjectileActive)
            {
                LaunchProjectile();
            }
        
            if (grappleCounter == 1)
            {
                Detatch();
                ResetSpringJoint();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && grappleCounter == 0) // prevent multiple grapples
        {
            StartGrappling();
        }
    }

    // ---------------------------------------------------------------------------------------
    // Grappling and Projectile
    private void LaunchProjectile()
    {
        if (harpoonRope.enabled && currentProjectile != null)
        {
            grapplePoint = currentProjectile.position;
            RotateGun(grapplePoint, true);
            DrawGrappleLine();
            isProjectileActive = true;
            Debug.Log("Grapple Active.");
        }
    }
    // Assigns the current projectile
    public void SetCurrentProjectile(Rigidbody projectile)
    {
        currentProjectile = projectile;
        grapplePoint = currentProjectile.position;
        harpoonRope.enabled = true;
        isProjectileActive = true;
        Debug.Log("Projectile Active.");
    }
    // If the arrow collides with and object
    public void OnProjectileCollision()
    {
        projectileHasCollided = true;
        grapplePoint = currentProjectile.position;
        springJoint.connectedBody = null;
        springJoint.connectedAnchor = grapplePoint;
        harpoonRope.enabled = true;
    }
    // Runs the Grapple function if possible
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
            Debug.Log("Projectile has not collided yet.");
        }
    }

    // Stop grappling and reload
    private void StopGrappling()
    {
        isGrappling = false;
        harpoonRope.enabled = false;
        lineRenderer.enabled = false;
        isProjectileActive = false;
        projectileHasCollided = false;
        missed = false;
        Debug.Log("Projectile IS NOT Active.");
    }

    public void Reload()
    {
        Vector3 returnDirection = firePoint.position - grapplePoint;
        float returnDistance = returnDirection.magnitude;
        returnDirection.Normalize();
        float returnForceMagnitude = launchSpeed * Mathf.Clamp01(returnDistance / maxSpeed);
        returnForceMagnitude = Mathf.Min(returnForceMagnitude, 0.05f);
        currentProjectile.AddForce(returnDirection * returnForceMagnitude, ForceMode.VelocityChange);

        if (returnDistance <= .4f)
        {
            harpoonDestroy = true;
            StopGrappling();
            Debug.Log("Reached");
        }
    }

    public void Detatch()
    {
        harpoonDestroy = true;
        StopGrappling();
        Debug.Log("Datatched");

    }


    // ---------------------------------------------------------------------------------------
    // Grapple Mechanics
    public void Grapple()
    {
        springJoint.connectedAnchor = grapplePoint;
        springJoint.maxDistance = 0.2f;
        springJoint.minDistance = 0.2f;
        springJoint.spring = 0.1f;
        springJoint.damper = 50f;
        springJoint.tolerance = 0.0f;
    }

    private void ApplyGrappleForce()
    {
        if (!launchToPoint) return;

        // Check if the object is static (has no Rigidbody)
        bool isObjectStatic = collidedObject.GetComponent<Rigidbody>() == null;

        // Determine which Rigidbody will receive the force
        Rigidbody targetRigidbody;
        Vector3 direction;

        if (isObjectStatic)
        {
            // If the target is static, apply force to the player only
            targetRigidbody = playerRigidbody;
            direction = grapplePoint - firePoint.position;

            // Move the player towards the grapple point directly
            float distance = direction.magnitude;
            direction.Normalize();
            
            grappleCounter = (distance <= 1f) ? 1 : 0;

            float forceMagnitude = launchSpeed * Mathf.Clamp01(distance / maxSpeed);
            forceMagnitude = Mathf.Min(forceMagnitude, 0.5f); // Force magnitude limited for static objects

            // Apply force to the player to pull them towards the grapple point
            targetRigidbody.AddForce(direction * forceMagnitude, ForceMode.VelocityChange);

        }
        else
        {
            // If the target has a Rigidbody, apply force based on relative mass
            targetRigidbody = (playerRigidbody.mass > objectRigidbody.mass) ? objectRigidbody : playerRigidbody;
            direction = (playerRigidbody.mass > objectRigidbody.mass) ? firePoint.position - grapplePoint : grapplePoint - firePoint.position;

            float distance = direction.magnitude;
            direction.Normalize();

            grappleCounter = (distance <= 1f) ? 1 : 0;

            float forceMagnitude = launchSpeed * Mathf.Clamp01(distance / maxSpeed);
            forceMagnitude = Mathf.Min(forceMagnitude, (playerRigidbody.mass > objectRigidbody.mass || isObjectStatic) ? 0.005f : 0.7f);

            // Apply force to the target Rigidbody
            targetRigidbody.AddForce(direction * forceMagnitude, ForceMode.VelocityChange);
        }

        RotateGun(grapplePoint, true);
    }


    private void UpdateGrappleLine()
    {
        if (isProjectileActive)
        {
            grapplePoint = currentProjectile.position;
            DrawGrappleLine();

            if (isGrappling)
            {
                // Calculate distance to the grapple point
                float distance = Vector3.Distance(playerRigidbody.position, grapplePoint);

                if (distance > 0.2f)
                {
                ApplyGrappleForce();
                }
                else
                {
                    grappleCounter = 1;
                }
            }
        }
    }

    private void UpdateGrappleAnchor()
    {
        if (projectileHasCollided)
        {
            springJoint.connectedAnchor = grapplePoint;
        }
    }

    // ---------------------------------------------------------------------------------------
    // Gun Rotation and Visuals
    private void UpdateGunRotation()
    {
        if (!isProjectileActive)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            Plane plane = new Plane(Vector3.forward, gunPivot.position);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldMousePos = ray.GetPoint(distance);
                RotateGun(worldMousePos, true);
            }
        }
        else
        {
            RotateGun(grapplePoint, true);
        }
    }

    private void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

        gunPivot.rotation = rotateOverTime && allowRotationOverTime
            ? Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * rotationSpeed)
            : Quaternion.Euler(0, 0, angle);
    }

    private void DrawGrappleLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, grapplePoint);

        // Call SetLinePoints to update the line path based on potential collisions
    }

    public void ResetSpringJoint()
    {
        springJoint.connectedAnchor = Vector3.zero;
        springJoint.spring = 0;
        springJoint.maxDistance = 0;
        springJoint.minDistance = 0;
        springJoint.damper = 0;
    }

    // ---------------------------------------------------------------------------------------
    // Debugging Tools
    private void OnDrawGizmos()
    {
        if (lineRenderer.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
            Gizmos.DrawSphere(grapplePoint, 0.1f);
        }
    }
}
