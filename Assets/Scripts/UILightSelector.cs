using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILightSelector : MonoBehaviour
{
    [SerializeField] GameObject[] lightOptions;
    [SerializeField] RectTransform panel;
    GameObject currentLightOptions;
    int currentIndex;

    [SerializeField] float nullHeight;

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
                currentIndex = value;
                break;

            default:
                ChangeCurrentOptions(lightOptions[value]);
                currentIndex = value;
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
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, nullHeight);
        currentLightOptions = null;
    }


}
