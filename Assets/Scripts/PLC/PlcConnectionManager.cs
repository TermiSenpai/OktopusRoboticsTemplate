using S7.Net;
using System;
using UnityEngine;

// Clase que gestiona la conexión y operaciones con el PLC
public class PlcConnectionManager : MonoBehaviour
{
    // Instancia única de la clase (Singleton)
    public static PlcConnectionManager InstanceManager;

    // Objeto que representa la conexión con el PLC
    private Plc plc;

    // Método que se llama cuando se crea la instancia del script
    private void Awake()
    {
        // Singleton: Garantiza que solo haya una instancia de la clase en la aplicación
        if (InstanceManager == null)        
            InstanceManager = this;            
        
        else        
            Destroy(gameObject); // Destruir el objeto si ya existe una instancia
        
    }

    // Método para iniciar la conexión con el PLC
    public void InitializeConnection(CpuType cpu, string ip, short racks, short slots)
    {
        // Crear una nueva instancia de Plc con los parámetros proporcionados
        plc = new(cpu, ip, racks, slots);
        plc.Open(); // Abrir la conexión con el PLC
    }

    // Método para verificar si la conexión con el PLC está activa
    public bool IsPLCConnected()
    {
        // Devuelve true si el PLC no está conectado o si el objeto Plc es nulo
        return plc == null || !plc.IsConnected;
    }

    // Método para leer el valor de una variable del PLC
    public T ReadVariableValue<T>(string address)
    {
        // Verificar la conexión antes de intentar leer la variable
        if (IsPLCConnected())
        {
            Debug.LogError("PLC is not connected.");
            return default; // Devolver el valor predeterminado del tipo T
        }

        try
        {
            // Intentar leer la variable del PLC y convertirla al tipo T
            return (T)plc.Read(address);
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que pueda ocurrir al intentar leer la variable
            Debug.LogError($"Error reading variable at {address}: {ex.Message}");
            return default; // Devolver el valor predeterminado del tipo T
        }
    }

    // Método para escribir en una variable del PLC
    public void WriteVariableValue(string address, object value)
    {
        // Verificar la conexión antes de intentar escribir en la variable
        if (IsPLCConnected())
        {
            Debug.LogError("PLC is not connected.");
            return;
        }

        try
        {
            // Intentar escribir en la variable del PLC
            plc.Write(address, value);
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que pueda ocurrir al intentar escribir en la variable
            Debug.LogError($"Error writing variable at {address}: {ex.Message}");
        }
    }
}
