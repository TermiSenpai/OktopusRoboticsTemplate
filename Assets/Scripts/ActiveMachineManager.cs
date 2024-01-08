using System.Collections.Generic;
using UnityEngine;

public class ActiveMachineManager : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] MachineScriptable[] machines;
    List<GameObject> currentSceneMachines = new List<GameObject>();
    GameObject activeMachine;
    // Start is called before the first frame update
    void Start()
    {
        SpawnAllMachinesInScene();
        changeMachine(currentSceneMachines[0]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnAllMachinesInScene()
    {
        foreach (var machine in machines)
        {
            currentSceneMachines.Add(Instantiate(machine.machineObj, parent));
        }

    }

    public void changeMachine(GameObject machine)
    {
        if (activeMachine != null)
            activeMachine.SetActive(false);
        activeMachine = machine;
        activeMachine.SetActive(true);
    }
}
