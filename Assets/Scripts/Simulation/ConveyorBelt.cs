using System.Threading.Tasks;
using System;
using UnityEngine;
using System.Collections;
using S7.Net;
using Unity.VisualScripting;

public class ConveyorBelt : MonoBehaviour
{
    [Header("PLC")]
    [SerializeField] private string conveyorCode;
    [SerializeField] private string speedCode;
    [Header("Manual")]
    // Variable que almacena la velocidad actual de la cinta transportadora
    [SerializeField] private float currentSpeed;
    [SerializeField] private float stopSpeed = 0f;
    [SerializeField] private float workingSpeed = 0.5f;
    bool currentBeltState;
    bool isTaskActive = false;
    Coroutine currentCoroutine;
    readonly float waitSecs = 0.1f;
    uint speedValue;

    private void Start()
    {
        SetSpeed(workingSpeed);
    }

    private void Update()
    {
        if (PlcConnectionManager.InstanceManager.IsPLCDisconnected()) return;

        switch (currentBeltState)
        {
            case false:
                SetSpeed(stopSpeed); break;
            case true:
                SetSpeed(speedValue.ConvertToFloat());
                break;
        }
    }



    // Método para establecer la velocidad de la cinta transportadora
    public void SetSpeed(float speed) => currentSpeed = speed;

    // Método para obtener la velocidad actual de la cinta transportadora
    public float GetSpeed()
    {
        return currentSpeed;
    }

    // Método para obtener el estado del sensor y ajustar la velocidad en consecuencia
    public void GetSensorState(bool sensorState)
    {
        if (currentBeltState == sensorState) return;
        if (!PlcConnectionManager.InstanceManager.IsPLCDisconnected()) return;
        // Estructura de control switch para manejar el estado del sensor  en el modo manual
        switch (sensorState)
        {
            // Si el estado del sensor es true, detener la cinta (velocidad = 0)
            case true:
                SetSpeed(stopSpeed);
                currentBeltState = sensorState;
                break;

            // Si el estado del sensor es false, establecer una velocidad predeterminada (0.5)
            case false:
                SetSpeed(workingSpeed);
                currentBeltState = sensorState;
                break;
        }
    }
    // Enable the script and subscribe to the OnPLCConnectedRelease event when the script is enabled
    private void OnEnable() => PlcConnectionManager.OnPLCConnectedRelease += Call;

    // Disable the script and unsubscribe from the OnPLCConnectedRelease event when the script is disabled
    private void OnDisable() => PlcConnectionManager.OnPLCConnectedRelease -= Call;

    // Method to be called when the OnPLCConnectedRelease event is triggered
    private void Call()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(nameof(TimeUpdateData));
    }

    // Coroutine to update data over time
    IEnumerator TimeUpdateData()
    {
        // Continue looping while the PLC is connected
        while (!PlcConnectionManager.InstanceManager.IsPLCDisconnected())
        {
            // Trigger an asynchronous read of PLC values and wait for a specified amount of time
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
            Task<uint> speedTask = PlcConnectionManager.InstanceManager.ReadVariableAsync<uint>(speedCode);
            Task<bool> conveyorState = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(conveyorCode);

            // Wait for all tasks to complete
            await Task.WhenAll(speedTask, conveyorState);

            // Handle the read values
            speedValue = speedTask.Result;
            currentBeltState = conveyorState.Result;
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
}
