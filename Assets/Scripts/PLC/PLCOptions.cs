using S7.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PLC Options", menuName = ("New PLC"))]
public class PLCOptions : ScriptableObject
{
    public CpuType CPU;
    public string IP;
    public short racks;
    public short slots;
}
