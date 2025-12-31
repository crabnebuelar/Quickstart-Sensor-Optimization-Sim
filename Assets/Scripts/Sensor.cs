using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public LayerMask detectableLayers;
    public LayerMask obstacleLayers;
    public float maxDistance = 10f;
    public GameObject rangeIndicator;
    public LineRenderer lineRenderer;
    public int segments = 64;
    public float checkInterval = 0.1f;

    private HashSet<Transform> candidates = new HashSet<Transform>();
    private float nextCheckTime;

    private SensorManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & detectableLayers) != 0)
        {
            candidates.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        candidates.Remove(other.transform);
    }

    public void setRange(float range)
    {
        maxDistance = range;
        OnValidate();
    }

    void Awake()
    {
        PopulateInitialCandidates();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = maxDistance;

        lineRenderer.useWorldSpace = false; // So circle stays relative to sensor
        lineRenderer.loop = true; // Close the circle
        lineRenderer.positionCount = segments;
        lineRenderer.widthMultiplier = 0.1f; // Thickness of the circle
    }

    void OnEnable()
    {
        PopulateInitialCandidates();
        if (manager == null)
            manager = GetComponentInParent<SensorManager>();

        manager?.Register(this);
    }

    private void Start()
    {
        PopulateInitialCandidates();
    }

    void PopulateInitialCandidates()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            maxDistance,
            detectableLayers
        );

        foreach (Collider hit in hits)
        {
            candidates.Add(hit.transform);
        }
    }

    void OnValidate()
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        if (sc != null)
            sc.radius = maxDistance;

        float diameter = maxDistance * 2f;

        // rangeIndicator.transform.localScale = new Vector3(diameter, 0.01f, diameter);

        float angleStep = 360f / segments;
        Vector3[] points = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float x = Mathf.Cos(angle) * maxDistance;
            float z = Mathf.Sin(angle) * maxDistance;
            points[i] = new Vector3(x, 0f, z); // Flat on XZ plane
        }

        lineRenderer.SetPositions(points);
    }

    private void Update()
    {
        if (Time.time < nextCheckTime) return;
        nextCheckTime = Time.time + checkInterval;

        foreach (Transform target in candidates)
        {
            if (target == null) continue;

            /*if (HasLineOfSight(target))
            {
                //print("test");
            }*/
        }
    }

    public int CanSense(Detectable detectable)
    {
        if(candidates.Contains(detectable.transform) && HasLineOfSight(detectable.transform))
        {
            return 1;
        }
        return 0;
    }

    bool HasLineOfSight(Transform target)
    {
        Vector3 origin = transform.position;
        //Debug.DrawLine(origin, target.position, Color.black, 0.1f);
        Vector3 direction = (target.position - origin).normalized;

        float distance = Vector3.Distance(origin, target.position);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, detectableLayers | obstacleLayers))
        {
            return hit.transform == target;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Gizmos.color = Color.red;
        foreach (Transform target in candidates)
        {
            if (target != null)
                Gizmos.DrawLine(transform.position, target.position);
        }
    }

}
