using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVisibilityManager : MonoBehaviour
{
    [SerializeField, Tooltip("Tecla para alternar la visualización del ratón")] KeyCode mouseVisibilityKey;
    // Start is called before the first frame update

    public delegate void MouseVisibility();
    public static event MouseVisibility MouseRelease;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(mouseVisibilityKey))
        {
            MouseRelease?.Invoke();
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
