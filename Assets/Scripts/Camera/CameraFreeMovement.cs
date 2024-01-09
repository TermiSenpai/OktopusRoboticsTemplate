using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float scrollWheelSpeed = 1f;
    private float currentMouseAxis;

    private void Update()
    {
        MoveCamera();
        RotateCamera();
        MovementSpeedModify();
    }

    private void MovementSpeedModify()
    {
        currentMouseAxis = Input.GetAxis("Mouse ScrollWheel");
        movementSpeed += currentMouseAxis * scrollWheelSpeed;
    }

    private bool CanRotate()
    {
        if (Input.GetKey(KeyCode.Mouse1))
            return true;
        return false;
    }

    private void RotateCamera()
    {
        if (CanRotate())
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float rotationX = mouseY * rotateSpeed * -1;
            float rotationY = mouseX * rotateSpeed ;

            //// Almacenar la rotación actual en un Quaternion
            //Quaternion currentRotation = transform.rotation;

            // Aplicar rotación al objeto
            transform.Rotate(rotationX, rotationY, 0);

            // Bloquear el eje Z
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.z = 0f;

            // Aplicar la rotación bloqueada al objeto
            transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }


    private void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 localDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Transforma la dirección local a dirección global
        Vector3 worldDirection = transform.TransformDirection(localDirection);

        transform.Translate(movementSpeed * Time.deltaTime * worldDirection, Space.World);
    }

}
