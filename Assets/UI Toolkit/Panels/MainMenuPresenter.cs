using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuPresenter
{ 
    public Action OpenSettings { set => settingsButton.clicked += value; }
    public Action OpenConnect { set => connectButton.clicked += value; }

    private Button playButton;
    private Button settingsButton;
    private Button connectButton;
    public MainMenuPresenter (VisualElement root)
    {

        playButton = root.Q<Button>("PlayButton");
        settingsButton = root.Q<Button>("SettingsButton");
        connectButton = root.Q<Button>("ConnectButton");       
        AddLogsToButtons();

    }

    private void AddLogsToButtons()
    {
        playButton.clicked += () => Debug.Log("Play button clicked");
        connectButton.clicked += () => Debug.Log("Wallet button clicked");
        settingsButton.clicked += () => Debug.Log("Settings button clicked");
    }
}
