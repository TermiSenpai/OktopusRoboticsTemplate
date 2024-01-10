using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralLightManager : MonoBehaviour
{
    [SerializeField] private Light MainSpot;
    [SerializeField] private Light[] spotLights;
    [SerializeField] private Light pointLight;
    [SerializeField] private GameObject UI;

    private void Awake()
    {
        UI.SetActive(false);
    }

    private void OnEnable()
    {
        MouseVisibilityManager.MouseRelease += ToggleUI;
    }

    private void OnDisable()
    {
        MouseVisibilityManager.MouseRelease -= ToggleUI;
    }

    public void OnTogglePointBtn()
    {
        pointLight.enabled = !pointLight.enabled;
    }

    public void OnToggleSpotBtn()
    {
        foreach (Light light in spotLights)
        {
            light.enabled = !light.enabled;
        }
    }

    public void OnSpotIntensitySliderChange(float value)
    {
        pointLight.intensity = value;
    }
    public void OnPointIntensitySliderChange(float value)
    {
        foreach (Light light in spotLights)
        {
            light.intensity = value;
        }
    }

    private void ToggleUI()
    {
        UI.gameObject.SetActive(!UI.activeInHierarchy);
    }
}
