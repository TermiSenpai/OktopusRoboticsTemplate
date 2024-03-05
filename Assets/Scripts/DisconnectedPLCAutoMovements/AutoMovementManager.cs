using System.Collections;
using UnityEngine;

public class AutoMovementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Engine axisX;
    [SerializeField] Engine axisY;
    [SerializeField] Engine axisZ;
    private BoxManager box;

    [Header("Coords")]
    [SerializeField] private Vector3 beltTarget = new(-0.07428553f, 0.7341763f, -0.6578024f);
    [SerializeField] private Vector3 takeBoxAndElevate = new(-0.07428553f, 1.189561f, 0.4916487f);
    [SerializeField] private Vector3[] palletPositions; // Array para almacenar las posiciones del pallet
    [SerializeField] int index;

    private void Awake()
    {
        box = FindObjectOfType<BoxManager>();
    }
    public void StartPaletizer()
    {
        StartCoroutine(StartSecuence());
    }
    IEnumerator StartSecuence()
    {
        // Go to belt
        yield return StartCoroutine(MoveToTarget(beltTarget));
        // Take box
        box.OnTakeBtn();
        // elevate axis
        yield return StartCoroutine(MoveToTarget(takeBoxAndElevate));
        // Go to pallet
        yield return StartCoroutine(MoveToTarget(palletPositions[index]));
        // Drop box
        box.OnDropBtn();
        //Elevate axis
        yield return StartCoroutine(MoveToTarget(takeBoxAndElevate));

        //Repeat
        if (index < palletPositions.Length - 1)
        {
            index++;
            yield return new WaitForEndOfFrame();
            StartCoroutine(StartSecuence());
        }


    }

    public void GoToBelt() => StartCoroutine(MoveToTarget(beltTarget));

    public void GoToPallet() // Cambiado para aceptar un índice como parámetro
    {
        if (index >= 0 && index < palletPositions.Length)
            StartCoroutine(MoveToTarget(palletPositions[index]));
    }

    public void TakeBoxAndElevate()
    {
        box.OnTakeBtn();
        StartCoroutine(MoveToTarget(takeBoxAndElevate));
    }

    public void DropBoxAndElevate()
    {
        box.OnDropBtn();
        StartCoroutine(MoveToTarget(takeBoxAndElevate));
        index++;
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        while (true)
        {
            bool axisXAtTarget = Mathf.Abs(axisX.GetObjectToMovePosition().x - target.x) < 0.01f;
            bool axisYAtTarget = Mathf.Abs(axisY.GetObjectToMovePosition().y - target.y) < 0.01f;
            bool axisZAtTarget = Mathf.Abs(axisZ.GetObjectToMovePosition().z - target.z) < 0.01f;

            axisX.debugR = !axisXAtTarget && axisX.GetObjectToMovePosition().x > target.x;
            axisX.debugL = !axisXAtTarget && axisX.GetObjectToMovePosition().x < target.x;

            axisY.debugR = !axisYAtTarget && axisY.GetObjectToMovePosition().y > target.y;
            axisY.debugL = !axisYAtTarget && axisY.GetObjectToMovePosition().y < target.y;

            axisZ.debugR = !axisZAtTarget && axisZ.GetObjectToMovePosition().z > target.z;
            axisZ.debugL = !axisZAtTarget && axisZ.GetObjectToMovePosition().z < target.z;

            if (axisXAtTarget && axisYAtTarget && axisZAtTarget)
                break;

            yield return null;
        }
    }
}
