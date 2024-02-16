using S7.Net;
using System;
using System.Threading.Tasks;
using UnityEngine;

// Clase que controla el movimiento de un motor servo en un eje espec�fico
public class ServoEngine : Engine
{
    // Private
    float axisPos;
    float lastAxisPos;

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
                // If debug, change and send speed to plc
                if (speedDebugControler) SendDebugSpeedToPLC();
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
        Vector3 currentPosition = objectToMove.transform.localPosition;
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
                objectToMove.transform.localPosition = new Vector3(clampedValue, currentPosition.y, currentPosition.z);
                break;

            // Case for limiting movement along the Y axis
            case AxisMovement.Y:
                // Clamp the y-coordinate of the current position between posMin and posMax
                clampedValue = Mathf.Clamp(currentPosition.y, posMin, posMax);
                // Update the local position of the axis with the clamped y-coordinate
                objectToMove.transform.localPosition = new Vector3(currentPosition.x, clampedValue, currentPosition.z);
                break;

            // Case for limiting movement along the Z axis
            case AxisMovement.Z:
                // Clamp the z-coordinate of the current position between posMin and posMax
                clampedValue = Mathf.Clamp(currentPosition.z, posMin, posMax);
                // Update the local position of the axis with the clamped z-coordinate
                objectToMove.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, clampedValue);
                break;
        }
    }

    protected override void UpdateAxisPos()
    {
        switch (axisToLimit)
        {
            case AxisMovement.X:
                // Update the axis position variable with the clamped x-coordinate
                axisPos = objectToMove.transform.localPosition.x;
                break;
            case AxisMovement.Y:
                // Update the axis position variable with the clamped y-coordinate
                axisPos = objectToMove.transform.localPosition.y;
                break;
            case AxisMovement.Z:
                // Update the axis position variable with the clamped z-coordinate
                axisPos = objectToMove.transform.localPosition.z;
                break;
        }
    }    

    // Send the current position to PLC
    protected override void SendCurrentPosToPLC()
    {
        PlcConnectionManager.InstanceManager.WriteVariableValue(positionCode, axisPos);
        // Check if the current axis position has changed since the last update
        if (axisPos != lastAxisPos)
        {
            // If the position has changed, update the last known position and send it to the PLC
            lastAxisPos = axisPos;
        }

        // Reset the task activity flag to indicate that the task is no longer active
        isTaskActive = false;
    }

    // Mover el eje del servo manualmente
    private void MoveAxisManually(Vector3 movementDirection)
    {
        objectToMove.transform.localPosition += movementDirection * speed;
    }

    // Mover el eje del servo con velocidad especificada
    protected override void MoveAxis(Vector3 movement) => objectToMove.transform.localPosition += movement * speed;
}
