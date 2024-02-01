using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] private string PLCCode;
    [SerializeField] private float raycastDistance = 0.5f;

    private ISensorEventHandler sensorEventHandler;

    private void Start()
    {
        // Inicializar la implementación de ISensorEventHandler (podría ser un singleton o inyección desde fuera)
        sensorEventHandler = PLCControl.Instance;
    }

    // Método llamado en cada frame para actualizar el estado del sensor
    private void Update()
    {
        // Obtener la posición y la dirección del eje y local del sensor
        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection = transform.up;

        // Realizar el Raycast y gestionar el resultado
        HandleRaycast(raycastOrigin, raycastDirection);
    }

    // Realiza el Raycast y gestiona el resultado
    private void HandleRaycast(Vector3 origin, Vector3 direction)
    {
        // Lanzar el Raycast
        if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance))
        {
            // El Raycast golpeó algo
            DebugDrawRay(origin, direction * hit.distance, Color.red);
            Debug.Log("Golpeó: " + hit.collider.gameObject.name);
            HandleDetectionResult(true);
        }
        else
        {
            // El Raycast no golpeó nada
            DebugDrawRay(origin, direction * raycastDistance, Color.green);
            HandleDetectionResult(false);
        }
    }

    // Actúa según el resultado de la detección
    private void HandleDetectionResult(bool detectionResult)
    {
        // Verificar la conexión al PLC antes de realizar acciones
        if (PLCConexion.plc == null || !PLCConexion.plc.IsConnected) return;

        // Activar/desactivar el PLC asociado al sensor según el resultado de la detección
        sensorEventHandler.OnSensorDetected(PLCCode, detectionResult);
    }

    // Dibuja el rayo de depuración
    private void DebugDrawRay(Vector3 origin, Vector3 direction, Color color) => Debug.DrawRay(origin, direction, color);

}
