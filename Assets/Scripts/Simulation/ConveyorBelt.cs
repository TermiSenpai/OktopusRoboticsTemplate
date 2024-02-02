using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float currentSpeed;
    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }

    private void Update()
    {
        
    }

    public void GetSensorState(bool sensorState)
    {
        switch (sensorState)
        {
            case true:
                SetSpeed(0f);
                break; 
            
            case false:
                SetSpeed(0.5f);
                break;
        }
    }
}
