using System.Threading.Tasks;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject box;

    public bool canSpawnBox = true;
    [SerializeField] float timer = 0f;
    [SerializeField] float maxTimer = 1.25f;

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

    private void OnEnable()
    {
        PlcConnectionManager.OnPLCConnectedRelease += OnPlcConnected;
    }
    private void OnDisable()
    {

        PlcConnectionManager.OnPLCConnectedRelease -= OnPlcConnected;
    }

    void OnPlcConnected()
    {
        canSpawnBox = PlcConnectionManager.InstanceManager.ReadVariableValue<bool>("DB1.DBX2.5");
    }
}
