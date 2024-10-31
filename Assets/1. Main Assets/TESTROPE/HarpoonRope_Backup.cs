/*

using UnityEngine;

public class HarpoonRope : MonoBehaviour
{
    [Header("General References:")]
    public HarpoonGun HarpoonGun;
    public LineRenderer m_lineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int precision = 40; // Fixed spelling mistake
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    private float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1;

    private float moveTime = 0;

    [HideInInspector] public bool isGrappling = true;

    private bool straightLine = true; // Fixed spelling mistake

    private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.positionCount = precision; // Fixed spelling mistake
        waveSize = StartWaveSize;
        straightLine = false;

        LinePointsToFirePoint();
        m_lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }

    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < precision; i++)
        {
            m_lineRenderer.SetPosition(i, HarpoonGun.firePoint.position);
        }
    }

    private void Update()
    {
        if (HarpoonGun.isProjectileActive) // Check if the projectile is active
        {
            moveTime += Time.deltaTime;
            DrawRope();
        }
    }

    void DrawRope()
    {
        if (!straightLine)
        {
            if (Vector3.Distance(m_lineRenderer.GetPosition(precision - 1), HarpoonGun.grapplePoint) < 0.1f)
            {
                straightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling)
            {
                HarpoonGun.Grapple();
                isGrappling = true;
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;
                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }
                DrawRopeNoWaves();
            }
        }
    }

    void DrawRopeWaves()
    {
        for (int i = 0; i < precision; i++)
        {
            float delta = (float)i / ((float)precision - 1f);
            Vector2 offset = Vector2.Perpendicular(HarpoonGun.grapplePoint - HarpoonGun.firePoint.position).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(HarpoonGun.firePoint.position, HarpoonGun.grapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(HarpoonGun.firePoint.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            m_lineRenderer.SetPosition(i, currentPosition);
        }
    }

    void DrawRopeNoWaves()
    {
        m_lineRenderer.SetPosition(0, HarpoonGun.firePoint.position);
        m_lineRenderer.SetPosition(1, HarpoonGun.grapplePoint);
    }
}


*/