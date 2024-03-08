using System.Collections;
using Unity.Collections;
using UnityEngine;

public class AutomaticSequenceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Engine axisX;
    [SerializeField] Engine axisY;
    [SerializeField] Engine axisZ;
    private BoxManager box;

    [Header("Coords")]
    [SerializeField] private Vector3 beltTarget = new(-0.07428553f, 0.7341763f, -0.6578024f);
    [SerializeField] private Vector3 upperBelt = new(-0.07428553f, 1.189561f, 0.4916487f);
    [SerializeField] private Vector3[] palletPositions; // Array to store pallet positions
    private Vector3[] upperPallet; // Array to store upper pallet positions
    [SerializeField] int index;

    [SerializeField] private float sequenceTime;
    private float totalSequenceTime;
    private float sequencesPerMinute;

    private bool activeProcess;
    private int frames;
    [Header("Average Speed")]
    [SerializeField] float averageSpeed;
    float averageXSpeed;
    float averageYSpeed;
    float averageZSpeed;

    public float AverageSpeed { get => averageSpeed; set => averageSpeed = value; }
    public float TotalSequenceTime { get => totalSequenceTime; set => totalSequenceTime = value; }
    public float SequencesPerMinute { get => sequencesPerMinute; set => sequencesPerMinute = value; }

    private void Awake() => box = FindObjectOfType<BoxManager>(); // Finding the BoxManager component in the scene and assigning it to the box variable
    private void Start()
    {
        // Initialize upperPallet array with the same length as palletPositions
        upperPallet = new Vector3[palletPositions.Length];

        // Fill upperPallet array with positions having Y coordinate of 1
        for (int i = 0; i < palletPositions.Length; i++)
        {
            upperPallet[i] = new Vector3(palletPositions[i].x, 1f, palletPositions[i].z);
        }
    }
    private void Update()
    {
        if (!activeProcess) return;

        sequenceTime += Time.deltaTime;
        TotalSequenceTime += Time.deltaTime;
        frames++;
        CalculateAverageSpeed();
    }
    public void StartPaletizer()
    {
        activeProcess = true;
        TotalSequenceTime = 0;
        StartCoroutine(StartSequence()); // Initiating the StartSequence coroutine
    }

    IEnumerator StartSequence()
    {
        sequenceTime = 0;
        ChangeSpeed(0.007f);
        // Go to belt
        yield return StartCoroutine(MoveToTarget(beltTarget)); // Initiating a coroutine to move to the belt position

        // Take box
        box.OnTakeBtn(); // Calling the OnTakeBtn method of the box

        // Elevate axis
        yield return StartCoroutine(MoveToTarget(upperBelt)); // Initiating a coroutine to move to the position to take and elevate the box
        yield return StartCoroutine(MoveToTarget(upperPallet[index]));

        ChangeSpeed(0.003f);
        // Go to pallet
        yield return StartCoroutine(MoveToTarget(palletPositions[index])); // Initiating a coroutine to move to the current pallet position

        // Drop box
        box.OnDropBtn(); // Calling the OnDropBtn method of the box

        ChangeSpeed(0.007f);
        // Elevate axis
        yield return StartCoroutine(MoveToTarget(upperPallet[index]));
        //yield return StartCoroutine(MoveToTarget(takeBoxAndElevate)); // Initiating a coroutine to move to the position to elevate the box again

        CalculateTime();

        // Repeat
        if (index < palletPositions.Length - 1) // Checking if there are more pallet positions to visit
        {
            index++; // Incrementing the index to move to the next pallet position
            yield return new WaitForEndOfFrame(); // Waiting until the end of the current frame
            StartCoroutine(StartSequence()); // Initiating the StartSequence coroutine again for the next pallet position
        }
        else
            activeProcess = false;
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        while (true)
        {
            // Checking if each axis is at the target position within a small threshold
            bool axisXAtTarget = Mathf.Abs(axisX.GetObjectToMovePosition().x - target.x) < axisX.Speed;
            bool axisYAtTarget = Mathf.Abs(axisY.GetObjectToMovePosition().y - target.y) < axisY.Speed;
            bool axisZAtTarget = Mathf.Abs(axisZ.GetObjectToMovePosition().z - target.z) < axisZ.Speed;

            // Setting debug variables for each axis based on their current position relative to the target position
            axisX.debugR = !axisXAtTarget && axisX.GetObjectToMovePosition().x > target.x;
            axisX.DebugL = !axisXAtTarget && axisX.GetObjectToMovePosition().x < target.x;

            axisY.debugR = !axisYAtTarget && axisY.GetObjectToMovePosition().y > target.y;
            axisY.DebugL = !axisYAtTarget && axisY.GetObjectToMovePosition().y < target.y;

            axisZ.debugR = !axisZAtTarget && axisZ.GetObjectToMovePosition().z > target.z;
            axisZ.DebugL = !axisZAtTarget && axisZ.GetObjectToMovePosition().z < target.z;

            // Exiting the loop if all axes are at their target positions
            if (axisXAtTarget && axisYAtTarget && axisZAtTarget)
                break;

            yield return new WaitForEndOfFrame(); // Waiting for the next frame
        }
    }

    // Definition of the ChangeSpeed function that changes the speed of the axes
    void ChangeSpeed(float value)
    {
        // Assigns the provided speed value to the X, Y, and Z axes
        axisX.Speed = value;
        axisY.Speed = value;
        axisZ.Speed = value;
    }

    // Definition of the CalculateTime function that calculates the number of sequences per minute
    void CalculateTime() => SequencesPerMinute = 60 / sequenceTime;

    // Definition of the CalculateAverageSpeed function that calculates the average speed
    void CalculateAverageSpeed()
    {
        // Accumulates the speed of each axis divided by the number of frames into the respective average speed
        averageXSpeed += axisX.Speed / frames;
        averageYSpeed += axisY.Speed / frames;
        averageZSpeed += axisZ.Speed / frames;

        // Calculates the overall average speed by taking the sum of the average speeds of the three axes and dividing it by 3
        AverageSpeed = (averageXSpeed + averageYSpeed + averageZSpeed) / 3;
    }



}
