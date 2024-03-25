using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    [SerializeField] private string PLCCode;
    [SerializeField] private float raycastDistance = 0.5f;

    [SerializeField] private UnityEvent<bool> eventHandler;
    private bool lastState = false;

    // Get the position and local Y-axis direction of the sensor
    Vector3 raycastOrigin;
    Vector3 raycastDirection;

    private void Start()
    {
        raycastOrigin = transform.position;
        raycastDirection = transform.up;
    }

    // Perform Raycast and handle the result
    private void Update() => HandleRaycast(raycastOrigin, raycastDirection);

    private void OnEnable() => PlcConnectionManager.OnPLCConnectedRelease += HandleDelegate;

    private void OnDisable() => PlcConnectionManager.OnPLCConnectedRelease -= HandleDelegate;

    // Perform Raycast and handle the result
    private void HandleRaycast(Vector3 origin, Vector3 direction)
    {
        // Perform Raycast
        if (Physics.Raycast(origin, direction, raycastDistance))
            // Raycast hit something
            HandleDetectionResult(true);
        else
            // Raycast didn't hit anything
            HandleDetectionResult(false);
    }

    // Act according to the detection result
    private void HandleDetectionResult(bool detectionResult)
    {
        if (lastState == detectionResult) return;

        lastState = detectionResult;
        // If there is an event and a call, the call will be made
        eventHandler?.Invoke(detectionResult);

        // Check PLC connection before performing actions
        if (PlcConnectionManager.InstanceManager.IsPLCDisconnected()) return;

        // Activate/deactivate the PLC associated with the sensor according to the detection result
        _ = PlcConnectionManager.InstanceManager.WriteVariableAsync(PLCCode, detectionResult);
    }

    void HandleDelegate()
    {
        lastState = !lastState;
        HandleRaycast(raycastOrigin, raycastDirection);
    }
}
