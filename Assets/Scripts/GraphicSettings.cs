using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicSettings : MonoBehaviour
{
    [Header("Current")]
    public Vector2 currentResolution;
    public bool currentFullScreen;
    [Header("Settings")]
    public Vector2 selectResolution;
    public bool selectFullScreen;

    [Header("Options")]
    public bool isOpenPanel = false;
    public Vector2[] resolution;
    public TMP_Dropdown TMP_dropdown_resolution;
    public UnityEngine.UI.Toggle toggleFullScreen;
    public CanvasScaler canvasScaler;

    private void Start()
    {
        currentResolution = new Vector2(Screen.width, Screen.height);
        currentFullScreen = Screen.fullScreen;
    }

    private void Update()
    {
        if (isOpenPanel)
        {
            if (selectFullScreen != toggleFullScreen.isOn)
            {
                selectFullScreen = toggleFullScreen.isOn;
            }
            if (TMP_dropdown_resolution.options.Count == resolution.Length)
            {
                if (selectResolution != resolution[(TMP_dropdown_resolution.value)])
                {
                    selectResolution = resolution[(TMP_dropdown_resolution.value)];
                }
            }
        }
    }

    public void TurnPanel(bool turn)
    {
        isOpenPanel = turn;
        if (isOpenPanel)
        {
            if (toggleFullScreen.isOn != currentFullScreen)
            {
                toggleFullScreen.isOn = currentFullScreen;
            }
            if (TMP_dropdown_resolution.value != System.Array.IndexOf(resolution, currentResolution))
            {
                TMP_dropdown_resolution.value = System.Array.IndexOf(resolution, currentResolution);
            }
        }
    }

    public void ApplySettings()
    {
        if (selectFullScreen)
        {
            Screen.SetResolution((int)selectResolution.x, (int)selectResolution.y, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution((int)selectResolution.x, (int)selectResolution.y, FullScreenMode.Windowed);
        }
        currentResolution = selectResolution;
        currentFullScreen = selectFullScreen;
        if(canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
        {
            if(canvasScaler.referenceResolution != selectResolution)
            {
                canvasScaler.referenceResolution = selectResolution;
            }
        }
    }

    public void DiscardSettings()
    {
        selectResolution = Vector2.zero;
        selectFullScreen = false;
    }

    public bool CheckResolution(Vector2 ScreenSize)
    {
        foreach (Vector2 item in resolution)
        {
            if(ScreenSize == item)
            {
                return true;
            }
        }
        return false;
    }
}
