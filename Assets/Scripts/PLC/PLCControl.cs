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
        if (PLCConexion.Instance.CheckPLCConnection())
            return;

        // Read data
        bool machineWorking = PLCConexion.Instance.ReadVariable<bool>("DB1.DBX0.3");
        UpdateLights(machineWorking);

        bool emergency = PLCConexion.Instance.ReadVariable<bool>("DB1.DBX1.0");
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
        PLCConexion.Instance.WriteVariable("DB1.DBX0.6", true);
        PLCConexion.Instance.WriteVariable("DB1.DBX0.7", false);
    }

    public void StopBtn()
    {
        PLCConexion.Instance.WriteVariable("DB1.DBX0.7", true);
        PLCConexion.Instance.WriteVariable("DB1.DBX0.6", false);
    }

    public void EmergencyStop()
    {
        bool currentState = PLCConexion.Instance.ReadVariable<bool>("DB1.DBX1.0");
        switch (currentState)
        {
            case true:
                PLCConexion.Instance.WriteVariable("DB1.DBX1.0", false);
                emergencyLight.TurnOff();
                break;
            case false:
                PLCConexion.Instance.WriteVariable("DB1.DBX1.0", true);
                emergencyLight.TurnOn();
                break;

        }
    }
    public void Btn1_end()
    {
        PLCConexion.Instance.WriteVariable("DB1.DBX0.0", false);
    }
    public void Btn2_end()
    {
        PLCConexion.Instance.WriteVariable("DB1.DBX0.1", false);
    }

    //Sensors
    public void OnSensorDetected(string plcCode, bool detectionResult)
    {
        if (PLCConexion.Instance.CheckPLCConnection()) return;
        PLCConexion.Instance.WriteVariable(plcCode, detectionResult);
    }
}
