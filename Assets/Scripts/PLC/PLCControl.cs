using UnityEngine;

public class PLCControl : MonoBehaviour
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
        startedLight.Off();
        stoppedLight.Off();
        emergencyLight.Off();
    }

    public void OnSensorDetection(string code, bool value)
    {
        PLCConexion.plc.Write(code, value);
    }

    private void Update()
    {
        if (PLCConexion.plc == null || !PLCConexion.plc.IsConnected)
            return;

        // Read data
        bool machineWorking = (bool)PLCConexion.plc.Read("DB1.DBX0.3");
        if (machineWorking == true)
        {
            startedLight.On();
            stoppedLight.Off();
        }
        else
        {
            startedLight.Off();
            stoppedLight.On();
        }

        bool emergency = (bool)PLCConexion.plc.Read("DB1.DBX1.0");
        if (emergency)
        {
            startedLight.Off();
            stoppedLight.Off();
            emergencyLight.On();
        }

        // Write current x pos
        PLCConexion.plc.Write("DB1.DBD6", PaletizerAxisMovement.xAxis.position.x);
        Debug.Log(PaletizerAxisMovement.xAxis.position.x);
    }

    // Write data
    // Buttons
    public void StartBtn()
    {
        PLCConexion.plc.Write("DB1.DBX0.6", true);
        PLCConexion.plc.Write("DB1.DBX0.7", false);
    }

    public void StopBtn()
    {
        PLCConexion.plc.Write("DB1.DBX0.7", true);
        PLCConexion.plc.Write("DB1.DBX0.6", false);
    }

    public void EmergencyStop()
    {
        bool currentState = (bool)PLCConexion.plc.Read("DB1.DBX1.0");
        switch (currentState)
        {
            case true:
                PLCConexion.plc.Write("DB1.DBX1.0", false);
                emergencyLight.Off();
                break;
            case false:
                PLCConexion.plc.Write("DB1.DBX1.0", true);
                emergencyLight.On();
                break;

        }
    }
    public void Btn1_end()
    {
        PLCConexion.plc.Write("DB1.DBX0.0", false);
    }
    public void Btn2_end()
    {
        PLCConexion.plc.Write("DB1.DBX0.1", false);
    }
}
