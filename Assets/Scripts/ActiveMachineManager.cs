using System.Collections.Generic;
using UnityEngine;

public class ActiveMachineManager : MonoBehaviour
{
    // Se almacena los datos del scriptable object en un array
    [SerializeField] MachineScriptable[] machines;

    // Se almacena la posici�n del gameobject padre
    Transform parent;

    // Se almacena una lista variable con los gameobjects para posterior modificaci�n
    readonly List<GameObject> currentSceneMachines = new();

    // Se almacena el gameobject activo
    GameObject activeMachine;
    // Se almacena el index actual para posterior modificaci�n
    int currentMachineIndex = 0;

    void Start()
    {
        // Obtiene el parent de todas las m�quinas
        parent = GameObject.FindGameObjectWithTag("MachinesParent").GetComponent<Transform>();

        // Crea las m�quinas en escena
        SpawnAllMachinesInScene();

        // Guarda y hace visible la primera m�quina
        ChangeMachine(currentSceneMachines[currentMachineIndex]);
    }

    public void SpawnAllMachinesInScene()
    {
        // Todos los gameobjects se almacenan en una lista para poder modificarlo despu�s
        foreach (var machine in machines)
            currentSceneMachines.Add(Instantiate(machine.machineObj, parent));

    }

    // Oculta la m�quina actual y hace visible una nueva
    public void ChangeMachine(GameObject machine)
    {
        if (activeMachine != null)
            activeMachine.SetActive(false);
        activeMachine = machine;
        activeMachine.SetActive(true);
    }

    // Aumenta en 1 el index actual
    public void OnAdvanceMachineBtn()
    {
        int newIndex = currentMachineIndex + 1;

        if (newIndex >= currentSceneMachines.Count)
            newIndex = 0;

        ChangeCurrentIndex(newIndex);
    }

    // Disminuye en 1 el index actual
    public void OnDecreaseMachineBtn()
    {
        int newIndex = currentMachineIndex - 1;

        if (newIndex < 0)
            newIndex = currentSceneMachines.Count - 1;

        ChangeCurrentIndex(newIndex);
    }

    // Actualiza la maquina y modifica el index actual
    private void ChangeCurrentIndex(int index)
    {
        ChangeMachine(currentSceneMachines[index]);
        currentMachineIndex = index;
    }
}
