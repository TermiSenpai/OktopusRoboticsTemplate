using UnityEngine;

public class PLCControl : MonoBehaviour, ISensorEventHandler
{
    public static PLCControl Instance;

    MachineLights startedLight;
    MachineLights stoppedLight;
    MachineLights emergencyLight;


    private void Awake()
    {
        startedLight = GameObject.Find("StartedLight").GetComponent<MachineLights>();
        stoppedLight = GameObject.Find("StopLight").GetComponent<MachineLights>();
        emergencyLight = GameObject.Find("EmergencyLight").GetComponent<MachineLights>();
    }

    private void Start()
    {
        ResetLights();
    }

    private void ResetLights()
    {
        startedLight.TurnOff();
        stoppedLight.TurnOff();
        emergencyLight.TurnOff();
    }


    private void Update()
    {
        if (PlcConnectionManager.InstanceManager.IsPLCDisconnect())
            return;

        // Read data
        bool machineWorking = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX0.3");
        UpdateLights(machineWorking);

        bool emergency = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX1.0");
        UpdateEmergencyLight(emergency);

    }

    private void UpdateLights(bool machineWorking)
    {
        if (machineWorking)
        {
            startedLight.TurnOn();
            stoppedLight.TurnOff();
        }
        else
        {
            startedLight.TurnOff();
            stoppedLight.TurnOn();
        }
    }

    private void UpdateEmergencyLight(bool emergency)
    {
        if (emergency)
        {
            startedLight.TurnOff();
            stoppedLight.TurnOff();
            emergencyLight.TurnOn();
        }
        else
        {
            emergencyLight.TurnOff();
        }
    }
    // Write data
    // Buttons
    public void StartBtn()
    {
        PlcConnectionManager.InstanceManager.WriteVariableValue("DB1.DBX0.6", true);
        PlcConnectionManager.InstanceManager.WriteVariableValue("DB1.DBX0.7", false);
    }

    public void StopBtn()
    {
        PlcConnectionManager.InstanceManager.WriteVariableValue("DB1.DBX0.7", true);
        PlcConnectionManager.InstanceManager.WriteVariableValue("DB1.DBX0.6", false);
    }

    public void EmergencyStop()
    {
        bool currentState = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX1.0");
        switch (currentState)
        {
            case true:
                PlcConnectionManager.InstanceManager.WriteVariableValue("DB1.DBX1.0", false);
                emergencyLight.TurnOff();
                break;
            case false:
                PlcConnectionManager.InstanceManager.WriteVariableValue("DB1.DBX1.0", true);
                emergencyLight.TurnOn();
                break;

        }
    }
    public void Btn1_end()
    {
        PlcConnectionManager.InstanceManager.WriteVariableValue("DB1.DBX0.0", false);
    }
    public void Btn2_end()
    {
        PlcConnectionManager.InstanceManager.WriteVariableValue("DB1.DBX0.1", false);
    }

    //Sensors
    public void OnSensorDetected(string plcCode, bool detectionResult)
    {
        if (PlcConnectionManager.InstanceManager.IsPLCDisconnect()) return;
        PlcConnectionManager.InstanceManager.WriteVariableValue(plcCode, detectionResult);
    }
}
