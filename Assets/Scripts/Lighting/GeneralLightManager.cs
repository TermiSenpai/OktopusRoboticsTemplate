using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralLightManager : MonoBehaviour
{
    [SerializeField] private Light[] spotLights;
    [SerializeField] ReflectionProbe reflection;
    [SerializeField] private Light pointLight;

    [Header("Spot Lights")]
    [SerializeField] Color spotLightsColor = Color.white;
    [SerializeField] float spotIntensity = 1.0f;

    private void Update()
    {

        //TODO 
        // Crear evento on modified
        // Seguramente hacer con GUI
        foreach (Light light in spotLights)
        {
            light.color = spotLightsColor;
            light.intensity = spotIntensity;
            if (spotIntensity < 0) spotIntensity = 0;
        }
    }
}
