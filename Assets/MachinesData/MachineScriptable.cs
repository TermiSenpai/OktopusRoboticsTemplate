using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="New Machine")]
public class MachineScriptable : ScriptableObject
{
    public GameObject machineObj;

    public float top;
    public float mid;
    public float bottom;
}
