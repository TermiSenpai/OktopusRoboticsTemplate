using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILightSelector : MonoBehaviour
{
    // Opciones en el dropdown
    [SerializeField] GameObject[] lightOptions;

    // Opción seleccionada visible
    GameObject currentLightOptions;

    private void Start()
    {
        // La primera opción estará vacia
        currentLightOptions = null;

        // Nos aseguramos de que cualquier menú que nos dejemos activado, se desactive
        foreach (GameObject lightOption in lightOptions)
        {
            if (lightOption != null)
                lightOption.SetActive(false);
        }
    }

    // Cuando seleccionas una opción en el dropdown
    public void OnOptionSelected(int value)
    {
        switch (value)
        {
            case 0:
                // La opción 0 es "Ninguna"
                ChangeCurrentOptions();
                break;

            default:
                // Cuando se selecciona otra opción
                ChangeCurrentOptions(lightOptions[value]);
                break;
        }
    }
    
    // Sobrecargas

    private void ChangeCurrentOptions(GameObject selectedOption)
    {
        // Comprobación de que no modifique algo que no hay
        if (currentLightOptions != null) currentLightOptions.SetActive(false);

        // La nueva opción se almacena y se hace visible
        currentLightOptions = selectedOption;
        currentLightOptions.SetActive(true);
    }
    private void ChangeCurrentOptions()
    {
        // Comprobación para que no oculte algo que no hay
        if (currentLightOptions != null) currentLightOptions.SetActive(false);

        // Nos aseguramos de que no haya ningun menú activo
        currentLightOptions = null;
    }
}
