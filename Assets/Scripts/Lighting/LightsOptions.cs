using TMPro;
using UnityEngine;

public class LightsOptions : MonoBehaviour
{
    // referencia de la luz que se desea modificar
    [SerializeField] private Light modificableLight;

    // Texto del valor de la intensidad
    [SerializeField] private TextMeshProUGUI intensityValueTxt;

    // Texto del valor de la intensidad de sombra
    [SerializeField] private TextMeshProUGUI shadowValueTxt;

    // Texto del valor del rango de luz
    [SerializeField] private TextMeshProUGUI rangeValueTxt;

    // Texto del valor del angulo de luz
    [SerializeField] private TextMeshProUGUI angleValueTxt;

    // Referencia al selector de color 
    [SerializeField] FlexibleColorPicker fcp;


    // Al modificar el slider de intensidad
    public void OnIntensitySliderValueChange(float value)
    {
        // Se modifica la intensidad de luz
        modificableLight.intensity = value;
        // Se modifica el texto con el valor de su slider
        ChangeTxtValue(value, intensityValueTxt, "0.00");
    }

    // Al modificar el slider de intensidad de sombra
    public void OnShadowSliderValueChange(float value)
    {
        // Se modifica la fuerza de la sombra
        modificableLight.shadowStrength = value;
        // Se modifica el texto con el valor de su slider
        ChangeTxtValue(value, shadowValueTxt, "0.00");
    }

    // Al modificar el slider del rango de luz
    public void OnRangeSliderValueChange(float value)
    {
        // Se modifica el rango / distancia de luz
        modificableLight.range = value;
        // Se modifica el texto de su slider
        ChangeTxtValue(value, rangeValueTxt, "0.0");
    }

    // Al modificar el slider del angulo de luz
    public void OnAngleSliderValueChange(float value)
    {
        // Se modifica el angulo del foco
        modificableLight.spotAngle = value;
        // Se modifica el texto de su slider
        ChangeTxtValue(value, angleValueTxt, "0.0");
    }

    // Modificar el texto para que tenga el valor de su slider
    private void ChangeTxtValue(float value, TextMeshProUGUI text, string type) => text.text = value.ToString(type);

    // Alterna la activación o desactivación del gameobject deseado, usado para activar o desactivar la luz seleccionada
    public void OnToggleBtn(GameObject item) => item.SetActive(!item.activeInHierarchy);

    // Actualiza el color de la luz segun la selección del color picker
    public void UpdateLightColor() => modificableLight.color = fcp.color;

}
