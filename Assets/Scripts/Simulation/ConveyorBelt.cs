using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    // Variable que almacena la velocidad actual de la cinta transportadora
    [SerializeField] private float currentSpeed;
    bool currentBeltState;

    // Método para establecer la velocidad de la cinta transportadora
    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    // Método para obtener la velocidad actual de la cinta transportadora
    public float GetSpeed()
    {
        return currentSpeed;
    }

    // Método para obtener el estado del sensor y ajustar la velocidad en consecuencia
    public void GetSensorState(bool sensorState)
    {
        if (currentBeltState == sensorState) return;

        // Estructura de control switch para manejar el estado del sensor
        switch (sensorState)
        {
            // Si el estado del sensor es true, detener la cinta (velocidad = 0)
            case true:
                SetSpeed(0f);
                currentBeltState = sensorState;
                break;

            // Si el estado del sensor es false, establecer una velocidad predeterminada (0.5)
            case false:
                SetSpeed(0.5f);
                currentBeltState = sensorState;
                break;
        }
    }
}
