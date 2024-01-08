using UnityEngine;

public class ActiveMachineManager : MonoBehaviour
{
    [SerializeField] MachineScriptable[] machines;
    GameObject activeMachine;
    // Start is called before the first frame update
    void Start()
    {
       activeMachine = machines[0].machineObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeMachine(MachineScriptable machine)
    {
        activeMachine.SetActive(false);
        machine.machineObj = activeMachine;
        activeMachine.SetActive(true);
    }
}
