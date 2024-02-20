using S7.Net;
using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

    bool rightMove;
    bool leftMove;
    uint speedValue;
    uint lastSpeedValue;

    float waitSecs = 0.1f;

    Coroutine currentCoroutine;

    protected bool isTaskActive = false;

    #endregion
    // TODO
    // Modificar el update para que lea todas las variables de forma asíncrona y con su respectivo valor, realizar modificaciones
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
                // Trigger asynchronous reading of PLC values without waiting for completion
                //_ = ReadPLCValuesAsync();
                // Receive speed from PLC
                ReceiveSpeed();
                // Handle movements based on PLC instructions
                HandlePLCMovements();
                // Send the current position of the servo to the PLC
                SendCurrentPosToPLC();
                // If debug mode is enabled, change and send speed to PLC
                if (speedDebugControler) SendDebugSpeedToPLC();
                break;
        }
        // Update the axis position.
        UpdateAxisPos();
    }

    private void OnEnable()
    {
        PlcConnectionManager.OnPLCConnectedRelease += Call;
    }

    private void OnDisable()
    {
        PlcConnectionManager.OnPLCConnectedRelease -= Call;
    }

    void Call()
    {
        currentCoroutine = StartCoroutine(nameof(TimeUpdateData));
    }

    IEnumerator TimeUpdateData()
    {
        while (!PlcConnectionManager.InstanceManager.IsPLCDisconnected())
        {
            _ = ReadPLCValuesAsync();
            yield return new WaitForSeconds(waitSecs);
        }
    }

    protected async Task ReadPLCValuesAsync()
    {
        // Check if a task is already active, if so, return without executing further code
        if (isTaskActive) return;

        try
        {
            // Set the task as active
            isTaskActive = true;

            // Asynchronously read the values of PLC variables
            Task<bool> rightMoveTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(rightCode);
            Task<bool> leftMoveTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(leftCode);
            Task<uint> speedTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<uint>(speedCode);

            // Wait for all tasks to complete
            await Task.WhenAll(rightMoveTask, leftMoveTask, speedTask);

            // Handle the read values
            rightMove = rightMoveTask.Result;
            leftMove = leftMoveTask.Result;
            speedValue = speedTask.Result;
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during PLC reading
            Debug.LogError($"Error while reading PLC values: {ex.Message}");
        }
        finally
        {
            // Set the task as inactive
            isTaskActive = false;
        }
    }


    // Receive speed from PLC
    protected void ReceiveSpeed()
    {
        if (lastSpeedValue == speedValue) return;

        try
        {
            // Convert the read value to float type
            float result = speedValue.ConvertToFloat();

            // Store the converted speed value in the 'speed' variable
            speed = result;
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during reading or conversion
            Debug.LogError($"Error while reading float value: {ex.Message}");
            return;
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
    protected void HandlePLCMovements()
    {
        // If PLC signals a right movement, move the axis in the specified direction with a certain speed
        if (rightMove)
            MoveAxis(direction);

        // If PLC signals a left movement, move the axis in the opposite direction with the same speed
        if (leftMove)
            MoveAxis(-direction);
    }

    protected abstract void MoveAxis(Vector3 vector);
    protected abstract void SendCurrentPosToPLC();
    protected abstract void UpdateAxisPos();
    protected abstract void MoveAxisManually(Vector3 rotationDirection);
    protected abstract void LimitAxisPosition();
    protected abstract void HandleDebugMovements();
}
