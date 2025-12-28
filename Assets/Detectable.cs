using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detectable : MonoBehaviour
{
    private SensorManager manager;

    void OnEnable()
    {
        if (manager == null)
            manager = GetComponentInParent<SensorManager>();

        manager?.Register(this);
    }
}
