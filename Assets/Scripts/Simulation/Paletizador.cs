using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Axis
{
    x, y, z
}

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
        //MoverEjeX(movimientoX);
        MoverEje(ejeX, movimientoX, velocidadMovimiento, limiteXMin, limiteXMax, Axis.x);

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
        if (Physics.Raycast(sensor.position, sensor.forward, out RaycastHit hit, Mathf.Infinity, capaObjeto))
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

    void MoverEje(Transform eje, float input, float speed, float min, float max, Axis type)
    {
        float deltaMove = input * speed * Time.deltaTime;

        switch (type)
        {
            case Axis.x:
                float newPosX = Mathf.Clamp(eje.localPosition.x + deltaMove, min, max);
                eje.localPosition = new Vector3(newPosX, eje.localPosition.y, eje.localPosition.z);
                break;
            case Axis.y:
                float newPosY = Mathf.Clamp(eje.localPosition.x + deltaMove, min, max);
                eje.localPosition = new Vector3(eje.localPosition.x, newPosY, eje.localPosition.z);
                break;
            case Axis.z:
                float newPosZ = Mathf.Clamp(eje.localPosition.x + deltaMove, min, max);
                eje.localPosition = new Vector3(eje.localPosition.x, eje.localPosition.z, newPosZ);
                break;
        }
    }

    //void MoverEjeX(float inputMovimiento)
    //{
    //    // Calcular el cambio en la posición local basado en la entrada del teclado
    //    float deltaMovimiento = inputMovimiento * velocidadMovimiento * Time.deltaTime;

    //    // Calcular la nueva posición local limitada
    //    float nuevaPosicionX = Mathf.Clamp(ejeX.localPosition.x + deltaMovimiento, limiteXMin, limiteXMax);

    //    // Aplicar el cambio de posición local al objeto
    //    ejeX.localPosition = new Vector3(nuevaPosicionX, ejeX.localPosition.y, ejeX.localPosition.z);
    //}

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
}

