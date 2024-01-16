using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    public float panelHeight;

    private void OnEnable()
    {
        changePanelHeight(panelHeight);
    }

    public void changePanelHeight(float height)
    {

        panel.sizeDelta = new Vector2(panel.sizeDelta.x, height);
    }

}
