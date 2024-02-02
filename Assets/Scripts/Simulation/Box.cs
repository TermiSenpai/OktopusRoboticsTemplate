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
            //collision.gameObject.TryGetComponent<ConveyorBelt>(out var belt);
            //if (currentSpeed == belt.GetSpeed())
            //    return;

            currentSpeed = belt.GetSpeed();
        }
    }

    private void Update()
    {
        // Mover el GameObject en la direcci?n positiva del eje X
        MoveGameObject();
    }

    private void MoveGameObject()
    {
        // Obtener la posici?n actual
        Vector3 currentPosition = transform.position;

        // Calcular la nueva posici?n en funci?n de la velocidad y el tiempo
        float newXPosition = currentPosition.x + currentSpeed * Time.deltaTime;

        // Asignar la nueva posici?n al GameObject
        transform.position = new Vector3(newXPosition, currentPosition.y, currentPosition.z);
    }
}
