using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILightSelector : MonoBehaviour
{
    // Opciones en el dropdown
    [SerializeField] GameObject[] lightOptions;

    // Opci�n seleccionada visible
    GameObject currentLightOptions;

    private void Start()
    {
        // La primera opci�n estar� vacia
        currentLightOptions = null;

        // Nos aseguramos de que cualquier men� que nos dejemos activado, se desactive
        foreach (GameObject lightOption in lightOptions)
        {
            if (lightOption != null)
                lightOption.SetActive(false);
        }
    }

    // Cuando seleccionas una opci�n en el dropdown
    public void OnOptionSelected(int value)
    {
        switch (value)
        {
            case 0:
                // La opci�n 0 es "Ninguna"
                ChangeCurrentOptions();
                break;

            default:
                // Cuando se selecciona otra opci�n
                ChangeCurrentOptions(lightOptions[value]);
                break;
        }
    }
    
    // Sobrecargas

    private void ChangeCurrentOptions(GameObject selectedOption)
    {
        // Comprobaci�n de que no modifique algo que no hay
        if (currentLightOptions != null) currentLightOptions.SetActive(false);

        // La nueva opci�n se almacena y se hace visible
        currentLightOptions = selectedOption;
        currentLightOptions.SetActive(true);
    }
    private void ChangeCurrentOptions()
    {
        // Comprobaci�n para que no oculte algo que no hay
        if (currentLightOptions != null) currentLightOptions.SetActive(false);

        // Nos aseguramos de que no haya ningun men� activo
        currentLightOptions = null;
    }
}
