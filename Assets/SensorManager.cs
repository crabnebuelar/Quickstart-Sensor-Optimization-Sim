using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    private List<Sensor> sensors = new List<Sensor>();
    private List<Detectable> detectables = new List<Detectable>();


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

    public void Register(Sensor sensor)
    {
        if (!sensors.Contains(sensor)) sensors.Add(sensor);
    }

    public void Unregister(Sensor sensor)
    {
        sensors.Remove(sensor);
    }

    public void Register(Detectable detectable)
    {
        if (!detectables.Contains(detectable)) detectables.Add(detectable);
    }

    public void Unregister(Detectable detectable)
    {
        detectables.Remove(detectable);
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
