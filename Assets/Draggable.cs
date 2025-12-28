using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public LayerMask draggableLayer; // Assign your draggable objects layer
    private Camera cam;
    private Transform selectedObject;
    private float yPosition;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Start dragging
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, draggableLayer))
            {
                selectedObject = hit.transform.parent;
                yPosition = selectedObject.position.y; // Fix y
            }
        }

        // Dragging
        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, yPosition, 0));
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                selectedObject.position = new Vector3(hitPoint.x, yPosition, hitPoint.z);
            }
        }

        // Stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;
        }
    }
}
