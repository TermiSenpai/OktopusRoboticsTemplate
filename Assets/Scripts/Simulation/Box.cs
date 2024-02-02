using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private float currentSpeed = 0f;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Belt"))
        {

            collision.gameObject.TryGetComponent<ConveyorBelt>(out var belt);
            currentSpeed = belt.GetSpeed();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Belt"))
        {
            collision.gameObject.TryGetComponent<ConveyorBelt>(out var belt);
            if (currentSpeed == belt.GetSpeed())
                return;

            currentSpeed = belt.GetSpeed();
        }
    }
}
