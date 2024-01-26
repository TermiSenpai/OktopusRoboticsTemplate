using UnityEngine;
using S7.Net;
using System;

public class PLCControl : MonoBehaviour
{
    Plc plc;
    [SerializeField] PLCOptions plcData;

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
        plc = new(plcData.CPU, plcData.IP, plcData.racks, plcData.racks);
        plc.Open();

        ResetLights();

    }

    private void ResetLights()
    {
        startedLight.Off();
        stoppedLight.Off();
        emergencyLight.Off();
    }

    private void Update()
    {
        // Read data
        bool machineWorking = (bool)plc.Read("DB1.DBX0.3");
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

        bool emergency = (bool)plc.Read("DB1.DBX1.0");
        if (emergency)
        {
            startedLight.Off();
            stoppedLight.Off();
            emergencyLight.On();
        }
    }

    // Write data
    // Buttons
    public void StartBtn()
    {
        plc.Write("DB1.DBX0.6", true);
        plc.Write("DB1.DBX0.7", false);
    }

    public void StopBtn()
    {
        plc.Write("DB1.DBX0.7", true);
        plc.Write("DB1.DBX0.6", false);
    }

    public void EmergencyStop()
    {
        bool currentState = (bool)plc.Read("DB1.DBX1.0");
        switch (currentState)
        {
            case true:
                plc.Write("DB1.DBX1.0", false);
                emergencyLight.Off();
                break;
            case false:
                plc.Write("DB1.DBX1.0", true);
                emergencyLight.On();
                break;

        }
    }
    public void Btn1_end()
    {
        plc.Write("DB1.DBX0.0", false);
    }
    public void Btn2_end()
    {
        plc.Write("DB1.DBX0.1", false);
    }
}
