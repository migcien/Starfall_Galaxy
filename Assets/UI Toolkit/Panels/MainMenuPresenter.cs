using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuPresenter
{ 
    public Action OpenSettings { set => settingsButton.clicked += value; }
    public Action OpenConnect { set => connectButton.clicked += value; }

    private Button playButton;
    private Button settingsButton;
    private Button connectButton;
    private Button exitButton;



    public MainMenuPresenter (VisualElement root)
    {

        playButton = root.Q<Button>("PlayButton");
        settingsButton = root.Q<Button>("SettingsButton");
        connectButton = root.Q<Button>("ConnectButton");
        exitButton = root.Q<Button>("ExitButton");
        AddLogsToButtons();
        PlayButtonOnClicked();
        ExitButtonOnClicked();

    }

    private void AddLogsToButtons()
    {
        playButton.clicked += () => Debug.Log("Play button clicked");
        connectButton.clicked += () => Debug.Log("Wallet button clicked");
        settingsButton.clicked += () => Debug.Log("Settings button clicked");
        exitButton.clicked += () => Debug.Log("Exit button clicked");
    }

    private void PlayButtonOnClicked()
    {
        playButton.clicked += () => SceneManager.LoadScene("Controller1");
    }
    private void ExitButtonOnClicked()
    {
        Application.Quit();
    }
}
