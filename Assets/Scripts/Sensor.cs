using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public LayerMask detectableLayers;
    public LayerMask obstacleLayers;
    public float maxDistance = 10f;
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
