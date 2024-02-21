using UnityEngine;

public class PaletizerAxisMovement : MonoBehaviour
{
    public static PaletizerAxisMovement Instance;

    public static Transform xAxis;
    Transform yAxis;
    Transform zAxis;

    private void Awake()
    {
        Instance = this;
        xAxis = GameObject.FindGameObjectWithTag("X").transform;
        yAxis = GameObject.FindGameObjectWithTag("Y").transform;
        zAxis = GameObject.FindGameObjectWithTag("Z").transform;
    }

    public void OnSliderX(float value)
    {
        xAxis.localPosition = new Vector3(value, xAxis.localPosition.y, xAxis.localPosition.z);
    }
    public void OnSliderY(float value)
    {
        yAxis.localPosition = new Vector3(yAxis.localPosition.x, value, yAxis.localPosition.z);
    }
    public void OnSliderZ(float value)
    {
        zAxis.localPosition = new Vector3(zAxis.localPosition.x, zAxis.localPosition.y, value);
    }

    public float GetAxisX()
    {
        return xAxis.localPosition.x;
    }
    public float GetAxisY()
    {
        return yAxis.localPosition.y;
    }
    public float GetAxisZ()
    {
        return zAxis.localPosition.z;
    }

}
