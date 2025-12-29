using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class SensorManager : MonoBehaviour
{
    private List<Sensor> sensors = new List<Sensor>();
    private List<Detectable> detectables = new List<Detectable>();
    
    public GameObject sensorPrefab;
    public GameObject detectablePrefab;

    public Text sensorText;
    public Text detectableText;

    public int maxSensors;
    public int maxDetectables;




    private void Update()
    {
        /*int[,] matrix = generateCoverageMatrix();

        string matString = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matString += matrix[i, j] + " ";
            }
            matString += "\n"; // new line per row
        }

        print(matString);*/

    }

    public void AddSensor()
    {
        if (sensors.Count >= maxSensors) return;

        GameObject sObj = Instantiate(sensorPrefab, transform.position, Quaternion.identity);
        sObj.transform.SetParent(this.transform);
        Register(sObj.GetComponent<Sensor>());
    }

    public void RemoveSensor()
    {
        if (sensors.Count == 0) return;
        Sensor toRemove = sensors[sensors.Count - 1];
        Unregister(toRemove);
        Destroy(toRemove.gameObject);
    }

    public void AddDetectable()
    {
        if (detectables.Count >= maxDetectables) return;

        GameObject sObj = Instantiate(detectablePrefab, transform.position, Quaternion.identity);
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
    public List<Sensor> GetSensors() => sensors;
    public List<Detectable> GetDetectables() => detectables;
}
