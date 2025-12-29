using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour
{
    public LayerMask draggableLayer; // Assign your draggable objects layer
    private Camera cam;
    private Transform prevSelected;
    private Transform selectedObject;
    private float yPosition;

    private Sensor selectedSensor;
    private bool canDrag;

    public Renderer rend;
    public GameObject rangeSlider;

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
                selectedObject = hit.transform;
                yPosition = selectedObject.position.y; // Fix y

                if (selectedObject.GetComponent<Sensor>() != null)
                {
                    selectedSensor = selectedObject.GetComponent<Sensor>();
                    rangeSlider.GetComponent<Slider>().value = selectedSensor.maxDistance;
                }

                rend = selectedObject.GetComponent<Renderer>();
                Color originalColor = rend.material.color;
                rend.material.color = Color.yellow;
            }
            else
            {
                selectedObject = prevSelected;
                canDrag = false;
            }
        }

        // Dragging
        if (canDrag && Input.GetMouseButton(0) && selectedObject != null && prevSelected == selectedObject)
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
            prevSelected = selectedObject;
            selectedObject = null;
            canDrag = true;
        }
    }

    public void setRangeOnSelected(float range)
    {
        selectedSensor?.setRange(range);
    }
}
