/*

using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public HarpoonRope HarpoonRope;

    [Header("Layers Settings:")]
    //[SerializeField] private bool grappleToAll = false;
    //[SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint m_springJoint;
    public Rigidbody m_rigidbody;

    [Header("Line Renderer:")]
    public LineRenderer lineRenderer;

    [Header("Projectile:")]
    public Rigidbody currentProjectile;

    [Header("Projectile State")]
    public bool isProjectileActive = false;
    public bool projectileHasCollided = false;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistance = 20;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    //[SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;
    private bool isGrappling = false;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequency = 1;

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

    private void LaunchProjectile()
    {
        if (HarpoonRope.enabled && currentProjectile != null)
        {
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
            mousePos.z = 0; // Adjust this if necessary
            Vector3 worldMousePos = m_camera.ScreenToWorldPoint(mousePos);
            RotateGun(worldMousePos, true);
        }
        else
        {
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
            gunPivot.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    public void Grapple()
    {
        if (m_springJoint == null) return;

        m_springJoint.connectedAnchor = grapplePoint;
        m_springJoint.spring = targetFrequency;

        if (!launchToPoint)
        {
            m_springJoint.maxDistance = autoConfigureDistance ? grapplePoint.magnitude : targetDistance;
        }
        else
        {
            Vector3 distanceVector = firePoint.position - gunHolder.position;
            m_springJoint.maxDistance = 4f;
            m_springJoint.minDistance = 0f;
            m_springJoint.spring = 10f;
            m_springJoint.damper = 20;
        }
    }

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



*/
