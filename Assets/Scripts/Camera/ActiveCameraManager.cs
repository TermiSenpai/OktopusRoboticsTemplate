using Cinemachine;
using UnityEngine;

// Estado de las camaras
public enum CameraState
{
    OrbitCamera,
    FreeCamera
}

public class ActiveCameraManager : MonoBehaviour
{
    // Camara que orbita 
    [SerializeField] CinemachineFreeLook orbitCamera;
    // Camara libre
    [SerializeField] CinemachineVirtualCamera freeCamera;
    // Estado de inicio
    [SerializeField] CameraState starterState;
    // Estado actual
    CameraState currentState;
    // Tecla para cambiar de camara
    [SerializeField] KeyCode alternateCameraKey;
    // Al usar diferentes tipos de cámara, se almacena en su clase padre
    CinemachineVirtualCameraBase currentCamera;

    // Inicia con la cámara seleccionada en el inspector
    private void Start() => EnableStarterCamera();

    private void Update()
    {
        // Cada frame espera a que se pulse la tecla para cambiar de camara
        if (Input.GetKeyDown(alternateCameraKey))
            CheckState();
    }

    // Camara de inicio seleccionable en el inspector
    private void EnableStarterCamera()
    {
        // Asigna los estados base para iniciar con la camara seleccionada
        switch (starterState)
        {
            case CameraState.OrbitCamera:
                ChangeCamera(orbitCamera);
                currentState = CameraState.OrbitCamera;
                break;
            case CameraState.FreeCamera:
                ChangeCamera(freeCamera);
                currentState = CameraState.FreeCamera;
                break;
        }
    }

    private void CheckState()
    {
        // Alterna las camaras, en base a la camara que se este usando en el momento
        switch (currentState)
        {
            case CameraState.OrbitCamera:
                ChangeCamera(freeCamera);
                currentState = CameraState.FreeCamera;
                break;
            case CameraState.FreeCamera:
                ChangeCamera(orbitCamera);
                currentState = CameraState.OrbitCamera;
                break;
        }
    }

    // Se cambia la camara, desactivando la actual, asignando la nueva y activandola
    private void ChangeCamera(CinemachineVirtualCameraBase camera)
    {
        // Se comprueba que la camara que se intenta desactivar exista
        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);

        // Se asigna la nueva camara
        currentCamera = camera;
        // Se activa para que comience la transición
        currentCamera.gameObject.SetActive(true);
    }
}
