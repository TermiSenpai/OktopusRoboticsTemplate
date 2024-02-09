using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Clase que gestiona la conexión y operaciones con el PLC en un entorno de Unity.
/// </summary>

// Clase que gestiona la conexión y operaciones con el PLC
public class PlcConnectionManager : MonoBehaviour
{
    /// <summary>
    /// Instancia única de la clase (Singleton).
    /// </summary>
    // Instancia única de la clase (Singleton)
    public static PlcConnectionManager InstanceManager;

    /// <summary>
    /// Objeto que representa la conexión con el PLC.
    /// </summary>
    // Objeto que representa la conexión con el PLC
    private Plc plc;

    /// <summary>
    /// Método que se llama cuando se crea la instancia del script.
    /// </summary>
    private void Awake()
    {
        // Singleton: Garantiza que solo haya una instancia de la clase en la aplicación
        if (InstanceManager == null)
            InstanceManager = this;

        else
            Destroy(gameObject); // Destruir el objeto si ya existe una instancia

    }

    /// <summary>
    /// Método para iniciar la conexión con el PLC.
    /// </summary>
    /// <param name="cpu">Tipo de CPU del PLC.</param>
    /// <param name="ip">Dirección IP del PLC.</param>
    /// <param name="racks">Número de racks del PLC.</param>
    /// <param name="slots">Número de slots del PLC.</param>
    public void InitializeConnection(CpuType cpu, string ip, short racks, short slots)
    {
        // Si ya está conectado, no hace nada
        if (!IsPLCDisconnected()) return;

        // Crear una nueva instancia de Plc con los parámetros proporcionados
        plc = new(cpu, ip, racks, slots);
        plc.Open(); // Abrir la conexión con el PLC
    }

    /// <summary>
    /// Método para verificar si la conexión con el PLC está activa.
    /// </summary>
    /// <returns>True si el PLC no está conectado o si el objeto Plc es nulo, de lo contrario False.</returns>
    public bool IsPLCDisconnected()
    {
        // Devuelve true si el PLC no está conectado o si el objeto Plc es nulo
        return plc == null || !plc.IsConnected;
    }

    // Método para leer el valor de una variable del PLC
    public T ReadVariableValue<T>(string address)
    {
        // Verificar la conexión antes de intentar leer la variable
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
            // Manejar cualquier excepción que pueda ocurrir al intentar leer la variable
            Debug.LogError($"Error reading variable at {address}: {ex.Message}");
            return default; // Devolver el valor predeterminado del tipo T
        }
    }

    /// <summary>
    /// Método para leer el valor de una variable del PLC de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">Tipo de datos del valor a leer.</typeparam>
    /// <param name="address">Dirección de la variable a leer.</param>
    /// <returns>Valor de la variable leída de tipo T.</returns>
    public async Task<T> ReadVariableAsync<T>(string address)
    {
        // Verificar la conexión antes de intentar leer la variable
        if (IsPLCDisconnected())
        {
            Debug.LogError("PLC is not connected.");
            return default; // Devolver el valor predeterminado del tipo T
        }

        try
        {
            // Intentar leer la variable del PLC de forma asíncrona
            object result = await plc.ReadAsync(address);
            // Si el resultado es nulo, devolver el valor predeterminado del tipo T
            if (result == null)
            {
                Debug.LogError($"Variable at {address} returned null.");
                return default;
            }

            // Convertir el resultado al tipo T
            return (T)Convert.ChangeType(result, typeof(T));
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que pueda ocurrir al intentar leer la variable
            Debug.LogError($"Error reading variable at {address}: {ex.Message}");
            return default; // Devolver el valor predeterminado del tipo T
        }
    }

    /// <summary>
    /// Método para escribir en una variable del PLC de forma asíncrona.
    /// </summary>
    /// <param name="address">Dirección de la variable a escribir.</param>
    /// <param name="value">Valor a escribir en la variable.</param>
    public void WriteVariableAsync(string address, object value)
    {
        // Verificar la conexión antes de intentar escribir en la variable
        if (IsPLCDisconnected())
        {
            Debug.LogError("PLC is not connected.");
            return;
        }

        try
        {
            // Intentar escribir en la variable del PLC
            plc.WriteAsync(address, value);
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que pueda ocurrir al intentar escribir en la variable
            Debug.LogError($"Error writing variable at {address}: {ex.Message}");
        }
    }
    public void WriteVariableValue(string address, object value)
    {
        // Verificar la conexión antes de intentar escribir en la variable
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
            // Manejar cualquier excepción que pueda ocurrir al intentar escribir en la variable
            Debug.LogError($"Error writing variable at {address}: {ex.Message}");
        }
    }
}
