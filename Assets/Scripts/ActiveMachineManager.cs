using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMachineManager : MonoBehaviour
{
    [SerializeField] MachineScriptable[] machines;

    Transform parent;
    List<GameObject> currentSceneMachines = new();
    GameObject activeMachine;
    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("MachinesParent").GetComponent<Transform>();
        SpawnAllMachinesInScene();
        ChangeMachine(currentSceneMachines[0]);
    }

    public void SpawnAllMachinesInScene()
    {
        foreach (var machine in machines)        
            currentSceneMachines.Add(Instantiate(machine.machineObj, parent));
        
    }

    public void ChangeMachine(GameObject machine)
    {
        if (activeMachine != null)
            activeMachine.SetActive(false);
        activeMachine = machine;
        activeMachine.SetActive(true);
    }
}
