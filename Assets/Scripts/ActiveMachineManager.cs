using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMachineManager : MonoBehaviour
{
    [SerializeField] MachineScriptable[] machines;

    Transform parent;
    List<GameObject> currentSceneMachines = new();
    GameObject activeMachine;
    int currentMachineIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("MachinesParent").GetComponent<Transform>();
        SpawnAllMachinesInScene();
        ChangeMachine(currentSceneMachines[currentMachineIndex]);
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

    public void OnAdvanceMachineBtn()
    {
        int newIndex = currentMachineIndex + 1;

        if (newIndex >= currentSceneMachines.Count)
            newIndex = 0;

        ChangeCurrentIndex(newIndex);
    }

    public void OnDecreaseMachineBtn()
    {
        int newIndex = currentMachineIndex - 1;

        if (newIndex < 0)
            newIndex = currentSceneMachines.Count - 1;

        ChangeCurrentIndex(newIndex);
    }

    private void ChangeCurrentIndex(int index)
    {
        ChangeMachine(currentSceneMachines[index]);
        currentMachineIndex = index;
    }
}
