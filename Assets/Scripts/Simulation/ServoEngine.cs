using UnityEngine;

// Enumeración que representa los ejes de movimiento posibles
public enum AxisMovement
{
    X,
    Y,
    Z
}

// Clase que controla el movimiento de un motor servo en un eje específico
public class ServoEngine : MonoBehaviour
{
    // Códigos asociados a movimientos a la derecha e izquierda (editable desde el Inspector)
    [SerializeField] private string rightCode;
    [SerializeField] private string leftCode;

    // Velocidad de movimiento del servo (editable desde el Inspector)
    [SerializeField] private float speed;

    // Dirección del movimiento del servo (editable desde el Inspector)
    [SerializeField] private Vector3 direction;

    // Objeto que representa el eje del servo (editable desde el Inspector)
    [SerializeField] private GameObject axis;

    // Límites de posición para el eje del servo (editable desde el Inspector)
    [SerializeField] private float posMin;
    [SerializeField] private float posMax;

    // Eje que se limitará en posición (editable desde el Inspector)
    [SerializeField] private AxisMovement axisToLimit;

    // Opciones de depuración (editable desde el Inspector)
    [SerializeField] private bool debugR;
    [SerializeField] private bool debugL;

    // Método llamado en cada frame para actualizar el estado del motor servo
    private void Update()
    {
        // Movimiento manual del servo durante la depuración
        HandleDebugMovements();

        // Limitar la posición del eje según la configuración especificada
        LimitAxisPosition();

        // Realizar movimientos controlados por PLC
        HandlePLCMovements();
    }

    // Manejar movimientos manuales durante la depuración
    private void HandleDebugMovements()
    {
        if (debugR)
            MoveAxisManually(direction);
        if (debugL)
            MoveAxisManually(-direction);
    }

    // Limitar la posición del eje según la configuración especificada
    private void LimitAxisPosition()
    {
        Vector3 currentPosition = axis.transform.localPosition;
        float clampedValue;

        switch (axisToLimit)
        {
            case AxisMovement.X:
                clampedValue = Mathf.Clamp(currentPosition.x, posMin, posMax);
                axis.transform.localPosition = new Vector3(clampedValue, currentPosition.y, currentPosition.z);
                break;
            case AxisMovement.Y:
                clampedValue = Mathf.Clamp(currentPosition.y, posMin, posMax);
                axis.transform.localPosition = new Vector3(currentPosition.x, clampedValue, currentPosition.z);
                break;
            case AxisMovement.Z:
                clampedValue = Mathf.Clamp(currentPosition.z, posMin, posMax);
                axis.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, clampedValue);
                break;
        }
    }

    // Realizar movimientos controlados por PLC
    private void HandlePLCMovements()
    {
        if (PlcConnectionManager.InstanceManager.IsPLCDisconnected()) return;

        bool rightMove = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>(rightCode);
        if (rightMove)
            MoveAxis(direction * speed);

        bool leftMove = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>(leftCode);
        if (leftMove)
            MoveAxis(-direction * speed);
    }

    // Mover el eje del servo manualmente
    private void MoveAxisManually(Vector3 movementDirection)
    {
        axis.transform.localPosition += movementDirection * speed;
    }

    // Mover el eje del servo con velocidad especificada
    private void MoveAxis(Vector3 movement)
    {
        axis.transform.localPosition += movement;
    }
}
