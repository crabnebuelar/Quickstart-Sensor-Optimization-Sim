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
    private Color originalColor;
    private float yPosition;

    private Sensor selectedSensor;
    private bool canDrag;

    private Renderer rend;
    public Slider rangeSlider;

    private bool paused = false;

    void Start()
    {
        cam = Camera.main;
        rangeSlider.interactable = false;
        
    }

    void Update()
    {
        // Start dragging
        if (Input.GetMouseButtonDown(0) && !paused)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, draggableLayer))
            {
                if(prevSelected != null)
                {
                    rend.material.color = originalColor;
                }
                selectedObject = hit.transform;
                yPosition = selectedObject.position.y; // Fix y

                Sensor sComp = selectedObject.GetComponent<Sensor>();
                if (sComp != null && sComp != selectedSensor)
                {
                    setSelectedSensor(sComp);
                    rangeSlider.interactable = true;
                    rangeSlider.value = selectedSensor.maxDistance;
                }

                rend = selectedObject.GetChild(0).GetComponent<Renderer>();
                originalColor = rend.material.color;
                rend.material.color = Color.yellow;
            }
            else
            {
                selectedObject = prevSelected;
                canDrag = false;
            }
        }

        // Dragging
        if (Input.GetMouseButton(0) && canDrag && !paused && selectedObject != null && prevSelected == selectedObject)
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
        if (Input.GetMouseButtonUp(0) && !paused)
        {
            prevSelected = selectedObject;
            selectedObject = null;
            canDrag = true;
        }
    }

    public void pauseDragging()
    {
        paused = true;

        selectedObject = null;
        prevSelected = null;
        if (rend != null)
        {
            rend.material.color = originalColor;
        }
        setSelectedSensor(null);
    }

    public void resumeDragging()
    {
        paused = false;
        rangeSlider.interactable = false;
    }

    public void setRangeOnSelected(float range)
    {
        selectedSensor?.setRange(range);
    }
    
    public Sensor getSelectedSensor()
    {
        return selectedSensor;
    }

    public void setSelectedSensor(Sensor s)
    {
        selectedSensor?.rangeIndicator.SetActive(false);
        s?.rangeIndicator.SetActive(true);
  
        
        selectedSensor = s;
    }
}
