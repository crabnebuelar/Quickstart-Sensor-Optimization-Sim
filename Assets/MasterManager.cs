using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    [SerializeField] private SensorManager sensorManager;
    [SerializeField] private Camera mainMenuCamera;
    [SerializeField] private Camera cam1;
    [SerializeField] private Camera cam2;
    [SerializeField] private Camera cam3;
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas settingsCanvas;

    private void Start()
    {
        sensorManager.gameObject.SetActive(false);
        mainMenuCamera.gameObject.SetActive(true);
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(false);
        cam3.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
    }

    public void ToRoom1()
    {
        cam1.gameObject.SetActive(true);
        mainMenuCamera.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);

        sensorManager.gameObject.SetActive(true);
        sensorManager.transform.position = new Vector3(12f, 0f, 142f);
    }

    public void ToRoom2()
    {
        cam2.gameObject.SetActive(true);
        mainMenuCamera.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);

        sensorManager.gameObject.SetActive(true);
        sensorManager.transform.position = new Vector3(12f, 0f, 181f);
    }

    public void ToRoom3()
    {
        cam3.gameObject.SetActive(true);
        mainMenuCamera.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);

        sensorManager.gameObject.SetActive(true);
        sensorManager.transform.position = new Vector3(12f, 0f, 223f);
    }

    public void ToMainMenu()
    {
        while (!sensorManager.SensorListIsEmpty()) { sensorManager.RemoveSensor(); }
        while (!sensorManager.DetectableListIsEmpty()) { sensorManager.RemoveDetectable(); }

        sensorManager.gameObject.SetActive(false);
        mainMenuCamera.gameObject.SetActive(true);
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(false);
        cam3.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
    }

}
