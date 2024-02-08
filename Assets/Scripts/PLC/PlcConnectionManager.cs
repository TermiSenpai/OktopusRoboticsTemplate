using S7.Net;
using System;
using System.Threading.Tasks;
using UnityEngine;

// Clase que gestiona la conexi�n y operaciones con el PLC
public class PlcConnectionManager : MonoBehaviour
{
    // Instancia �nica de la clase (Singleton)
    public static PlcConnectionManager InstanceManager;

    // Objeto que representa la conexi�n con el PLC
    private Plc plc;

    // M�todo que se llama cuando se crea la instancia del script
    private void Awake()
    {
        // Singleton: Garantiza que solo haya una instancia de la clase en la aplicaci�n
        if (InstanceManager == null)
            InstanceManager = this;

        else
            Destroy(gameObject); // Destruir el objeto si ya existe una instancia

    }

    // M�todo para iniciar la conexi�n con el PLC
    public void InitializeConnection(CpuType cpu, string ip, short racks, short slots)
    {
        // Crear una nueva instancia de Plc con los par�metros proporcionados
        plc = new(cpu, ip, racks, slots);
        plc.Open(); // Abrir la conexi�n con el PLC
    }

    // M�todo para verificar si la conexi�n con el PLC est� activa
    public bool IsPLCDisconnected()
    {
        // Devuelve true si el PLC no est� conectado o si el objeto Plc es nulo
        return plc == null || !plc.IsConnected;
    }

    //// M�todo para leer el valor de una variable del PLC
    //public T ReadVariableValue<T>(string address)
    //{
    //    // Verificar la conexi�n antes de intentar leer la variable
    //    if (IsPLCDisconnected())
    //    {
    //        Debug.LogError("PLC is not connected.");
    //        return default; // Devolver el valor predeterminado del tipo T
    //    }

    //    try
    //    {
    //        // Intentar leer la variable del PLC y convertirla al tipo T
    //        return (T)plc.Read(address);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Manejar cualquier excepci�n que pueda ocurrir al intentar leer la variable
    //        Debug.LogError($"Error reading variable at {address}: {ex.Message}");
    //        return default; // Devolver el valor predeterminado del tipo T
    //    }
    //}

    // M�todo para leer el valor de una variable del PLC
    public T ReadVariableValue<T>(string address)
    {
        // Verificar la conexi�n antes de intentar leer la variable
        if (IsPLCDisconnected())
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
            // Manejar cualquier excepci�n que pueda ocurrir al intentar leer la variable
            Debug.LogError($"Error reading variable at {address}: {ex.Message}");
            return default; // Devolver el valor predeterminado del tipo T
        }
    }



    // M�todo asincr�nico para leer el valor de una variable del PLC
    public async Task<T> ReadVariableValueAsync<T>(string address)
    {
        // Verificar la conexi�n antes de intentar leer la variable
        if (IsPLCDisconnected())
        {
            Debug.LogError("PLC is not connected.");
            return default; // Devolver el valor predeterminado del tipo T
        }

        try
        {
            // Intentar leer la variable del PLC de forma as�ncrona
            return await Task.Run(() => (T)plc.Read(address));
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepci�n que pueda ocurrir al intentar leer la variable
            Debug.LogError($"Error reading variable at {address}: {ex.Message}");
            return default; // Devolver el valor predeterminado del tipo T
        }
    }


    // M�todo para escribir en una variable del PLC
    public void WriteVariableValue(string address, object value)
    {
        // Verificar la conexi�n antes de intentar escribir en la variable
        if (IsPLCDisconnected())
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
            // Manejar cualquier excepci�n que pueda ocurrir al intentar escribir en la variable
            Debug.LogError($"Error writing variable at {address}: {ex.Message}");
        }
    }
}
