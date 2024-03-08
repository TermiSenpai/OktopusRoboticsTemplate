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

    public void SpawnBox()
    {
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
