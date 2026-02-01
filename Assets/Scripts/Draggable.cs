// ============================================================================
// Draggable.cs
// Manages the dragging system for sensors and detectables
// ============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour
{

    [Header("Dragging")]
    [SerializeField] private LayerMask draggableLayer;

    [Header("UI")]
    [SerializeField] private Slider rangeSlider;

    private Camera cam;

    private Transform selectedObject;
    private Transform lastSelectedObject;

    private Renderer selectedRenderer;
    private Color originalColor;

    private float fixedY;

    private Sensor selectedSensor;

    private bool paused;
    public bool canSelect = true;


    private void Start()
    {
        cam = Camera.main;

        rangeSlider.interactable = false;
        rangeSlider.onValueChanged.AddListener(SetRangeOnSelected);
    }

    private void Update()
    {
        if (!canSelect || paused)
            return;

        HandleSelectionInput();
        HandleDragging();
        HandleReleaseInput();
    }

    // Input Handling

    private void HandleSelectionInput()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (RaycastForDraggable(out RaycastHit hit))
        {
            SelectObject(hit.transform);
        }
    }

    private void HandleDragging()
    {
        if (!Input.GetMouseButton(0) || selectedObject == null || lastSelectedObject != selectedObject)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0f, fixedY, 0f));

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            selectedObject.position = new Vector3(hitPoint.x, fixedY, hitPoint.z);
        }
    }

    private void HandleReleaseInput()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        lastSelectedObject = selectedObject;
        selectedObject = null;
    }

    // Selection Logic

    private bool RaycastForDraggable(out RaycastHit hit)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 100f, draggableLayer);
    }

    private void SelectObject(Transform target)
    {
        ClearHighlight();

        selectedObject = target;
        fixedY = target.position.y;

        HighlightSelected(target);
        TrySelectSensor(target);
    }

    private void HighlightSelected(Transform target)
    {
        selectedRenderer = target.GetChild(0).GetComponent<Renderer>();
        originalColor = selectedRenderer.material.color;
        selectedRenderer.material.color = Color.yellow;
    }

    private void ClearHighlight()
    {
        if (selectedRenderer != null)
            selectedRenderer.material.color = originalColor;
    }

    // Sensor Logic

    private void TrySelectSensor(Transform target)
    {
        Sensor sensor = target.GetComponent<Sensor>();

        if (sensor == null || sensor == selectedSensor)
            return;

        SetSelectedSensor(sensor);

        rangeSlider.interactable = true;
        rangeSlider.value = selectedSensor.maxDistance;
        SetRangeOnSelected(rangeSlider.value);
    }

    private void SetRangeOnSelected(float range)
    {
        selectedSensor?.setRange(range);
    }

    public void SetSelectedSensor(Sensor sensor)
    {
        selectedSensor?.rangeIndicator.SetActive(false);
        sensor?.rangeIndicator.SetActive(true);

        selectedSensor = sensor;
    }

    public Sensor GetSelectedSensor()
    {
        return selectedSensor;
    }

    // Public Control

    public void PauseDragging()
    {
        paused = true;

        ClearHighlight();
        selectedObject = null;
        lastSelectedObject = null;

        SetSelectedSensor(null);
        rangeSlider.interactable = false;
    }

    public void ResumeDragging()
    {
        paused = false;
    }
}