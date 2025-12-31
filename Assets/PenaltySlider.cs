using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PenaltySlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text text;
    [SerializeField] private int lambda;

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        SensorManager.Active?.OnSliderChanged(lambda, value);
        text.text = value.ToString();
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnSliderChanged);
    }
}
