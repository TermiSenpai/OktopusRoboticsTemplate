using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVisibilityManager : MonoBehaviour
{
    [SerializeField, Tooltip("Tecla para alternar la visualización del ratón")] KeyCode mouseVisibilityKey;
    // Start is called before the first frame update

    // Delegado
    // Similar a un puntero en C++
    public delegate void MouseVisibility();
    public static event MouseVisibility MouseRelease;

    void Start()
    {
        // Inicia el programa con el cursor fijado
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Cada frame comprueba si se ha pulsado la tecla configurada en el inspector
        if (Input.GetKeyDown(mouseVisibilityKey))
        {
            // En caso de que exista algún método vinculado al delegado, se llama y se ejecuta ese método
            MouseRelease?.Invoke();

            // Modifica el modo del cursor actual según el estado, alternando entre fijo o sin limite
            switch (Cursor.lockState)
            {
                case CursorLockMode.Locked:
                    Cursor.lockState = CursorLockMode.None;
                    break;
                case CursorLockMode.None:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
            }
        }
    }
}
