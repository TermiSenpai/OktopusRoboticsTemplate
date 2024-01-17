using UnityEngine;

public class UIItem : MonoBehaviour
{
    // Referencia al contenedor
    [SerializeField] private RectTransform panel;
    // Tama�o del contenedor (A mano)
    [SerializeField] float panelHeight;

    // Cuando se activa, cambia el tama�o del panel
    private void OnEnable() => ChangePanelHeight(panelHeight);

    // Solo es necesario cambiar la altura del contenedor, por lo que el ancho ser� el mismo
    public void ChangePanelHeight(float height) => panel.sizeDelta = new Vector2(panel.sizeDelta.x, height);

}
