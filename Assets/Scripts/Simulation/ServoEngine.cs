using S7.Net;
using S7.Net.Types;
using System;
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
    // Codes associated with right and left movements (editable from the Inspector)
    [Header("PLC Codes")]
    [SerializeField] private string rightCode;
    [SerializeField] private string leftCode;
    [SerializeField] private string rightInputCode;
    [SerializeField] private string leftInputCode;
    [SerializeField] private string positionCode;
    [SerializeField] private string speedCode;

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
    [SerializeField] private float debugSpeed = 0.1f;

    [SerializeField] float currentpos;
    // Private
    float axisPos;
    float lastAxisPos;
    bool isTaskActive = false;
    //bool rightInput;
    //bool leftInput;


    // Method called on each frame to update the state of the servo motor
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
                var convert = debugSpeed.ConvertToUInt();
                PlcConnectionManager.InstanceManager.WriteVariableValue(speedCode, convert);
                Debug.Log(debugSpeed);
                break;
        }
        UpdateAxisPos();
    }


    // Handle manual movements during debugging
    private void HandleDebugMovements()
    {
        // If debugging right movement is enabled
        if (debugR)
            // Move the axis manually in the specified direction
            MoveAxisManually(direction);

        // If debugging left movement is enabled
        if (debugL)
            // Move the axis manually in the opposite direction to the specified direction
            MoveAxisManually(-direction);
    }


    // Limit the position of the axis according to the specified configuration
    private void LimitAxisPosition()
    {
        // Get the current local position of the axis
        Vector3 currentPosition = axis.transform.localPosition;
        // Variable to store the clamped value
        float clampedValue;

        // Switch statement based on the axis to limit
        switch (axisToLimit)
        {
            // Case for limiting movement along the X axis
            case AxisMovement.X:
                // Clamp the x-coordinate of the current position between posMin and posMax
                clampedValue = Mathf.Clamp(currentPosition.x, posMin, posMax);
                // Update the local position of the axis with the clamped x-coordinate
                axis.transform.localPosition = new Vector3(clampedValue, currentPosition.y, currentPosition.z);
                break;

            // Case for limiting movement along the Y axis
            case AxisMovement.Y:
                // Clamp the y-coordinate of the current position between posMin and posMax
                clampedValue = Mathf.Clamp(currentPosition.y, posMin, posMax);
                // Update the local position of the axis with the clamped y-coordinate
                axis.transform.localPosition = new Vector3(currentPosition.x, clampedValue, currentPosition.z);
                break;

            // Case for limiting movement along the Z axis
            case AxisMovement.Z:
                // Clamp the z-coordinate of the current position between posMin and posMax
                clampedValue = Mathf.Clamp(currentPosition.z, posMin, posMax);
                // Update the local position of the axis with the clamped z-coordinate
                axis.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, clampedValue);
                break;
        }
    }

    void UpdateAxisPos()
    {
        switch (axisToLimit)
        {
            case AxisMovement.X:
                // Update the axis position variable with the clamped x-coordinate
                axisPos = axis.transform.localPosition.x;
                break;
            case AxisMovement.Y:
                // Update the axis position variable with the clamped y-coordinate
                axisPos = axis.transform.localPosition.y;
                break;
            case AxisMovement.Z:
                // Update the axis position variable with the clamped z-coordinate
                axisPos = axis.transform.localPosition.z;
                break;
        }
    }


    // Perform movements controlled by PLC
    private async void HandlePLCMovements()
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
            MoveAxis(direction * speed);

        // If PLC signals a left movement, move the axis in the opposite direction with the same speed
        if (leftMove)
            MoveAxis(-direction * speed);
    }


    // No borrar todavia
    //Task<bool> rightInputTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(rightInputCode);
    //Task<bool> leftInputTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(leftInputCode);

    //await Task.WhenAll(rightInputTask, leftInputTask);

    //leftInput = leftInputTask.Result;
    //rightInput = leftInputTask.Result;

    //if (rightInput || leftInput)

    // Send the current position to PLC
    private void SendCurrentPosToPLC()
    {
        PlcConnectionManager.InstanceManager.WriteVariableValue(positionCode, axisPos);
        // Check if the current axis position has changed since the last update
        if (lastAxisPos != axisPos)
        {
            // If the position has changed, update the last known position and send it to the PLC
            lastAxisPos = axisPos;
        }

        // Reset the task activity flag to indicate that the task is no longer active
        isTaskActive = false;

        currentpos = PlcConnectionManager.InstanceManager.ReadVariableValue<float>(positionCode);
    }

    private void ReceiveSpeed()
    {
        try
        {
            var valorFloat = PlcConnectionManager.InstanceManager.ReadVariableValue<uint>(speedCode);
            float result = valorFloat.ConvertToFloat();
            speed = result;
        }
        catch (Exception ex)
        {
            // Manejar la excepción
            Debug.LogError($"Error al leer el valor float: {ex.Message}");
        }
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
