using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLights : MonoBehaviour, ILight
{
    private MeshRenderer mesh;
    private Material material;
    public Color onColor;
    public Color offColor;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        material = mesh.materials[0];
    }

    public void ChangeState(bool state)
    {
        if (state)
        {
            material.color = onColor;
        }
        else
        {
            material.color = offColor;
        }
    }

    public void TurnOn()
    {
        material.color = onColor;
    }

    public void TurnOff()
    {
        material.color = offColor;
    }
}
