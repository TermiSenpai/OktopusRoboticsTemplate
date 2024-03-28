using UnityEngine;
using UnityEngine.UI;

public class SimUI : MonoBehaviour
{
    [SerializeField] private Slider sliderX;
    [SerializeField] private Slider sliderY;
    [SerializeField] private Slider sliderZ;

    private void Start()
    {
        sliderX.onValueChanged.AddListener(PaletizerAxisMovement.Instance.OnSliderX);
        sliderX.value = PaletizerAxisMovement.Instance.GetAxisX();

        sliderY.onValueChanged.AddListener(PaletizerAxisMovement.Instance.OnSliderY);
        sliderY.value = PaletizerAxisMovement.Instance.GetAxisY();

        sliderZ.onValueChanged.AddListener(PaletizerAxisMovement.Instance.OnSliderZ);
        sliderZ.value = PaletizerAxisMovement.Instance.GetAxisZ();
    }

    private void Update()
    {
        sliderX.value = PaletizerAxisMovement.Instance.GetAxisX();
        sliderY.value = PaletizerAxisMovement.Instance.GetAxisY();
        sliderZ.value = PaletizerAxisMovement.Instance.GetAxisZ();
    }
}
