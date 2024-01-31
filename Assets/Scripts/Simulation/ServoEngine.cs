using UnityEngine;

public enum AxisMovement
{
    X,
    Y,
    Z
}

public class ServoEngine : MonoBehaviour
{
    [SerializeField] string rightCode;
    [SerializeField] string leftCode;
    [SerializeField] float speed;

    [SerializeField] Vector3 direction;
    [SerializeField] GameObject axis;
    [SerializeField] bool debugR;
    [SerializeField] bool debugL;

    [SerializeField] float posMin;
    [SerializeField] float posMax;

    [SerializeField] AxisMovement axisToLimit;
    private void Update()
    {

        // Debug
        if (debugR)
            axis.transform.localPosition += direction * speed;

        if (debugL)
            axis.transform.localPosition -= direction * speed;

        Vector3 currentPosition = axis.transform.localPosition;
        float clampedValue;

        switch (axisToLimit)
        {
            case AxisMovement.X:
                clampedValue = Mathf.Clamp(currentPosition.x, posMin, posMax);
                Debug.Log(clampedValue);
                axis.transform.localPosition = new Vector3(clampedValue, currentPosition.y, currentPosition.z);
                break;
            case AxisMovement.Y:
                clampedValue = Mathf.Clamp(currentPosition.y, posMin, posMax);
                Debug.Log(clampedValue);
                axis.transform.localPosition = new Vector3(currentPosition.x, clampedValue, currentPosition.z);
                break;
            case AxisMovement.Z:
                clampedValue = Mathf.Clamp(currentPosition.z, posMin, posMax);
                Debug.Log(clampedValue);
                axis.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, clampedValue);
                break;
        }

        if (PLCConexion.plc == null || !PLCConexion.plc.IsConnected) return;

       bool rightMove = (bool) PLCConexion.plc.Read(rightCode);
        if(rightMove)
        {
            axis.transform.localPosition += direction * speed;
        }
       bool leftMove = (bool) PLCConexion.plc.Read(leftCode);
        if(rightMove)
        {
            axis.transform.localPosition += direction * speed;
        }
        

        

    }
}
