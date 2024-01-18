using TMPro;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtValue;

    // Se modifica la rotaci�n en el eje Y del gameobject actual a trav�s de un slider
    public void OnRotateSlider(float value)
    {
        transform.rotation = Quaternion.Euler(0, value, 0);
        txtValue.text = value.ToString("0.0");
    }
}
