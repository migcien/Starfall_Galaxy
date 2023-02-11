using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StartViewPresenter : MonoBehaviour
{
    private VisualElement _startView;
    private VisualElement _settingsView;
    private VisualElement _ConnectWalletView;

    // Start is called before the first frame update
   /* void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _startView = root.Q("StartView");
        _settingsView = root.Q("SettingsView");
        _ConnectWalletView = root.Q("ConnectWalletView");

        SetupStartView();
        SetupSettingsView();
        SetupConnectView();
    }

    private void SetupStartView()
    {
        MainMenuPresenter menuPresenter = new MainMenuPresenter();
        menuPresenter.OpenSettings = () => ToggleSettingsView(true);
        menuPresenter.OpenConnect = () => ToggleConnectView(true);
    }
    private void SetupSettingsView()
    {
        SettingsViewPresenter settingsPresenter = new SettingsViewPresenter();
        settingsPresenter.BackAction = () => ToggleSettingsView(false) && ToggleConnectView(false);
    }

    private void SetupConnectView()
    {
        ConnectWalletViewPresenter connectPresenter = new ConnectWalletViewPresenter();
        connectPresenter.BackAction = () => ToggleSettingsView(false) && ToggleConnectView(false);
    }


    private void ToggleSettingsView(bool enable)
    {
        _startView.Display(!enable);
        _settingsView.Display(enable);
        _ConnectWalletView.Display(!enable);
    }
    private void ToggleConnectView(bool enable)
    {
        _startView.Display(!enable);
        _settingsView.Display(!enable);
        _ConnectWalletView.Display(enable);
    }*/
}
