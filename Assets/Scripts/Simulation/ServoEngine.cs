using System.Threading.Tasks;
using UnityEngine;

// Enumeraci�n que representa los ejes de movimiento posibles
public enum AxisMovement
{
    X,
    Y,
    Z
}

// Clase que controla el movimiento de un motor servo en un eje espec�fico
public class ServoEngine : MonoBehaviour
{
    // C�digos asociados a movimientos a la derecha e izquierda (editable desde el Inspector)
    [SerializeField] private string rightCode;
    [SerializeField] private string leftCode;
    [SerializeField] private string rightInputCode;
    [SerializeField] private string leftInputCode;
    [SerializeField] private string positionCode;

    // Velocidad de movimiento del servo (editable desde el Inspector)
    [SerializeField] private float speed;

    // Direcci�n del movimiento del servo (editable desde el Inspector)
    [SerializeField] private Vector3 direction;

    // Objeto que representa el eje del servo (editable desde el Inspector)
    [SerializeField] private GameObject axis;

    // L�mites de posici�n para el eje del servo (editable desde el Inspector)
    [SerializeField] private float posMin;
    [SerializeField] private float posMax;

    // Eje que se limitar� en posici�n (editable desde el Inspector)
    [SerializeField] private AxisMovement axisToLimit;

    // Opciones de depuraci�n (editable desde el Inspector)
    [SerializeField] private bool debugR;
    [SerializeField] private bool debugL;

    bool derInput;
    bool leftInput;

    private bool isTaskRunning = false;

    // M�todo llamado en cada frame para actualizar el estado del motor servo
    private void Update()
    {
        HandleDebugMovements();

        if (!isTaskRunning)
        {
            isTaskRunning = true;
            Task.Run(async () => await HandlePLCMovements());
        }

        LimitAxisPosition();
    }

    // Manejar movimientos manuales durante la depuraci�n
    private void HandleDebugMovements()
    {
        if (debugR)
            MoveAxisManually(direction);
        if (debugL)
            MoveAxisManually(-direction);
    }

    // Limitar la posici�n del eje seg�n la configuraci�n especificada
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
    private async Task HandlePLCMovements()
    {
        if (PlcConnectionManager.InstanceManager.IsPLCDisconnected()) return;

        bool rightMove = await PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(rightCode);
        if (rightMove)
            MoveAxis(direction * speed);

        bool leftMove = await PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(leftCode);
        if (leftMove)
            MoveAxis(-direction * speed);

        SendCurrentPosToPLC();

        isTaskRunning = false;
    }

    private async void SendCurrentPosToPLC()
    {
        derInput = await PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(rightInputCode);
        leftInput = await PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(leftInputCode);
        if (derInput || leftInput)
            PlcConnectionManager.InstanceManager.WriteVariableValue(positionCode, axis.transform.localPosition.x);

        isTaskRunning = false;
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
