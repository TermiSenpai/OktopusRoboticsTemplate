using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    Orbit,
    Free
}

public class ActiveCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook orbitCamera;
    [SerializeField] CinemachineFreeLook freeCamera;

    [SerializeField] KeyCode alternateKey;

    CinemachineFreeLook currentCamera;
    CameraState currentState;

    private void Start()
    {
        currentState = CameraState.Orbit;
        ChangeCamera(orbitCamera);
    }

    private void Update()
    {
        if (Input.GetKeyDown(alternateKey))
        {
            switch (currentState)
            {
                case CameraState.Orbit:
                    ChangeCamera(freeCamera);
                    currentState = CameraState.Free;
                    break;
                case CameraState.Free:
                    ChangeCamera(orbitCamera);
                    currentState = CameraState.Orbit;
                    break;
            }
        }
    }

    private void ChangeCamera(CinemachineFreeLook camera)
    {
        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);
        currentCamera = camera;
        currentCamera.gameObject.SetActive(true);
    }
}
