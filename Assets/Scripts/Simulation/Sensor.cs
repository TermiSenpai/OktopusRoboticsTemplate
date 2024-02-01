using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] private string PLCCode;
    [SerializeField] private float raycastDistance = 0.5f;

    private ISensorEventHandler sensorEventHandler;

    private void Start()
    {
        // Inicializar la implementaci�n de ISensorEventHandler (podr�a ser un singleton o inyecci�n desde fuera)
        sensorEventHandler = PLCControl.Instance;
    }

    // M�todo llamado en cada frame para actualizar el estado del sensor
    private void Update()
    {
        // Obtener la posici�n y la direcci�n del eje y local del sensor
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
            // El Raycast golpe� algo
            DebugDrawRay(origin, direction * hit.distance, Color.red);
            Debug.Log("Golpe�: " + hit.collider.gameObject.name);
            HandleDetectionResult(true);
        }
        else
        {
            // El Raycast no golpe� nada
            DebugDrawRay(origin, direction * raycastDistance, Color.green);
            HandleDetectionResult(false);
        }
    }

    // Act�a seg�n el resultado de la detecci�n
    private void HandleDetectionResult(bool detectionResult)
    {
        // Verificar la conexi�n al PLC antes de realizar acciones
        if (PLCConexion.plc == null || !PLCConexion.plc.IsConnected) return;

        // Activar/desactivar el PLC asociado al sensor seg�n el resultado de la detecci�n
        sensorEventHandler.OnSensorDetected(PLCCode, detectionResult);
    }

    // Dibuja el rayo de depuraci�n
    private void DebugDrawRay(Vector3 origin, Vector3 direction, Color color) => Debug.DrawRay(origin, direction, color);

}
