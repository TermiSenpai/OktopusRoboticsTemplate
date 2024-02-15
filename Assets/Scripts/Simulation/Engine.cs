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

    // Receive speed from PLC
    protected void ReceiveSpeed()
    {
        try
        {
            // Attempt to read the speed value from the PLC (PLC Use uint to real values)
            var valorFloat = PlcConnectionManager.InstanceManager.ReadVariableValue<uint>(speedCode);

            // Convert the read value to float type
            float result = valorFloat.ConvertToFloat();

            // Store the converted speed value in the 'speed' variable
            speed = result;
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during reading or conversion
            Debug.LogError($"Error while reading float value: {ex.Message}");
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

        Debug.Log(rightMove);
        Debug.Log(leftMove);

        // If PLC signals a right movement, move the axis in the specified direction with a certain speed
        if (rightMove)
            MoveAxis(direction);

        // If PLC signals a left movement, move the axis in the opposite direction with the same speed
        if (leftMove)
            MoveAxis(-direction);
    }

    public abstract void MoveAxis(Vector3 vector);
    // Send the current position to PLC
    public abstract void SendCurrentPosToPLC();

    public abstract void UpdateAxisPos();
}
