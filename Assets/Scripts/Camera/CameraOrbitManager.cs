using Cinemachine;
using UnityEngine;

public class CameraOrbitManager : MonoBehaviour
{
    // Velocidad de zoom de la camara, limitada en un slider
    [SerializeField, Tooltip("Velocidad de zoom de la c�mara"), Range(1f, 10f)] float wheelSpeed = 5f;
    
    // C�mara que orbita
    CinemachineFreeLook cameraData;
    
    // Movimiento actual de la rueda del rat�n
    float currentMouseAxis;

    private void Start() => cameraData = GetComponent<CinemachineFreeLook>();

    private void Update()
    {
        // Cada frame lee el movimiento de la rueda del rat�n
        currentMouseAxis = Input.GetAxis("Mouse ScrollWheel") * -1;
        // Obtiene todas las orbitas de la c�mara (top, mid, bottom)
        CinemachineFreeLook.Orbit[] orbits = cameraData.m_Orbits;
        
        // Modifica el radio de todas las orbitas para mantener la relaci�n esf�rica entre todas
        for (int i = 0; i < orbits.Length; i++)     
            // Se suma el movimiento del rat�n por la velocidad al valor actual
            orbits[i].m_Radius += currentMouseAxis * wheelSpeed;        
    }
}
