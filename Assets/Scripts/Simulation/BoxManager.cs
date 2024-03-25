using TMPro;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public Transform boxParent; // Referencia al objeto padre
    public LayerMask targetLayer; // Capa del objeto que deseas mover
    public GameObject currentBox;
    public float raycastDistance = 1f;

    [SerializeField] Transform origin;
    Vector3 dir = Vector3.down;


    private void Update()
    {
        if (currentBox != null)
            return;

        Ray ray = new(origin.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, targetLayer))
        {
            Debug.Log("Golpeó: " + hit.collider.name);
            Debug.DrawRay(origin.position, dir * hit.distance, Color.red);
        }
        else Debug.DrawRay(origin.position, dir * hit.distance, Color.green);
    }

    public void OnTakeBtn()
    {
        Ray ray = new(origin.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, targetLayer))
        {
            Debug.Log("Golpeó: " + hit.collider.name);
            Debug.DrawRay(origin.position, dir * hit.distance, Color.red);
            currentBox = hit.collider.gameObject;

            currentBox.transform.parent = boxParent;
            currentBox.GetComponent<Rigidbody>().isKinematic = true;
            currentBox.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    public void OnTakeBtn(string code)
    {
        Ray ray = new(origin.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, targetLayer))
        {
            currentBox = hit.collider.gameObject;

            currentBox.transform.parent = boxParent;
            currentBox.GetComponent<Rigidbody>().isKinematic = true;
            currentBox.GetComponent<Rigidbody>().useGravity = false;
            PlcConnectionManager.InstanceManager.WriteVariableValue(code, true);
        }
    }

    public void OnDropBtn()
    {
        if (currentBox != null)
        {
            currentBox.transform.parent = null;
            currentBox.GetComponent<Rigidbody>().useGravity = true;
            currentBox.GetComponent<Rigidbody>().isKinematic = false;
            currentBox = null;
            return;
        }
    }


}
