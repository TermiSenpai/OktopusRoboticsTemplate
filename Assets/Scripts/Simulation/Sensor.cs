using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] string PLCCode;
    [SerializeField] float raycastDistance = 0.5f;

    private void Update()
    {
        // Obtener la posici�n y la direcci�n del eje y local del sensor
        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection = transform.up;

        // Lanzar el Raycast
        if (Physics.Raycast(raycastOrigin, raycastDirection, out RaycastHit hit, raycastDistance))
        {
            // El Raycast golpe� algo
            Debug.DrawRay(raycastOrigin, raycastDirection * hit.distance, Color.red);
            Debug.Log("Golpe�: " + hit.collider.gameObject.name);

            //TODO
            // Llamar al controlador del PLC para su activaci�n
            PLCControl.Instance.OnSensorDetection(PLCCode, true);
        }
        else
        {
            // El Raycast no golpe� nada
            Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.green);
            PLCControl.Instance.OnSensorDetection(PLCCode, false);
        }
    }
}
