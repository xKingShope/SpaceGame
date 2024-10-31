using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public HarpoonRope HarpoonRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

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
    public LineRenderer lineRenderer; // Add this line

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
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequency = 1;

    [HideInInspector] public Vector3 grapplePoint;
    [HideInInspector] public Vector3 grappleDistanceVector;

    private void Start()
    {
        HarpoonRope.enabled = false;
        ResetSpringJoint(); 
        lineRenderer.enabled = false; // Disable line renderer initially
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (HarpoonRope.enabled)
            {
                RotateGun(grapplePoint, false);
                DrawGrappleLine(); // Draw the line while grappling
            }
            else
            {
                Vector3 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && HarpoonRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector3 firePointDistance = firePoint.position - gunHolder.localPosition;
                    Vector3 targetPos = grapplePoint - firePointDistance;
                    gunHolder.position = Vector3.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            HarpoonRope.enabled = false;
            ResetSpringJoint(); 
            m_rigidbody.useGravity = true; 
            lineRenderer.enabled = false; // Hide the line when not grappling
        }
        else
        {
            Vector3 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }
    }

    void DrawGrappleLine()
    {
        lineRenderer.enabled = true; // Show the line
        Vector3 startPoint = firePoint.position;
        Vector3 endPoint = grapplePoint;

        // Restrict Z to 0
        startPoint.z = 0;
        endPoint.z = 0;

        lineRenderer.SetPosition(0, startPoint); // Start point
        lineRenderer.SetPosition(1, endPoint);   // End point
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

    void SetGrapplePoint()
    {
        Vector3 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        distanceVector.z = 0; // Ensure the distance vector is also flat in the X-Y plane
        if (Physics.Raycast(firePoint.position, distanceVector.normalized, out RaycastHit hit))
        {
            if (hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                if (Vector3.Distance(hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
                {
                    // Constrain grapplePoint to X and Y axes
                    grapplePoint = new Vector3(hit.point.x, hit.point.y, 0);
                    grappleDistanceVector = grapplePoint - gunPivot.position;
                    HarpoonRope.enabled = true;
                }
            }
        }
    }

    public void Grapple()
    {
        if (m_springJoint == null) return;

        m_springJoint.connectedAnchor = grapplePoint;
        m_springJoint.spring = targetFrequency;

        if (!launchToPoint)
        {
            m_springJoint.maxDistance = autoConfigureDistance ? grappleDistanceVector.magnitude : targetDistance;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    Vector3 distanceVector = firePoint.position - gunHolder.position;
                    m_springJoint.maxDistance = distanceVector.magnitude; 
                    m_springJoint.spring = launchSpeed; 
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.useGravity = false; 
                    m_rigidbody.velocity = Vector3.zero;
                    break;
            }
        }
    }

    private void ResetSpringJoint()
    {
        m_springJoint.connectedAnchor = Vector3.zero; 
        m_springJoint.spring = 0;                       
        m_springJoint.maxDistance = 0;                  
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
