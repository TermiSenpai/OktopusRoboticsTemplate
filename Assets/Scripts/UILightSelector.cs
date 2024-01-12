using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILightSelector : MonoBehaviour
{
    [SerializeField] GameObject[] lightOptions;
    GameObject currentLightOptions;

    private void Start()
    {
        currentLightOptions = null;
        foreach (GameObject lightOption in lightOptions)
        {
            if (lightOption != null)
                lightOption.SetActive(false);
        }
    }

    public void OnOptionSelected(int value)
    {
        switch (value)
        {
            case 0:
                ChangeCurrentOptions();
                break;

            default:
                ChangeCurrentOptions(lightOptions[value]);
                break;
        }
    }

    private void ChangeCurrentOptions(GameObject selectedOption)
    {
        if (currentLightOptions != null) currentLightOptions.SetActive(false);

        currentLightOptions = selectedOption;
        currentLightOptions.SetActive(true);
    }
    private void ChangeCurrentOptions()
    {
        if (currentLightOptions != null) currentLightOptions.SetActive(false);
        currentLightOptions.SetActive(false);
        currentLightOptions = null;
    }


}
