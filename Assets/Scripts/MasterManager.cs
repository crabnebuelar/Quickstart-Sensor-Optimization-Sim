// ============================================================================
// MasterManager.cs
// Switches between the main menu and the simulation rooms.
// ============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public Camera camera;
    public Vector3 sensorPosition;
}

public class MasterManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SensorManager sensorManager;

    [Header("Cameras")]
    [SerializeField] private Camera mainMenuCamera;
    [SerializeField] private Room[] rooms;

    [Header("UI")]
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas settingsCanvas;

    private void Start()
    {
        GoToMainMenu();
    }

    public void GoToRoom(int roomIndex)
    {
        if (roomIndex < 0 || roomIndex >= rooms.Length)
            return;

        DisableAllCameras();

        rooms[roomIndex].camera.gameObject.SetActive(true);
        mainMenuCamera.gameObject.SetActive(false);

        mainMenuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);

        sensorManager.gameObject.SetActive(true);
        sensorManager.transform.position = rooms[roomIndex].sensorPosition;
    }

    public void GoToMainMenu()
    {
        ClearSensors();

        sensorManager.gameObject.SetActive(false);

        DisableAllCameras();
        mainMenuCamera.gameObject.SetActive(true);

        mainMenuCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
    }

    private void DisableAllCameras()
    {
        mainMenuCamera.gameObject.SetActive(false);

        foreach (var room in rooms)
            room.camera.gameObject.SetActive(false);
    }

    private void ClearSensors()
    {
        while (!sensorManager.SensorListIsEmpty())
            sensorManager.RemoveSensor();

        while (!sensorManager.DetectableListIsEmpty())
            sensorManager.RemoveDetectable();
    }
}
