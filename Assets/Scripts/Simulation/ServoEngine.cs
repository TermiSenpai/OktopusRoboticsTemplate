using UnityEngine;

public class ServoEngine : MonoBehaviour
{
    [SerializeField] string rightCode;
    [SerializeField] string leftCode;
    [SerializeField] float speed;

    [SerializeField] Vector3 direction;
    [SerializeField] GameObject axis;

    private void Update()
    {
        if (!PLCConexion.plc.IsConnected) return;

       bool rightMove = (bool) PLCConexion.plc.Read(rightCode);
        if(rightMove)
        {
            axis.transform.localPosition += direction * speed;
        }
    }
}
