using Cinemachine;
using System;
using UnityEngine;

public class CameraFreeMovement : MonoBehaviour
{
    // Velocidad de movimiento
    [SerializeField, Range(0.1f, 10f)] float movementSpeed = 5f;
    // Velocidad de rotaci�n de la c�mara
    [SerializeField, Range(0.1f, 10f)] float rotateSpeed = 2f;
    // Velocidad de multiplicador con la rueda del raton
    [SerializeField, Range(0.1f, 10f)] float scrollWheelSpeed = 2.5f;
    // Movimiento actual del rat�n
    private float currentMouseAxis;
    // Referencia a la c�mara virutal de cinemachine
    private CinemachineVirtualCamera virtualCamera;



    // Obtener referencia de virtualCamera
    private void Start() => virtualCamera = GetComponent<CinemachineVirtualCamera>();

    private void Update()
    {
        // Permite mover la camara
        MoveCamera();
        // Permite rotar la camara
        RotateCamera();
        // Permite modificar la velocidad de movimiento de la camara
        MovementSpeedModify();
    }

    private void OnEnable() => MouseVisibilityManager.MouseRelease += AlternateCinemachine;

    private void OnDisable() => MouseVisibilityManager.MouseRelease -= AlternateCinemachine;

    private void AlternateCinemachine() => virtualCamera.enabled = !virtualCamera.enabled;

    private void MovementSpeedModify()
    {
        // Obtiene el movimiento de la rueda del rat�n
        currentMouseAxis = Input.GetAxis("Mouse ScrollWheel");
        // Suma el valor del input multiplicado por la velocidad
        // Si el valor del input es negativo, se restar� (X + (-y))
        movementSpeed += currentMouseAxis * scrollWheelSpeed;
    }

    private bool CanRotate()
    {
        // Si el cinemachine est� desactivado, se cancela
        if (!virtualCamera.enabled) return false;

        // Chequea si se est� pulsando el click derecho
        if (Input.GetKey(KeyCode.Mouse1))
            return true;
        return false;
    }

    private void RotateCamera()
    {
        // Chequea si se puede rotar
        if (CanRotate())
        {
            // Obtiene el input del rat�n, separado por axis
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // // Calcula la rotaci�n en el eje X basada en la entrada vertical del rat�n y se invierte
            float rotationX = mouseY * rotateSpeed * -1;
            // Calcula la rotaci�n en el eje Y basada en la entrada horizontal del rat�n
            float rotationY = mouseX * rotateSpeed;

            // Aplicar rotaci�n al objeto
            transform.Rotate(rotationX, rotationY, 0);

            // Bloquear el eje Z
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.z = 0f;

            // Aplicar la rotaci�n bloqueada al objeto
            transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }

    private void MoveCamera()
    {
        // Si el cinemachine est� desactivado, se cancela
        if (!virtualCamera.enabled) return;

        // Obtiene los inputs del movimiento del rat�n
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Crea un vector3 con la entrada horizontal en el eje X, el eje vertical 0 y el Z con el vertical
        // Todo normalizado para que la velocidad sea igual en todas direcciones
        Vector3 localDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Transforma la direcci�n local a direcci�n global
        // Para moverse en la direcci�n que se est� observando
        Vector3 worldDirection = transform.TransformDirection(localDirection);

        // Se aplica el movimiento
        transform.Translate(movementSpeed * Time.deltaTime * worldDirection, Space.World);
    }



}
