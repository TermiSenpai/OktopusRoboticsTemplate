using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralLightManager : MonoBehaviour
{
    [SerializeField] private Light[] spotLights;
    [SerializeField] ReflectionProbe reflection;
    [SerializeField] private Light pointLight;
    [SerializeField] private GameObject UI;

    [Header("Spot Lights")]
    [SerializeField] Color spotLightsColor = Color.white;
    [SerializeField] float spotIntensity = 1.0f;

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

    public void OnToggleReflectionBtn()
    {
        reflection.enabled = !reflection.enabled;
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
