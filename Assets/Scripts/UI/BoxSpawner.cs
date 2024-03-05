using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject box;

    public bool canSpawnBox = true;

    private void Start()
    {
        InvokeRepeating(nameof(OnBoxSpawnerBtn), 0, 1);
    }

    public void OnBoxSpawnerBtn()
    {
        if (canSpawnBox)
            Instantiate(box, transform.position, Quaternion.identity);
    }
    public void GetSensorState(bool sensorState)
    {

        switch (sensorState)
        {
            // Si el estado del sensor es true, detener la cinta (velocidad = 0)
            case true:
                canSpawnBox = !sensorState;
                break;

            // Si el estado del sensor es false, establecer una velocidad predeterminada (0.5)
            case false:
                canSpawnBox = !sensorState;
                break;
        }
    }
}
