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


    // M�todo llamado en cada frame para actualizar el estado del motor servo
    private void Update()
    {
        // Movimiento manual del servo durante la depuraci�n
        HandleDebugMovements();

        // Realizar movimientos controlados por PLC
        HandlePLCMovements();


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
    private async void HandlePLCMovements()
    {
        if (PlcConnectionManager.InstanceManager.IsPLCDisconnected()) return;

        bool rightMove = await PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(rightCode);
        if (rightMove)
            MoveAxis(direction * speed);


        bool leftMove = await PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(leftCode);
        if (leftMove)
            MoveAxis(-direction * speed);

        // Limitar la posici�n del eje seg�n la configuraci�n especificada
        LimitAxisPosition();

        // Enviar la posición del eje actual, basado en el eje que mueva
        SendCurrentPosToPLC();
    }

    private void SendCurrentPosToPLC()
    {
        switch (axisToLimit)
        {
            case AxisMovement.X:
                AxisToPlcX();
                break;
            case AxisMovement.Y:
                AxisToPlcY();
                break;
            case AxisMovement.Z:
                AxisToPlcZ();
                break;
        }
    }

    void AxisToPlcX()
    {
        bool derInput = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX1.3");
        bool leftInput = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX1.4");
        if (derInput || leftInput)
            PlcConnectionManager.InstanceManager.WriteVariableValue(positionCode, axis.transform.localPosition.x);
    }
    void AxisToPlcY()
    {
        bool derInput = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX1.7");
        bool leftInput = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX2.0");
        if (derInput || leftInput)
            PlcConnectionManager.InstanceManager.WriteVariableValue(positionCode, axis.transform.localPosition.y);
    }
    void AxisToPlcZ()
    {
        bool derInput = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX2.3");
        bool leftInput = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX2.4");
        if (derInput || leftInput)
            PlcConnectionManager.InstanceManager.WriteVariableValue(positionCode, axis.transform.localPosition.z);
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
