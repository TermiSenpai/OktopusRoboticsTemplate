using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject box;

    public bool canSpawnBox = true;
    [SerializeField] float timer = 0f;
    [SerializeField] float maxTimer = 1.25f;

    [SerializeField] string beltCode = "DB1.DBX2.5";

    private void Update()
    {
        if (canSpawnBox)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                timer = maxTimer;
                SpawnBox();
            }
        }
    }

    async Task ReadBeltState()
    {
        // Asynchronously read the values of PLC variables
        Task<bool> conveyorState = PlcConnectionManager.InstanceManager.ReadVariableAsync<bool>(beltCode);
        // Wait for all tasks to complete
        await Task.WhenAll(conveyorState);

        canSpawnBox = conveyorState.Result;
    }

    public void SpawnBox() => Instantiate(box, transform.position, Quaternion.identity);
    public void GetSensorState(bool sensorState)
    {
        switch (sensorState)
        {
            case true:
                canSpawnBox = !sensorState;
                break;

            case false:
                canSpawnBox = !sensorState;
                break;
        }
    }

    private void OnEnable() => PlcConnectionManager.OnPLCConnectedRelease += OnPlcConnected;
    private void OnDisable() => PlcConnectionManager.OnPLCConnectedRelease -= OnPlcConnected;

    void OnPlcConnected() => StartCoroutine(PLCConnectionStatusReaderCoroutine());

    IEnumerator PLCConnectionStatusReaderCoroutine()
    {

        // Continue looping while the PLC is connected
        while (!PlcConnectionManager.InstanceManager.IsPLCDisconnected())
        {
            // Trigger an asynchronous read of PLC values and wait for a specified amount of time
            _ = ReadBeltState();
            yield return new WaitForEndOfFrame();
        }

    }
}
