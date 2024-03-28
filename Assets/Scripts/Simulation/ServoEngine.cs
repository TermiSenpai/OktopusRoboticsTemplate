using S7.Net;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

// Clase que controla el movimiento de un motor servo en un eje espec�fico
public class ServoEngine : Engine
{
    // Private
    float axisPos;
    float lastAxisPos;

    // Limit the position of the axis according to the specified configuration
    protected override void LimitAxisPosition()
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
    protected override async Task SendCurrentPosToPLC()
    {
        if (isTaskActive) return;
        // Check if the current axis position has changed since the last update
        if (axisPos != lastAxisPos)
        {
            // If the position has changed, update the last known position
            lastAxisPos = axisPos;

            // Start a coroutine to send the position asynchronously
            // Asynchronously read the values of PLC variables

            await plcInstance.WriteVariableAsync(positionCode, axisPos);
            //StartCoroutine(SendPosToPLCAsyncCoroutine(positionCode, axisPos));
        }
    }

    //private IEnumerator SendPosToPLCAsyncCoroutine(string code, float pos)
    //{
    //    // Write position to PLC asynchronously
    //    Task task = Task.Run(async () =>
    //    {
    //        await plcInstance.WriteVariableAsync(code, pos);
    //    });

    //    // Wait for the task to complete
    //    yield return new WaitUntil(() => task.IsCompleted);

    //    isTaskActive = false;

    //    yield return null; // Signal coroutine completion
    //}
    // Mover el eje del servo con velocidad especificada
    public override void MoveAxis(Vector3 movement) => objectToMove.transform.localPosition += movement * Speed;
}
