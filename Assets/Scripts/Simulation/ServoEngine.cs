using UnityEngine;

public class ServoEngine : MonoBehaviour
{
    [SerializeField] string rightCode;
    [SerializeField] string leftCode;
    [SerializeField] float speed;

    [SerializeField] Vector3 direction;
    [SerializeField] GameObject axis;
    [SerializeField] bool debugR;
    [SerializeField] bool debugL;
    private void Update()
    {

        // Debug
        if (debugR)
            axis.transform.localPosition += direction * speed;

        if (debugL)
            axis.transform.localPosition -= direction * speed;

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
