using UnityEngine;

public class LightRotation : MonoBehaviour
{
    // Se modifica la rotaci�n en el eje Y del gameobject actual a trav�s de un slider
    public void OnRotateSlider(float value) => transform.rotation = Quaternion.Euler(0, value, 0);
}
