using UnityEngine;
using S7.Net;

public class PLCControl : MonoBehaviour
{
    Plc plc;

    private void Start()
    {
        plc = new (CpuType.S71200, "127.0.0.1", 0, 1);
        plc.Open();
    }

    private void Update()
    {
        // Read data
        bool db1Bool1 = (bool)plc.Read("DB1.DBX0.0");
        if (db1Bool1 == true)
        {

        }
        else
        {
           
        }

        bool db1Bool2 = (bool)plc.Read("DB1.DBX0.1");
        if (db1Bool2 == true)
        {
        }
        else
        {
        }
    }

    // Write data
    // Buttons
    public void Btn1()
    {
        plc.Write("DB1.DBX0.0", true);
    }
    public void Btn1_end()
    {
        plc.Write("DB1.DBX0.0", false);
    }

    public void Btn2()
    {
        plc.Write("DB1.DBX0.1", true);
    }
    public void Btn2_end()
    {
        plc.Write("DB1.DBX0.1", false);
    }
}
