using Cinemachine;
using UnityEngine;

public class CameraOrbitManager : MonoBehaviour
{
    // Velocidad de zoom de la camara, limitada en un slider
    [SerializeField, Tooltip("Velocidad de zoom de la cámara"), Range(1f, 10f)] float wheelSpeed = 5f;
    
    // Cámara que orbita
    CinemachineFreeLook cameraData;
    
    // Movimiento actual de la rueda del ratón
    float currentMouseAxis;

    private void Start() => cameraData = GetComponent<CinemachineFreeLook>();

    private void Update()
    {
        // Cada frame lee el movimiento de la rueda del ratón
        currentMouseAxis = Input.GetAxis("Mouse ScrollWheel") * -1;
        // Obtiene todas las orbitas de la cámara (top, mid, bottom)
        CinemachineFreeLook.Orbit[] orbits = cameraData.m_Orbits;
        
        // Modifica el radio de todas las orbitas para mantener la relación esférica entre todas
        for (int i = 0; i < orbits.Length; i++)     
            // Se suma el movimiento del ratón por la velocidad al valor actual
            orbits[i].m_Radius += currentMouseAxis * wheelSpeed;        
    }
}
