using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMachineManager : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] MachineScriptable[] machines;
    [SerializeField] CinemachineFreeLook cameraData;
    [SerializeField] float wheelSpeed;
    List<GameObject> currentSceneMachines = new List<GameObject>();
    GameObject activeMachine;
    float currentMouseAxis;
    // Start is called before the first frame update
    void Start()
    {
        SpawnAllMachinesInScene();
        changeMachine(currentSceneMachines[0]);
    }

    // Update is called once per frame
    void Update()
    {
        currentMouseAxis = Input.GetAxis("Mouse ScrollWheel") * -1;
        CinemachineFreeLook.Orbit[] orbits = cameraData.m_Orbits;
        for (int i = 0; i < orbits.Length; i++)
        {
            orbits[i].m_Radius += currentMouseAxis * wheelSpeed;
        }
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
