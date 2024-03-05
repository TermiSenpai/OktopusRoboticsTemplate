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
    [SerializeField] private Vector3[] palletPositions; // Array to store pallet positions
    [SerializeField] int index;

    private void Awake() => box = FindObjectOfType<BoxManager>(); // Finding the BoxManager component in the scene and assigning it to the box variable

    public void StartPaletizer() => StartCoroutine(StartSequence()); // Initiating the StartSequence coroutine

    IEnumerator StartSequence()
    {
        // Go to belt
        yield return StartCoroutine(MoveToTarget(beltTarget)); // Initiating a coroutine to move to the belt position

        // Take box
        box.OnTakeBtn(); // Calling the OnTakeBtn method of the box

        // Elevate axis
        yield return StartCoroutine(MoveToTarget(takeBoxAndElevate)); // Initiating a coroutine to move to the position to take and elevate the box

        // Go to pallet
        yield return StartCoroutine(MoveToTarget(palletPositions[index])); // Initiating a coroutine to move to the current pallet position

        // Drop box
        box.OnDropBtn(); // Calling the OnDropBtn method of the box

        // Elevate axis
        yield return StartCoroutine(MoveToTarget(takeBoxAndElevate)); // Initiating a coroutine to move to the position to elevate the box again

        // Repeat
        if (index < palletPositions.Length - 1) // Checking if there are more pallet positions to visit
        {
            index++; // Incrementing the index to move to the next pallet position
            yield return new WaitForEndOfFrame(); // Waiting until the end of the current frame
            StartCoroutine(StartSequence()); // Initiating the StartSequence coroutine again for the next pallet position
        }
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        while (true)
        {
            // Checking if each axis is at the target position within a small threshold
            bool axisXAtTarget = Mathf.Abs(axisX.GetObjectToMovePosition().x - target.x) < 0.01f;
            bool axisYAtTarget = Mathf.Abs(axisY.GetObjectToMovePosition().y - target.y) < 0.01f;
            bool axisZAtTarget = Mathf.Abs(axisZ.GetObjectToMovePosition().z - target.z) < 0.01f;

            // Setting debug variables for each axis based on their current position relative to the target position
            axisX.debugR = !axisXAtTarget && axisX.GetObjectToMovePosition().x > target.x;
            axisX.debugL = !axisXAtTarget && axisX.GetObjectToMovePosition().x < target.x;

            axisY.debugR = !axisYAtTarget && axisY.GetObjectToMovePosition().y > target.y;
            axisY.debugL = !axisYAtTarget && axisY.GetObjectToMovePosition().y < target.y;

            axisZ.debugR = !axisZAtTarget && axisZ.GetObjectToMovePosition().z > target.z;
            axisZ.debugL = !axisZAtTarget && axisZ.GetObjectToMovePosition().z < target.z;

            // Exiting the loop if all axes are at their target positions
            if (axisXAtTarget && axisYAtTarget && axisZAtTarget)
                break;

            yield return new WaitForEndOfFrame(); // Waiting for the next frame
        }
    }
}
