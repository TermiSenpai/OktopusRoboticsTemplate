using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorPicker : MonoBehaviour
{
    [SerializeField] FlexibleColorPicker fcp;

    public void updateLightColor(Light currentLight)
    {
        currentLight.color = fcp.color;
    }
}
