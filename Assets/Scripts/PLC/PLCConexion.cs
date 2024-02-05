using S7.Net;
using System;
using UnityEngine;
public class PLCConexion : MonoBehaviour
{
    public static PLCConexion Instance;

    private Plc plc;

    // Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartConexion(CpuType cpu, string ip, short racks, short slots)
    {
        plc = new(cpu, ip, racks, slots);
        plc.Open();
    }

    public bool CheckPLCConnection()
    {
        return plc == null || !plc.IsConnected;
    }

    public T ReadVariable<T>(string address)
    {
        if (CheckPLCConnection())
        {
            Debug.LogError("PLC is not connected.");
            return default;
        }

        try
        {
            return (T)plc.Read(address);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error reading variable at {address}: {ex.Message}");
            return default;
        }
    }

    public void WriteVariable(string address, object value)
    {
        if (CheckPLCConnection())
        {
            Debug.LogError("PLC is not connected.");
            return;
        }

        try
        {
            plc.Write(address, value);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error writing variable at {address}: {ex.Message}");
        }
    }
}
