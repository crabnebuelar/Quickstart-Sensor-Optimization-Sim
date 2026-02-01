// ============================================================================
// PenaltySlider.cs
// Controls a UI slider that updates a penalty value in the SensorManager.
// ============================================================================

using System.Collections;
using System.Collections.Generic;
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

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        SensorManager.Active?.UpdateLambdaValue(lambda, value);
        text.text = value.ToString();
    }

    public void Default()
    {
        slider.value = defaultValue;
        text.text = defaultValue.ToString();
    }
}
