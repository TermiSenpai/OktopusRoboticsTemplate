using UnityEngine;

// Enumeracion que representa los ejes de movimiento posibles
public enum AxisMovement
{
    X,
    Y,
    Z
}

public class Engine : MonoBehaviour
{
    // Codes associated with right and left movements (editable from the Inspector)
    [Header("PLC Codes")]
    [SerializeField] protected string rightCode;
    [SerializeField] protected string leftCode;
    [SerializeField] protected string positionCode;
    [SerializeField] protected string speedCode;
}
