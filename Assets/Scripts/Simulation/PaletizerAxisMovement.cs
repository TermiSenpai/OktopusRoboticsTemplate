using UnityEngine;

public class PaletizerAxisMovement : MonoBehaviour
{
    Transform xAxis;
    Transform yAxis;
    Transform zAxis;

    private void Start()
    {
        xAxis = GameObject.FindGameObjectWithTag("X").transform;
        yAxis = GameObject.FindGameObjectWithTag("Y").transform;
        zAxis = GameObject.FindGameObjectWithTag("Z").transform;
    }

    public void OnSliderX(float value)
    {
        xAxis.position = new Vector3(value, xAxis.position.y, xAxis.position.z);
    }
    public void OnSliderY(float value)
    {
        yAxis.position = new Vector3(yAxis.position.x, value, yAxis.position.z);
    }
    public void OnSliderZ(float value)
    {
        xAxis.position = new Vector3(zAxis.position.x, zAxis.position.y, value);
    }


}
