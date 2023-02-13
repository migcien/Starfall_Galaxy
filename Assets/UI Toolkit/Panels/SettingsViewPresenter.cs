using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsViewPresenter
{
    // List of resolutions that the user can choose from in the settings menu
    private List<string> resolutions = new List<string>()
    {
        "3840x2160",
        "2560x1440",
        "1920x1080",
        "1600x900",
        "1366x768",
        "1280x720"
    };

    // Property that sets the action to be performed when the back button is clicked
    public Action BackAction { set => backButton.clicked += value; }

    // Variables for the back button and the toggle for fullscreen and the dropdown menu for resolution selection
    private Button backButton;
    private Toggle fullscreenToggle;
    private DropdownField resSelection;

    public SettingsViewPresenter(VisualElement root)
    {
        // Get reference to the back button from the UI Document
        backButton = root.Q<Button>("BackButton");

        // Get reference to the fullscreen toggle from the UI Document
        fullscreenToggle = root.Q<Toggle>("FullscreenToggle");

        // Register a callback that sets the fullscreen setting to the value of the fullscreen toggle when the mouse button is released
        fullscreenToggle.RegisterCallback<MouseUpEvent>((evt) => { SetFullscreen(fullscreenToggle.value); }, TrickleDown.TrickleDown);

        // Get reference to the resolution dropdown from the UI Document
        resSelection = root.Q<DropdownField>("ResDropdown");

        // Set the options in the resolution dropdown to the resolutions list
        resSelection.choices = resolutions;

        // Register a callback that sets the resolution to the selected value when the resolution is changed in the dropdown
        resSelection.RegisterValueChangedCallback((value) => SetResolution(value.newValue));

        // Set the default selection in the resolution dropdown to the first option
        resSelection.index = 0;
    }

    // Method to set the screen resolution based on the selected value in the resolution dropdown
    private void SetResolution(string newResolution)
    {
        // Split the string representation of the resolution into an array of strings, separated by 'x'
        string[] resolutionArray = newResolution.Split("x");

        // Convert the string values of the resolution into integers and store them in an array
        int[] valuesIntArray = new int[] { int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]) };

        // Set the screen resolution to the selected value and the fullscreen setting based on the value of the fullscreen toggle
        Screen.SetResolution(valuesIntArray[0], valuesIntArray[1], fullscreenToggle.value);
    }

    // Method to set the fullscreen setting based on the value of the fullscreen toggle
    private void SetFullscreen(bool enabled)
    {
        Screen.fullScreen = enabled;
    }
}