using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SensorManager : MonoBehaviour
{
    public static SensorManager Active;
    
    private List<Sensor> sensors = new List<Sensor>();
    private List<Detectable> detectables = new List<Detectable>();
    public Draggable draggable;
    
    public GameObject sensorPrefab;
    public GameObject detectablePrefab;

    public Text sensorText;
    public Text detectableText;

    public int maxSensors;
    public int maxDetectables;

    [SerializeField] Button optimizeButton;
    [SerializeField] Text optimizeText;
    [SerializeField] CanvasGroup panel;

    private int[] lambdas = { 5, 5, 10 };


    private void OnEnable()
    {
        Active = this;
    }

    private void OnDisable()
    {
        if (Active == this) Active = null;
    }

    private void Start()
    {
        optimizeButton.onClick.AddListener(Optimize);
    }

    public void OnSliderChanged(int lambda, float value)
    {
        lambdas[lambda] = (int) value;
    }

    public void AddSensor()
    {
        if (sensors.Count >= maxSensors) return;

        Vector3 spawnPos = transform.position;
        spawnPos.y += 3f;
        GameObject sObj = Instantiate(sensorPrefab, spawnPos, Quaternion.identity);
        sObj.transform.SetParent(this.transform);
        Register(sObj.GetComponent<Sensor>());
    }

    public void RemoveSensor()
    {
        if (sensors.Count == 0) return;
        Sensor toRemove = sensors[sensors.Count - 1];
        Unregister(toRemove);
        if(toRemove == draggable.getSelectedSensor())
        {
            draggable.setSelectedSensor(null);
        }
        Destroy(toRemove.gameObject);
    }

    public void AddDetectable()
    {
        if (detectables.Count >= maxDetectables) return;

        Vector3 spawnPos = transform.position;
        spawnPos.x += 25f;
        GameObject sObj = Instantiate(detectablePrefab, spawnPos, Quaternion.identity);
        sObj.transform.SetParent(this.transform);
        Register(sObj.GetComponent<Detectable>());
    }

    public void RemoveDetectable()
    {
        if (detectables.Count == 0) return;
        Detectable toRemove = detectables[detectables.Count - 1];
        Unregister(toRemove);
        Destroy(toRemove.gameObject);
    }

    public void Register(Sensor sensor)
    {
        if (!sensors.Contains(sensor)) sensors.Add(sensor);
        sensorText.text = sensors.Count.ToString();
    }

    public void Unregister(Sensor sensor)
    {
        sensors.Remove(sensor);
        sensorText.text = sensors.Count.ToString();
    }

    public void Register(Detectable detectable)
    {
        if (!detectables.Contains(detectable)) detectables.Add(detectable);
        detectableText.text = detectables.Count.ToString();
    }

    public void Unregister(Detectable detectable)
    {
        detectables.Remove(detectable);
        detectableText.text = detectables.Count.ToString();
    }

    public int[,] generateCoverageMatrix()
    {
        int[,] matrix = new int[sensors.Count, detectables.Count];
        for (int i = 0; i < sensors.Count; i++) 
        {
            for(int j = 0; j < detectables.Count; j++) 
            {
                matrix[i, j] = sensors[i].CanSense(detectables[j]);
            }
        }
        return matrix;
    }

    private string SerializeMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        List<string> lines = new List<string>();

        for (int i = 0; i < rows; i++)
        {
            int[] row = new int[cols];
            for (int j = 0; j < cols; j++)
                row[j] = matrix[i, j];

            lines.Add(string.Join(",", row));
        }

        return string.Join(";", lines);
    }

    public async void Optimize()
    {   
        draggable.pauseDragging();
        draggable.canSelect = false;

        SetUIInteractable(false);
        optimizeButton.interactable = false;
        optimizeText.text = "Optimizing...";

        string pythonPath = @"C:\Users\Crabnebuelar\miniconda3\python.exe";
        string scriptPath = @"Assets/Scripts/sensor_optimizer.py";
        string outputPath = @"C:\Users\Crabnebuelar\Documents\GitHub\Quickstart-Sensor-Optimization-Sim\solution.json";

        int[,] coverageMatrix = generateCoverageMatrix();
        string json = SerializeMatrix(coverageMatrix);
        print(json);

        await Task.Run(() =>
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"\"{scriptPath}\" \"{json}\" \"{outputPath}\" \"{lambdas[0]}\" \"{lambdas[1]}\" \"{lambdas[2]}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process p = Process.Start(psi))
            {
                p.WaitForExit();
                string output = p.StandardOutput.ReadToEnd();
                string errors = p.StandardError.ReadToEnd();

                print("PYTHON OUTPUT:\n" + output);
                print("PYTHON ERRORS:\n" + errors);
            }
        });


        string resultJson = File.ReadAllText("solution.json");

        SensorData data = JsonUtility.FromJson<SensorData>(resultJson);

        // Access values
        foreach (var kv in data.selected_sensors)
        {
            print(kv.key + " = " + kv.value);
        }

        for (int i = 0; i < data.selected_sensors.Count; i++)
        {
            if (data.selected_sensors[i].value == 0)
            {
                sensors[i].gameObject.SetActive(false);
            }
        }

        draggable.canSelect = true;

        optimizeButton.interactable = true;
        optimizeButton.onClick.RemoveAllListeners();
        optimizeButton.onClick.AddListener(Reset);

        optimizeText.text = "Reset";
    }

    public void Reset()
    {
        foreach (var sensor in sensors)
        {
            sensor.gameObject.SetActive(true);
        }

        optimizeButton.onClick.RemoveAllListeners();
        optimizeButton.onClick.AddListener(Optimize);

        SetUIInteractable(true);
        draggable.resumeDragging();

        optimizeText.text = "Optimize";

    }

    private void SetUIInteractable(bool enabled)
    {
        panel.interactable = enabled;
    }


    [Serializable]
    public class SensorKeyValue
    {
        public string key;
        public float value;
    }

    [Serializable]
    public class SensorData
    {
        public List<SensorKeyValue> selected_sensors;
    }

    public List<Sensor> GetSensors() => sensors;
    public List<Detectable> GetDetectables() => detectables;
}
