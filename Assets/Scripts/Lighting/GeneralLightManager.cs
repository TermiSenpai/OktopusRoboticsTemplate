using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralLightManager : MonoBehaviour
{
    [SerializeField] private Light MainSpot;
    [SerializeField] private Light backSpot;
    [SerializeField] private Light diagonalSpot;

    [SerializeField] private Light ambientLight;
    [SerializeField] private Light centralLight;

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

    public void OnToggleBtn(GameObject light)
    {
        light.SetActive(!light.activeInHierarchy);
    }

    public void OnAmbientIntensitySliderChange(float value)
    {
        ambientLight.intensity = value;
    }

    public void OnCentralIntensitySliderChange(float value)
    {
        centralLight.intensity = value;
    }

    public void OnMainSpotIntensitySliderChange(float value)
    {
        MainSpot.intensity = value;
    }

    public void OnBackIntensitySliderChange(float value)
    {
        backSpot.intensity = value;
    }

    public void OnDiagonalIntensitySliderChange(float value)
    {
        diagonalSpot.intensity = value;
    }

    private void ToggleUI()
    {
        UI.gameObject.SetActive(!UI.activeInHierarchy);
    }
}
