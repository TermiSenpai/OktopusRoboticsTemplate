using S7.Net;
using System;
using System.Threading.Tasks;
using UnityEngine;

// Enumeracion que representa los ejes de movimiento posibles
public enum AxisMovement
{
    X,
    Y,
    Z
}

public abstract class Engine : MonoBehaviour
{
    #region Variables
    // Codes associated with right and left movements (editable from the Inspector)
    [Header("PLC Codes")]
    [SerializeField] protected string rightCode;
    [SerializeField] protected string leftCode;
    [SerializeField] protected string positionCode;
    [SerializeField] protected string speedCode;
    [Header("References")]
    // Servo movement speed (editable from the Inspector)
    [SerializeField] protected float speed;
    // Direction of servo movement (editable from the Inspector)
    [SerializeField] protected Vector3 direction;
    // Object representing the servo axis (editable from the Inspector)
    [SerializeField] protected GameObject objectToMove;
    // Axis to be limited in position (editable from the Inspector)
    [SerializeField] protected AxisMovement axisToLimit;

    [Header("Debugging")]
    // Debugging options (editable from the Inspector)
    // Position limits for the servo axis (editable from the Inspector)
    [SerializeField] protected float posMin;
    [SerializeField] protected float posMax;

    [SerializeField] protected bool debugR;
    [SerializeField] protected bool debugL;
    [SerializeField] protected bool speedDebugControler = false;
    [SerializeField] protected float debugSpeed = 0.1f;

    protected bool isTaskActive = false;

    #endregion

    private void Update()
    {
        // Switch statement to handle different cases based on PLC connection status
        switch (PlcConnectionManager.InstanceManager.IsPLCDisconnected())
        {
            // Case when PLC is disconnected
            case true:
                // Handle debugging movements
                HandleDebugMovements();
                // Limit the position of the servo axis
                LimitAxisPosition();
                break;
            // Case when PLC is connected
            case false:
                // Check if a task is already active, if so, return without executing further code
                if (isTaskActive) return;
                // Set the task as active
                isTaskActive = true;
                ReceiveSpeed();
                // Handle movements based on PLC instructions
                HandlePLCMovements();
                // Send the current position of the servo to the PLC
                SendCurrentPosToPLC();
                // If debug, change and send speed to plc
                if (speedDebugControler) SendDebugSpeedToPLC();
                break;
        }
        // Actualiza la posición del eje.
        UpdateAxisPos();
    }

    // Receive speed from PLC
    protected float ReceiveSpeed()
    {
        try
        {
            // Attempt to read the speed value from the PLC (PLC Use uint to real values)
            var readValue = PlcConnectionManager.InstanceManager.ReadVariableValue<uint>(speedCode);

            // Convert the read value to float type
            float result = readValue.ConvertToFloat();

            // Store the converted speed value in the 'speed' variable
            return result;
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during reading or conversion
            Debug.LogError($"Error while reading float value: {ex.Message}");
            return default;
        }
    }

    // Send speed to PLC (Only Debug)
    protected void SendDebugSpeedToPLC()
    {
        // Convert the debug speed value to UInt (unsigned integer)
        var convert = debugSpeed.ConvertToUInt();

        // Write the converted debug speed value to the PLC
        // using the PlcConnectionManager instance
        // The 'speedCode' is used as the identifier for the variable to write to
        PlcConnectionManager.InstanceManager.WriteVariableValue(speedCode, convert);
    }

    // Perform movements controlled by PLC
    protected async void HandlePLCMovements()
    {
        // Asynchronously read variables from the PLC for right and left movements
        Task<bool> rightMoveTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(rightCode);
        Task<bool> leftMoveTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(leftCode);

        // Wait for both tasks to complete
        await Task.WhenAll(rightMoveTask, leftMoveTask);

        // Get the results of the tasks
        bool rightMove = rightMoveTask.Result;
        bool leftMove = leftMoveTask.Result;

        // If PLC signals a right movement, move the axis in the specified direction with a certain speed
        if (rightMove)
            MoveAxis(direction);

        // If PLC signals a left movement, move the axis in the opposite direction with the same speed
        if (leftMove)
            MoveAxis(-direction);
    }

    protected abstract void MoveAxis(Vector3 vector);
    // Send the current position to PLC
    protected abstract void SendCurrentPosToPLC();

    protected abstract void UpdateAxisPos();

    protected abstract void MoveAxisManually(Vector3 rotationDirection);

    protected abstract void LimitAxisPosition();

    protected abstract void HandleDebugMovements();
}
