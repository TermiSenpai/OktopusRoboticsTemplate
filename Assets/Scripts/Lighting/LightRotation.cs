using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    public void OnRotateSlider(float value)
    {
        transform.rotation = Quaternion.Euler(0, value, 0);
    }
}
