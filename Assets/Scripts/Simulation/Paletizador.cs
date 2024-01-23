using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paletizador : MonoBehaviour
{
    public float velocidadMovimiento = 2.0f;
    public float velocidadMovimientoRaton = 10.0f;

    public LayerMask capaObjeto;
    public Transform sensor;
    public bool sensorDetected;

    public Transform ejeZ;
    public Transform ejeX;
    public Transform ejeY;
    [Header("Eje X")]
    public float limiteXMin = -0.8f;
    public float limiteXMax = 0.5f;
    [Header("Eje Y")]
    public float limiteYMin = 0.0f;
    public float limiteYMax = 1.7f;
    [Header("Eje Z")]
    public float limiteZMin = -0.7f;
    public float limiteZMax = 0.9f;

    void Update()
    {
        // Mover el objetoEjeX
        float movimientoX = Input.GetAxis("Horizontal");
        MoverEjeX(movimientoX);

        // Mover el ejeY localmente con respecto al ejeX
        float movimientoY = Input.GetAxis("Vertical");
        MoverEjeY(movimientoY);

        // Mover el objetoEjeZ
        float movimientoZ = Input.GetAxis("Mouse ScrollWheel");
        MoverEjeZ(movimientoZ);

        SensorDetector();
    }

    void SensorDetector()
    {
        if(Physics.Raycast(sensor.position, sensor.forward, out RaycastHit hit, Mathf.Infinity, capaObjeto))
        {
            sensorDetected = true;
            Debug.Log("Detección");
        }
        else
        {
            sensorDetected = false;
            Debug.Log("Nada");
        }
    }

    void MoverEjeX(float inputMovimiento)
    {
        // Calcular el cambio en la posición local basado en la entrada del teclado
        float deltaMovimiento = inputMovimiento * velocidadMovimiento * Time.deltaTime;

        // Calcular la nueva posición local limitada
        float nuevaPosicionX = Mathf.Clamp(ejeX.localPosition.x + deltaMovimiento, limiteXMin, limiteXMax);

        // Aplicar el cambio de posición local al objeto
        ejeX.localPosition = new Vector3(nuevaPosicionX, ejeX.localPosition.y, ejeX.localPosition.z);
    }

    void MoverEjeY(float inputMovimiento)
    {
        // Calcular el cambio en la posición local basado en la entrada del teclado
        float deltaMovimiento = inputMovimiento * velocidadMovimiento * Time.deltaTime;

        // Calcular la nueva posición local limitada
        float nuevaPosicionY = Mathf.Clamp(ejeY.localPosition.y + deltaMovimiento, limiteYMin, limiteYMax);

        // Aplicar el cambio de posición local al objeto
        ejeY.localPosition = new Vector3(ejeY.localPosition.x, nuevaPosicionY, ejeY.localPosition.z);
    }

    void MoverEjeZ(float inputMovimiento)
    {
        // Calcular el cambio en la posición local basado en la entrada del teclado
        float deltaMovimiento = inputMovimiento * velocidadMovimientoRaton * Time.deltaTime;

        // Calcular la nueva posición local limitada
        float nuevaPosicionZ = Mathf.Clamp(ejeZ.localPosition.z + deltaMovimiento, limiteZMin, limiteZMax);

        // Aplicar el cambio de posición local al objeto
        ejeZ.localPosition = new Vector3(ejeZ.localPosition.x, ejeZ.localPosition.y, nuevaPosicionZ);
    }

    void MoverEjeZHaciaObjeto()
    {
        // Información de la colisión

        // Lanzar un rayo hacia adelante desde la posición del ejeZ
        if (Physics.Raycast(ejeZ.position, ejeZ.forward, out RaycastHit hit, Mathf.Infinity, capaObjeto))
        {
            // Calcular la posición del objeto detectado
            Vector3 posicionObjeto = hit.point;

            // Mover el ejeZ hacia la posición del objeto detectado
            ejeZ.position = new Vector3(
                Mathf.Clamp(posicionObjeto.x, limiteXMin, limiteXMax),
                Mathf.Clamp(posicionObjeto.y, limiteYMin, limiteYMax),
                Mathf.Clamp(posicionObjeto.z, limiteZMin, limiteZMax)
            );
        }
    }
}

