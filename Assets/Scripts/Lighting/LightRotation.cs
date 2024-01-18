using TMPro;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtValue;

    // Se modifica la rotación en el eje Y del gameobject actual a través de un slider
    public void OnRotateSlider(float value)
    {
        transform.rotation = Quaternion.Euler(0, value, 0);
        txtValue.text = value.ToString("0.0");
    }
}
