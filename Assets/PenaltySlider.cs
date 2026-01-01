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
    [SerializeField] private int defaultValue;

    private void OnEnable()
    {
        slider.value = defaultValue;
        slider.onValueChanged.AddListener(OnSliderChanged);
        text.text = defaultValue.ToString();
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

    public void Default()
    {
        slider.value = defaultValue;
        text.text = defaultValue.ToString();
    }
}
