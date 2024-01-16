using UnityEngine;
using UnityEngine.UI;

public class GeneralLightManager : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private RectTransform panelRect;
    private void Awake()
    {
        UI.SetActive(false);
    }

    private void OnEnable()
    {
        MouseVisibilityManager.MouseRelease += ToggleUI;
    }

    private void OnDisable()
    {
        MouseVisibilityManager.MouseRelease -= ToggleUI;
    }

    private void ToggleUI()
    {
        UI.gameObject.SetActive(!UI.activeInHierarchy);
    }
}
