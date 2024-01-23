using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class BuildVersion : MonoBehaviour
{
    TextMeshProUGUI versionTxt;

    private void Start()
    {
        versionTxt = GetComponent<TextMeshProUGUI>();
        string buildType = Debug.isDebugBuild ? "Dev" : string.Empty;

        versionTxt.text = $"build version: {buildType} {Application.version}";
    }
}
