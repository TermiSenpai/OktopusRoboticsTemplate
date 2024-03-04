using System.Collections;
using UnityEngine;

public class AutoMovementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Engine axisX;
    [SerializeField] Engine axisY;
    [SerializeField] Engine axisZ;
    [Header("Belt Objetive")]
    [SerializeField] float axisXBeltPos = -0.07428553f;
    [SerializeField] float axisYBeltPos = 0.7341763f;
    [SerializeField] float axisZBeltPos = -0.6578024f;
    [Header("Pallet Objetive")]
    [SerializeField] float axisXPalletPos = -0.1171427f;
    [SerializeField] float axisYPalletPos = 0.06351654f;
    [SerializeField] float axisZPalletPos = 0.4885714f;

    public void GoToBelt()
    {
        StartCoroutine(nameof(MoveToBelt));
    }

    public void GoToPallet()
    {
        StartCoroutine (nameof(MoveToPallet));
    }

    IEnumerator MoveToBelt()
    {
        while (true)
        {
            // Verificar si los ejes están en las posiciones objetivo
            bool axisXAtTarget = Mathf.Abs(axisX.GetObjectToMovePosition().x - axisXBeltPos) < 0.01f;
            bool axisYAtTarget = Mathf.Abs(axisY.GetObjectToMovePosition().y - axisYBeltPos) < 0.01f;
            bool axisZAtTarget = Mathf.Abs(axisZ.GetObjectToMovePosition().z - axisZBeltPos) < 0.01f;

            // Ajustar las variables booleanas en consecuencia

            axisX.debugR = !axisXAtTarget && axisX.GetObjectToMovePosition().x > axisXBeltPos;
            axisX.debugL = !axisXAtTarget && axisX.GetObjectToMovePosition().x < axisXBeltPos;

            axisY.debugR = !axisYAtTarget && axisY.GetObjectToMovePosition().y > axisYBeltPos;
            axisY.debugL = !axisYAtTarget && axisY.GetObjectToMovePosition().y < axisYBeltPos;

            axisZ.debugR = !axisZAtTarget && axisZ.GetObjectToMovePosition().z > axisZBeltPos;
            axisZ.debugL = !axisZAtTarget && axisZ.GetObjectToMovePosition().z < axisZBeltPos;

            // Salir del bucle si todas las posiciones son las correctas
            if (axisXAtTarget && axisYAtTarget && axisZAtTarget)
                break;

            yield return null;
        }
    }
    IEnumerator MoveToPallet()
    {
        while (true)
        {
            // Verificar si los ejes están en las posiciones objetivo
            bool axisXAtTarget = Mathf.Abs(axisX.GetObjectToMovePosition().x - axisXPalletPos) < 0.01f;
            bool axisYAtTarget = Mathf.Abs(axisY.GetObjectToMovePosition().y - axisYPalletPos) < 0.01f;
            bool axisZAtTarget = Mathf.Abs(axisZ.GetObjectToMovePosition().z - axisZPalletPos) < 0.01f;

            // Ajustar las variables booleanas en consecuencia

            axisX.debugR = !axisXAtTarget && axisX.GetObjectToMovePosition().x > axisXPalletPos;
            axisX.debugL = !axisXAtTarget && axisX.GetObjectToMovePosition().x < axisXPalletPos;

            axisY.debugR = !axisYAtTarget && axisY.GetObjectToMovePosition().y > axisYPalletPos;
            axisY.debugL = !axisYAtTarget && axisY.GetObjectToMovePosition().y < axisYPalletPos;

            axisZ.debugR = !axisZAtTarget && axisZ.GetObjectToMovePosition().z > axisZPalletPos;
            axisZ.debugL = !axisZAtTarget && axisZ.GetObjectToMovePosition().z < axisZPalletPos;

            // Salir del bucle si todas las posiciones son las correctas
            if (axisXAtTarget && axisYAtTarget && axisZAtTarget)
                break;

            yield return null;
        }
    }


}
