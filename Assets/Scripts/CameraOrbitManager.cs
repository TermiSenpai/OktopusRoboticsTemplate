using Cinemachine;
using UnityEngine;

public class CameraOrbitManager : MonoBehaviour
{
    [SerializeField,Tooltip("Velocidad de zoom de la cámara"), Range(1f,10f)] float wheelSpeed;
    CinemachineFreeLook cameraData;
    float currentMouseAxis;

    private void Start()
    {
        cameraData = GameObject.FindGameObjectWithTag("OrbitCamera").GetComponent<CinemachineFreeLook>();
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
