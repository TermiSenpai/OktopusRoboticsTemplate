using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1;
    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal, 0f, vertical);
        dir = transform.TransformDirection(dir);

        transform.Translate(dir * movementSpeed * Time.deltaTime);
    }
}
