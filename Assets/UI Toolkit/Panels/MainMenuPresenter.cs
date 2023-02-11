using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuPresenter : MonoBehaviour
{
    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>("PlayButton").clicked += () => Debug.Log("Play button clicked");
        root.Q<Button>("WalletButton").clicked += () => Debug.Log("Wallet button clicked");
        root.Q<Button>("SettingsButton").clicked += () => Debug.Log("Settings button clicked");
    }
}
