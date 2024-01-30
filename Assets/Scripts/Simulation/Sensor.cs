using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] string PLCCode;
    [SerializeField] float raycastDistance = 0.5f;

    private void Update()
    {
        // Obtener la posición y la dirección del eje y local del sensor
        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection = transform.up;

        // Lanzar el Raycast
        if (Physics.Raycast(raycastOrigin, raycastDirection, out RaycastHit hit, raycastDistance))
        {
            // El Raycast golpeó algo
            Debug.DrawRay(raycastOrigin, raycastDirection * hit.distance, Color.red);
            Debug.Log("Golpeó: " + hit.collider.gameObject.name);

            //TODO
            // Llamar al controlador del PLC para su activación
            PLCControl.Instance.OnSensorDetection(PLCCode, true);
        }
        else
        {
            // El Raycast no golpeó nada
            Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.green);
            PLCControl.Instance.OnSensorDetection(PLCCode, false);
        }
    }
}
