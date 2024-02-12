using System.Threading.Tasks;
using Unity.VisualScripting;
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
    // Codes associated with right and left movements (editable from the Inspector)
    [Header("PLC Codes")]
    [SerializeField] private string rightCode;
    [SerializeField] private string leftCode;
    [SerializeField] private string rightInputCode;
    [SerializeField] private string leftInputCode;
    [SerializeField] private string positionCode;

    [Header("References")]
    // Servo movement speed (editable from the Inspector)
    [SerializeField] private float speed;
    // Direction of servo movement (editable from the Inspector)
    [SerializeField] private Vector3 direction;
    // Object representing the servo axis (editable from the Inspector)
    [SerializeField] private GameObject axis;
    // Axis to be limited in position (editable from the Inspector)
    [SerializeField] private AxisMovement axisToLimit;

    [Header("Debugging")]
    // Debugging options (editable from the Inspector)
    // Position limits for the servo axis (editable from the Inspector)
    [SerializeField] private float posMin;
    [SerializeField] private float posMax;

    [SerializeField] private bool debugR;
    [SerializeField] private bool debugL;

    // Private
    float axisPos;
    float lastAxisPos;
    bool isTaskActive = false;
    //bool rightInput;
    //bool leftInput;


    // M�todo llamado en cada frame para actualizar el estado del motor servo
    private void Update()
    {
        switch (PlcConnectionManager.InstanceManager.IsPLCDisconnected())
        {
            case true:
                HandleDebugMovements();
                LimitAxisPosition();
                break;
            case false:
                if (isTaskActive) return;
                isTaskActive = true;
                HandlePLCMovements();
                SendCurrentPosToPLC();
                break;
        }
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
                axisPos = axis.transform.localPosition.x;
                break;
            case AxisMovement.Y:
                clampedValue = Mathf.Clamp(currentPosition.y, posMin, posMax);
                axis.transform.localPosition = new Vector3(currentPosition.x, clampedValue, currentPosition.z);
                axisPos = axis.transform.localPosition.y;
                break;
            case AxisMovement.Z:
                clampedValue = Mathf.Clamp(currentPosition.z, posMin, posMax);
                axis.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, clampedValue);
                axisPos = axis.transform.localPosition.z;
                break;
        }
    }

    // Realizar movimientos controlados por PLC
    private async void HandlePLCMovements()
    {

        Task<bool> rightMoveTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(rightCode);
        Task<bool> leftMoveTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(leftCode);

        await Task.WhenAll(rightMoveTask, leftMoveTask);

        bool rightMove = rightMoveTask.Result;
        bool leftMove = leftMoveTask.Result;

        if (rightMove)
            MoveAxis(direction * speed);

        if (leftMove)
            MoveAxis(-direction * speed);
    }


    private void SendCurrentPosToPLC()
    {
        //Task<bool> rightInputTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(rightInputCode);
        //Task<bool> leftInputTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(leftInputCode);

        //await Task.WhenAll(rightInputTask, leftInputTask);

        //leftInput = leftInputTask.Result;
        //rightInput = leftInputTask.Result;

        //if (rightInput || leftInput)

        if (lastAxisPos != axisPos)
        {
            lastAxisPos = axisPos;
            PlcConnectionManager.InstanceManager.WriteVariableValue(positionCode, axisPos);
        }

        isTaskActive = false;

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
