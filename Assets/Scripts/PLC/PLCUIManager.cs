using S7.Net;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Gestor de interfaz de usuario para la conexión PLC
public class PLCUIManager : MonoBehaviour
{
    // Referencias a objetos del juego
    [SerializeField] GameObject conexionMenu;
    [SerializeField] PLCOptions defaultOptions;

    // Referencias a elementos de la interfaz de usuario
    [SerializeField] TMP_Dropdown cpuDropdown;
    [SerializeField] TMP_InputField ipInput;
    [SerializeField] TMP_InputField racksInput;
    [SerializeField] TMP_InputField slotsInput;

    // Datos del PLC
    CpuType selectedCPU;
    string IP;
    short racks;
    short slots;

    // Método llamado al iniciar el script
    private void Start()
    {
        // Obtener los nombres de los elementos en el enum CpuType
        string[] enumNames = System.Enum.GetNames(typeof(CpuType));

        // Limpiar opciones actuales y añadir nuevas opciones al dropdown de la CPU
        cpuDropdown.ClearOptions();
        cpuDropdown.AddOptions(new List<string>(enumNames));

        // Agregar un listener para detectar cambios en el dropdown de la CPU
        cpuDropdown.onValueChanged.AddListener(OnCpuDropdownChange);

        // Cargar los valores predeterminados al iniciar
        LoadDefaults();
    }

    #region Buttons

    // Método llamado al presionar el botón "Default"
    public void OnDefaultBtn() => LoadDefaults();

    // Método llamado al presionar el botón "Cancel"
    public void OnCancelBtn() => CloseMenu();

    // Método llamado al presionar el botón "Connect"
    public void OnConnectBtn()
    {
        try
        {
            PlcConnectionManager.InstanceManager.InitializeConnection(selectedCPU, IP, racks, slots);

            if (PlcConnectionManager.InstanceManager.IsPLCConnected())
                CloseMenu();
            else
                Debug.LogError("No se pudo establecer la conexión con el PLC. Verifica la configuración.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al conectar con el PLC: {ex.Message}");
        }
    }

    #endregion

    #region Dropdown

    // Método llamado cuando cambia el valor del dropdown de la CPU
    void OnCpuDropdownChange(int index)
    {
        // Accede al valor seleccionado del enum
        CpuType selectedEnumValue = (CpuType)System.Enum.Parse(typeof(CpuType), cpuDropdown.options[index].text);

        selectedCPU = selectedEnumValue;

        // Puedes realizar acciones adicionales con el valor seleccionado aquí
        Debug.Log("Seleccionaste: " + selectedEnumValue);
    }

    #endregion

    #region Inputs

    // Método llamado cuando cambia el valor del campo de entrada de la IP
    public void OnIpInputChange(string ipInput) => IP = ipInput;

    // Método llamado cuando cambia el valor del campo de entrada de Racks
    public void OnRacksInputChange(string racksInput) => racks = ValidateAndParseTxt(racksInput);

    // Método llamado cuando cambia el valor del campo de entrada de Slots
    public void OnslotsInputChange(string slotsInput) => slots = ValidateAndParseTxt(slotsInput);

    #endregion

    // Intenta convertir una cadena a short, maneja errores
    short ValidateAndParseTxt(string txt)
    {
        if (Int16.TryParse(txt, out short result) && result >= 0)
            return result;

        Debug.LogError("Valor no válido ingresado. Debe ser un número entero no negativo.");
        return 0;
    }

    // Oculta el menú de conexión
    void CloseMenu() => conexionMenu.SetActive(false);

    // Muestra el menú de conexión
    void OpenMenu() => conexionMenu.SetActive(true);

    // Carga los valores predeterminados en la interfaz de usuario
    void LoadDefaults()
    {
        int index = (int)defaultOptions.CPU;
        cpuDropdown.value = index;
        ipInput.text = defaultOptions.IP;
        racksInput.text = defaultOptions.racks.ToString();
        slotsInput.text = defaultOptions.slots.ToString();
    }
}
