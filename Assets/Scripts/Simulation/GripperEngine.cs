using System.Threading.Tasks;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GridBrushBase;

public class GripperEngine : Engine
{
    float axisRot;
    float lastAxisRot;

    private void Start()
    {
        switch (axisToLimit)
        {
            case AxisMovement.X: direction = Vector3.right; break;
            case AxisMovement.Y: direction = Vector3.up; break;
            case AxisMovement.Z: direction = Vector3.forward; break;
        }
    }

    private void Update()
    {
        // Switch statement to handle different cases based on PLC connection status
        switch (PlcConnectionManager.InstanceManager.IsPLCDisconnected())
        {
            // Case when PLC is disconnected
            case true:
                // Handle debugging movements
                HandleDebugRotations();
                // Limit the position of the servo axis
                LimitAxisRotation();
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

    // Manejar rotaciones manuales durante la depuraci�n
    private void HandleDebugRotations()
    {
        // Si la rotaci�n hacia la derecha est� habilitada para depuraci�n
        if (debugR)
            // Rotar el eje manualmente en la direcci�n especificada
            RotateAxisManually(direction);

        // Si la rotaci�n hacia la izquierda est� habilitada para depuraci�n
        if (debugL)
            // Rotar el eje manualmente en la direcci�n opuesta a la direcci�n especificada
            RotateAxisManually(-direction);
    }

    // Limitar la rotaci�n del eje seg�n la configuraci�n especificada
    private void LimitAxisRotation()
    {
        // Obtener la rotaci�n local actual del eje
        Quaternion currentRotation = objectToMove.transform.localRotation;


        // Switch statement basado en el eje para limitar
        switch (axisToLimit)
        {
            // Caso para limitar la rotaci�n alrededor del eje X
            case AxisMovement.X:
                float currentAxisX = currentRotation.x;
                // Clamp the angle between minAngle and maxAngle
                currentAxisX = Mathf.Clamp(currentAxisX, posMin, posMax);
                // Construir una rotaci�n con el �ngulo restringido alrededor del eje X
                objectToMove.transform.localEulerAngles = new Vector3(currentAxisX, currentRotation.y, currentRotation.z);
                break;

            // Caso para limitar la rotaci�n alrededor del eje Y
            case AxisMovement.Y:
                float currentAxisY = currentRotation.y;
                // Clamp the angle between minAngle and maxAngle
                currentAxisY = Mathf.Clamp(currentAxisY, posMin, posMax);
                // Construir una rotaci�n con el �ngulo restringido alrededor del eje X
                objectToMove.transform.localEulerAngles = new Vector3(currentRotation.x, currentAxisY, currentRotation.z); 
                break;

            // Caso para limitar la rotaci�n alrededor del eje Z
            case AxisMovement.Z:
                float currentAxisZ = objectToMove.transform.localEulerAngles.z;
                // Clamp the angle between minAngle and maxAngle
                currentAxisZ = Mathf.Clamp(currentAxisZ, posMin, posMax);
                // Construir una rotaci�n con el �ngulo restringido alrededor del eje X
                objectToMove.transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentAxisZ);

                break;
        }
    }


    // Rotar el eje del servo manualmente
    private void RotateAxisManually(Vector3 rotationDirection) => objectToMove.transform.localRotation *= Quaternion.Euler(rotationDirection * speed);

    // Rotar el eje del servo con velocidad especificada
    protected override void MoveAxis(Vector3 rotation) => objectToMove.transform.localRotation *= Quaternion.Euler(rotation * speed);

    protected override void SendCurrentPosToPLC()
    {
        PlcConnectionManager.InstanceManager.WriteVariableValue(positionCode, axisRot);
        // Comprobar si la rotaci�n actual del eje ha cambiado desde la �ltima actualizaci�n
        if (axisRot != lastAxisRot)
        {
            // Si la rotaci�n ha cambiado, actualizar la �ltima posici�n conocida y enviarla al PLC
            lastAxisRot = axisRot;
        }

        // Restablecer la bandera de actividad de la tarea para indicar que la tarea ya no est� activa
        isTaskActive = false;
    }

    protected override void UpdateAxisPos()
    {
        switch (axisToLimit)
        {
            case AxisMovement.X:
                // Actualizar la variable de rotaci�n del eje con el �ngulo alrededor del eje X
                axisRot = objectToMove.transform.localRotation.eulerAngles.x;
                break;
            case AxisMovement.Y:
                // Actualizar la variable de rotaci�n del eje con el �ngulo alrededor del eje Y
                axisRot = objectToMove.transform.localRotation.eulerAngles.y;
                break;
            case AxisMovement.Z:
                // Actualizar la variable de rotaci�n del eje con el �ngulo alrededor del eje Z
                axisRot = objectToMove.transform.localRotation.eulerAngles.z;
                break;
        }
    }

}
