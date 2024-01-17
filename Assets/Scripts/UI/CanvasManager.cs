using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Referencia al canvas
    [SerializeField] private GameObject UI;

    // Iniciar con el canvas desactivado
    private void Awake() => UI.SetActive(false);

    // Al activarse o iniciar, se suscribe al delegado para recibir el momento donde el ratón se hace visible o invisible
    private void OnEnable() => MouseVisibilityManager.MouseRelease += ToggleUI;

    // Si este gameobject se desactiva, se desuscribe al delegado para evitar problemas
    private void OnDisable() => MouseVisibilityManager.MouseRelease -= ToggleUI;

    // Alterna el estado de visibilidad del canvas
    private void ToggleUI() => UI.SetActive(!UI.activeInHierarchy);

}
