using S7.Net;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PLCUIManager : MonoBehaviour
{
    [SerializeField] GameObject conexionMenu;

    [SerializeField] PLCOptions defaultOptions;

    // UI
    [SerializeField] TMP_Dropdown cpuDropdown;
    [SerializeField] TMP_InputField ipInput;
    [SerializeField] TMP_InputField racksInput;
    [SerializeField] TMP_InputField slotsInput;


    // PLC Data
    CpuType selectedCPU;
    string IP;
    short racks;
    short slots;

    private void Start()
    {
        string[] enumNames = System.Enum.GetNames(typeof(CpuType));

        cpuDropdown.ClearOptions();
        cpuDropdown.AddOptions(new List<string>(enumNames));
        cpuDropdown.onValueChanged.AddListener(OnCpuDropdownChange);

        LoadDefaults();
    }


    #region Buttons
    public void OnDefaultBtn()
    {
        LoadDefaults();
    }

    public void OnCancelBtn()
    {
        CloseMenu();

    }

    public void OnConnectBtn()
    {
        PlcConnectionManager.InstanceManager.InitializeConnection(selectedCPU, IP, racks, slots);

        if (PlcConnectionManager.InstanceManager.IsPLCConnected()) CloseMenu();
    }
    #endregion

    #region Dropdown

    void OnCpuDropdownChange(int index)
    {
        // Accede al valor seleccionado del enum
        CpuType selectedEnumValue = (CpuType)System.Enum.Parse(typeof(CpuType), cpuDropdown.options[index].text);

        selectedCPU = selectedEnumValue;

        // Puedes hacer algo con el valor seleccionado aquí
        Debug.Log("Seleccionaste: " + selectedEnumValue);
    }

    #endregion

    #region Inputs

    public void OnIpInputChange(string ipInput)
    {
        IP = ipInput;
    }

    public void OnRacksInputChange(string racksInput)
    {
        racks = TryParseTxt(racksInput);
    }

    public void OnslotsInputChange(string slotsInput)
    {
        slots = TryParseTxt(slotsInput);
    }

    #endregion

    short TryParseTxt(string txt)
    {

        if (Int16.TryParse(txt, out short result))
            return result;

        Debug.LogError("Error al convertir a short");
        return 0;
    }

    void CloseMenu() => conexionMenu.SetActive(false);


    void OpenMenu() => conexionMenu.SetActive(true);


    void LoadDefaults()
    {
        int index = (int)defaultOptions.CPU;
        cpuDropdown.value = index;
        ipInput.text = defaultOptions.IP;
        racksInput.text = defaultOptions.racks.ToString();
        slotsInput.text = defaultOptions.slots.ToString();
    }

}
