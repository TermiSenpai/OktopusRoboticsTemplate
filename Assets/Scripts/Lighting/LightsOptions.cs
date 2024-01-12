using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class LightsOptions : MonoBehaviour
{
    [SerializeField] private Light modificableLight;
    [SerializeField] private TextMeshProUGUI intensityValueTxt;
    [SerializeField] private TextMeshProUGUI shadowValueTxt;
    [SerializeField] private TextMeshProUGUI rangeValueTxt;
    [SerializeField] private TextMeshProUGUI angleValueTxt;

    [SerializeField] FlexibleColorPicker fcp;

    public void OnIntensitySliderValueChange(float value)
    {
        modificableLight.intensity = value;
        ChangeIntensityValueTxt(value);
    }
    public void OnShadowSliderValueChange(float value)
    {
        modificableLight.shadowStrength = value;
        ChangeShadowValueTxt(value);
    }
    public void OnRangeSliderValueChange(float value)
    {
        modificableLight.range = value;
        ChangeRangeValueTxt(value);
    }
    public void OnAngleSliderValueChange(float value)
    {
        modificableLight.spotAngle = value;
        ChangeAngleValueTxt(value);
    }

    private void ChangeIntensityValueTxt(float value)
    {
        intensityValueTxt.text = value.ToString("0.00");
    }
    private void ChangeShadowValueTxt(float value)
    {
        shadowValueTxt.text = value.ToString("0.00");
    }
    private void ChangeRangeValueTxt(float value)
    {
        if(value>=100)
        {
            rangeValueTxt.text = value.ToString();
            return;
        }
        rangeValueTxt.text = value.ToString("0.0");
    }
    private void ChangeAngleValueTxt(float value)
    {
        angleValueTxt.text = value.ToString("0.00");
    }

    public void ToggleColorPicker(GameObject colorPicker)
    {
        colorPicker.SetActive(!colorPicker.activeInHierarchy);
    }

    public void OnToggleBtn(GameObject light)
    {
        light.SetActive(!light.activeInHierarchy);
    }

    public void updateLightColor()
    {
        modificableLight.color = fcp.color;
    }
}
