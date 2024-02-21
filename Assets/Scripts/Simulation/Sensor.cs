using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    [SerializeField] private string PLCCode;
    [SerializeField] private float raycastDistance = 0.5f;

    [SerializeField] private UnityEvent<bool> eventHandler;
    private bool lastState = false;

    // Obtener la posicion y la direccion del eje y local del sensor
    Vector3 raycastOrigin;
    Vector3 raycastDirection;

    private void Start()
    {
        raycastOrigin = transform.position;
        raycastDirection = transform.up;
    }
    // M騁odo llamado en cada frame para actualizar el estado del sensor
    private void Update()
    {
        // Realizar el Raycast y gestionar el resultado
        HandleRaycast(raycastOrigin, raycastDirection);
    }

    // Realiza el Raycast y gestiona el resultado
    private void HandleRaycast(Vector3 origin, Vector3 direction)
    {
        // Lanzar el Raycast
        if (Physics.Raycast(origin, direction, raycastDistance))
            // El Raycast golpeo algo
            HandleDetectionResult(true);
        else
            // El Raycast no golpeo nada
            HandleDetectionResult(false);
    }

    // Actua segun el resultado de la deteccion
    private void HandleDetectionResult(bool detectionResult)
    {
        if (lastState == detectionResult) return;


        lastState = detectionResult;
        // En caso de que exista un evento y una llamada, se realizará la llamada
        eventHandler?.Invoke(detectionResult);

        // Verificar la conexion al PLC antes de realizar acciones
        if (PlcConnectionManager.InstanceManager.IsPLCDisconnected()) return;

        // Activar/desactivar el PLC asociado al sensor segun el resultado de la deteccion
        PlcConnectionManager.InstanceManager.WriteVariableAsync(PLCCode, detectionResult);
    }

}
