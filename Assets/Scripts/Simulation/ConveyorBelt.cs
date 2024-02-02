using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    private float currentSpeed;

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }
}
