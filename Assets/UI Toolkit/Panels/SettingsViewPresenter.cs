using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SettingsViewPresenter
{
    public Action BackAction { set => backButton.clicked += value; }

    private Button backButton;
    private Toggle fullscreenToggle;

    public SettingsViewPresenter(VisualElement root)
    {
        backButton = root.Q<Button>("BackButton");
    }

    private void SetFullscreen(bool enabled)
    {
        Screen.fullScreen = enabled;
    }
}
