using UnityEngine;
using UnityEngine.UI;

public class GeneralLightManager : MonoBehaviour
{
    [SerializeField] private Light MainSpot;
    [SerializeField] private Light backSpot;
    [SerializeField] private Light diagonalSpot;

    [SerializeField] private Light ambientLight;
    [SerializeField] private Light centralLight;

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

    public void OnToggleBtn(GameObject light)
    {
        light.SetActive(!light.activeInHierarchy);
    }

    public void OnAmbientIntensitySliderChange(float value)
    {
        ambientLight.intensity = value;
    }

    public void OnCentralIntensitySliderChange(float value)
    {
        centralLight.intensity = value;
    }

    public void OnMainSpotIntensitySliderChange(float value)
    {
        MainSpot.intensity = value;
    }

    public void OnBackIntensitySliderChange(float value)
    {
        backSpot.intensity = value;
    }

    public void OnDiagonalIntensitySliderChange(float value)
    {
        diagonalSpot.intensity = value;
    }

    public void ToggleColorPicker(GameObject colorPicker)
    {
        colorPicker.SetActive(!colorPicker.activeInHierarchy);
        CheckPanelHeight(colorPicker);
    }

    private void CheckPanelHeight(GameObject colorPicker)
    {
        if (colorPicker.activeInHierarchy)
        {
            Rect rect = panelRect.rect;
            rect.height = 750f;

            // Asigna el rect modificado de nuevo al RectTransform
            panelRect.sizeDelta = new Vector2(rect.width, 750f);
        }
        else
        {
            Rect rect = panelRect.rect;
            rect.height = 450f;

            // Asigna el rect modificado de nuevo al RectTransform
            panelRect.sizeDelta = new Vector2(rect.width, 450f);
        }
    }

    private void ToggleUI()
    {
        UI.gameObject.SetActive(!UI.activeInHierarchy);
    }
}
