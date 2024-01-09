using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    OrbitCamera,
    FreeCamera
}

public class ActiveCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook orbitCamera;
    [SerializeField] CinemachineVirtualCamera freeCamera;
    [SerializeField] CameraState starterState;
    CameraState currentState;

    [SerializeField] KeyCode alternateCameraKey;

    CinemachineVirtualCameraBase currentCamera;

    private void Start()
    {        
        EnableStarterCamera();
    }

    private void Update()
    {
        if (Input.GetKeyDown(alternateCameraKey))
        {
            CheckState();
        }
    }

    private void EnableStarterCamera()
    {
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

    private void ChangeCamera(CinemachineFreeLook camera)
    {
        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);
        currentCamera = camera;
        currentCamera.gameObject.SetActive(true);
    }
    private void ChangeCamera(CinemachineVirtualCamera camera)
    {
        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);
        currentCamera = camera;
        currentCamera.gameObject.SetActive(true);
    }
}
