using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuPresenter
{
    // The OpenSettings property allows external code to register an event that is triggered when the "Settings" button is clicked
    public Action OpenSettings { set => settingsButton.clicked += value; }
    // The OpenConnect property allows external code to register an event that is triggered when the "Connect" button is clicked
    public Action OpenConnect { set => connectButton.clicked += value; }

    private Button playButton;
    private Button settingsButton;
    private Button connectButton;
    private Button exitButton;

    public MainMenuPresenter(VisualElement root)
    {
        // Get references to the UI buttons in the MainMenu visual element
        playButton = root.Q<Button>("PlayButton");
        settingsButton = root.Q<Button>("SettingsButton");
        connectButton = root.Q<Button>("ConnectButton");
        exitButton = root.Q<Button>("ExitButton");

        // Register the log messages for each button
        AddLogsToButtons();

        // Register the action that occurs when the "Play" button is clicked
        PlayButtonOnClicked();

        // Register the action that occurs when the "Exit" button is clicked
        ExitButtonOnClicked();
    }

    // This method adds log messages to be printed when each button is clicked
    private void AddLogsToButtons()
    {
        playButton.clicked += () => Debug.Log("Play button clicked");
        connectButton.clicked += () => Debug.Log("Wallet button clicked");
        settingsButton.clicked += () => Debug.Log("Settings button clicked");
        exitButton.clicked += () => Debug.Log("Exit button clicked");
    }

    // This method adds an action to be performed when the "Play" button is clicked
    private void PlayButtonOnClicked()
    {
        playButton.clicked += () => SceneManager.LoadScene("Controller1");
    }

    // This method adds an action to be performed when the "Exit" button is clicked
    private void ExitButtonOnClicked()
    {
        Application.Quit();
    }
}