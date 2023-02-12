using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SettingsViewPresenter
{
    private List <string> resolutions = new List <string>()
    {
        "3840x2160",
        "2560x1440",
        "1920x1080",
        "1600x900",
        "1366x768",
        "1280x720"
    };

    public Action BackAction { set => backButton.clicked += value; }

    private Button backButton;
    private Toggle fullscreenToggle;
    private DropdownField resSelection;

    public SettingsViewPresenter(VisualElement root)
    {
        backButton = root.Q<Button>("BackButton");
        fullscreenToggle = root.Q<Toggle>("FullscreenToggle");

        fullscreenToggle.RegisterCallback<MouseUpEvent>((evt) => { SetFullscreen(fullscreenToggle.value); }, TrickleDown.TrickleDown);

        resSelection = root.Q<DropdownField>("ResDropdown");
        resSelection.choices = resolutions;
        resSelection.RegisterValueChangedCallback((value) => SetResolution(value.newValue));
        resSelection.index = 0;
    }

    private void SetResolution(string newResolution)
    {
        string[] resolutionArray = newResolution.Split("x");
        int[] valuesIntArray = new int[] { int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]) };

        Screen.SetResolution(valuesIntArray[0], valuesIntArray[1], fullscreenToggle.value);
    }

    private void SetFullscreen(bool enabled)
    {
        Screen.fullScreen = enabled;
    }
}
