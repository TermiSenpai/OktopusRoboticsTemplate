using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private float currentSpeed = 0f;
    ConveyorBelt belt;
    bool isInBelt = false;

    private void Start()
    {
        belt = GameObject.FindGameObjectWithTag("Belt").GetComponent<ConveyorBelt>();
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Belt"))
            isInBelt = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Belt"))
            isInBelt = false;
    }

    private void Update()
    {
        if (!isInBelt) return;

        // Mover el GameObject en la dirección positiva del eje X
        MoveGameObject();

        currentSpeed = belt.GetSpeed();
    }

    private void MoveGameObject()
    {
        // Obtener la posición actual
        Vector3 currentPosition = transform.position;

        // Calcular la nueva posición en función de la velocidad y el tiempo
        float newXPosition = currentPosition.x + currentSpeed * Time.deltaTime;

        // Asignar la nueva posición al GameObject
        transform.position = new Vector3(newXPosition, currentPosition.y, currentPosition.z);
    }
}
