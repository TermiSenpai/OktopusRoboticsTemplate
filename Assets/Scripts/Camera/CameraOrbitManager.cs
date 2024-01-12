using Cinemachine;
using UnityEngine;

public class CameraOrbitManager : MonoBehaviour
{
    [SerializeField,Tooltip("Velocidad de zoom de la c�mara"), Range(1f,10f)] float wheelSpeed = 5f;
    CinemachineFreeLook cameraData;
    float currentMouseAxis;

    private void Start()
    {
        cameraData = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        currentMouseAxis = Input.GetAxis("Mouse ScrollWheel") * -1;
        CinemachineFreeLook.Orbit[] orbits = cameraData.m_Orbits;
        for (int i = 0; i < orbits.Length; i++)
        {
            orbits[i].m_Radius += currentMouseAxis * wheelSpeed;
        }
    }
}
