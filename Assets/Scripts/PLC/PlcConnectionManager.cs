using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Clase que gestiona la conexi�n y operaciones con el PLC en un entorno de Unity.
/// </summary>

// Clase que gestiona la conexi�n y operaciones con el PLC
public class PlcConnectionManager : MonoBehaviour
{
    /// <summary>
    /// Instancia �nica de la clase (Singleton).
    /// </summary>
    // Instancia �nica de la clase (Singleton)
    public static PlcConnectionManager InstanceManager;

    /// <summary>
    /// Objeto que representa la conexi�n con el PLC.
    /// </summary>
    // Objeto que representa la conexi�n con el PLC
    private Plc plc;

    /// <summary>
    /// M�todo que se llama cuando se crea la instancia del script.
    /// </summary>
    private void Awake()
    {
        // Singleton: Garantiza que solo haya una instancia de la clase en la aplicaci�n
        if (InstanceManager == null)
            InstanceManager = this;

        else
            Destroy(gameObject); // Destruir el objeto si ya existe una instancia

    }

    /// <summary>
    /// M�todo para iniciar la conexi�n con el PLC.
    /// </summary>
    /// <param name="cpu">Tipo de CPU del PLC.</param>
    /// <param name="ip">Direcci�n IP del PLC.</param>
    /// <param name="racks">N�mero de racks del PLC.</param>
    /// <param name="slots">N�mero de slots del PLC.</param>
    public void InitializeConnection(CpuType cpu, string ip, short racks, short slots)
    {
        // Si ya est� conectado, no hace nada
        if (!IsPLCDisconnected()) return;

        // Crear una nueva instancia de Plc con los par�metros proporcionados
        plc = new(cpu, ip, racks, slots);
        plc.Open(); // Abrir la conexi�n con el PLC
    }

    /// <summary>
    /// M�todo para verificar si la conexi�n con el PLC est� activa.
    /// </summary>
    /// <returns>True si el PLC no est� conectado o si el objeto Plc es nulo, de lo contrario False.</returns>
    public bool IsPLCDisconnected()
    {
        // Devuelve true si el PLC no est� conectado o si el objeto Plc es nulo
        return plc == null || !plc.IsConnected;
    }

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
            // Registrar el evento de escritura
            Debug.Log($"Reading variable at {address}");

            object result = plc.Read(address);
            return (T)Convert.ChangeType(result, typeof(T));
            // Intentar leer la variable del PLC y convertirla al tipo T
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepci�n que pueda ocurrir al intentar leer la variable
            Debug.LogError($"Error reading variable at {address}: {ex.Message}");
            return default; // Devolver el valor predeterminado del tipo T
        }
    }

    /// <summary>
    /// M�todo para leer el valor de una variable del PLC de forma as�ncrona.
    /// </summary>
    /// <typeparam name="T">Tipo de datos del valor a leer.</typeparam>
    /// <param name="address">Direcci�n de la variable a leer.</param>
    /// <returns>Valor de la variable le�da de tipo T.</returns>
    public async Task<T> ReadVariableAsync<T>(string address)
    {
        // Verificar la conexi�n antes de intentar leer la variable
        if (IsPLCDisconnected())
        {
            Debug.LogError("PLC is not connected.");
            return default; // Devolver el valor predeterminado del tipo T
        }

        try
        {
            // Registrar el evento de escritura
            Debug.Log($"Reading variable at {address}");

            // Intentar leer la variable del PLC de forma as�ncrona
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
            // Manejar cualquier excepci�n que pueda ocurrir al intentar leer la variable
            Debug.LogError($"Error reading variable at {address}: {ex.Message}");
            return default; // Devolver el valor predeterminado del tipo T
        }
    }

    /// <summary>
    /// M�todo para escribir en una variable del PLC de forma as�ncrona.
    /// </summary>
    /// <param name="address">Direcci�n de la variable a escribir.</param>
    /// <param name="value">Valor a escribir en la variable.</param>
    public void WriteVariableAsync(string address, object value)
    {
        // Verificar la conexi�n antes de intentar escribir en la variable
        if (IsPLCDisconnected())
        {
            Debug.LogError("PLC is not connected.");
            return;
        }

        try
        {
            // Registrar el evento de escritura
            Debug.Log($"Writing variable at {address} with value {value}");
            // Intentar escribir en la variable del PLC
            plc.WriteAsync(address, value);
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepci�n que pueda ocurrir al intentar escribir en la variable
            Debug.LogError($"Error writing variable at {address}: {ex.Message}");
        }
    }
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
            // Registrar el evento de escritura
            Debug.Log($"Writing variable at {address} with value {value}");
            // Intentar escribir en la variable del PLC
            plc.Write(address, value);
        }
        catch (PlcException ex)
        {
            // Manejar la excepción específica de PLC
            Debug.LogError($"Error writing variable at {address}: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Manejar cualquier otra excepción genérica
            Debug.LogError($"Unexpected error writing variable at {address}: {ex.Message}");
        }
    }
}
