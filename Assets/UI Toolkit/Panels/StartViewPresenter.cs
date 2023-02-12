using UnityEngine;
using UnityEngine.UIElements;


public class StartViewPresenter : MonoBehaviour
{
    public VisualElement startView;
    public VisualElement settingsView;
    public VisualElement connectView;

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        startView = root.Q("MainMenu");
        settingsView = root.Q("SettingsView");
        connectView = root.Q("ConnectView");

        SetupStartView();
        SetupSettingsView();
        SetupConnectView();
    }

    private void SetupStartView()
    {
        MainMenuPresenter menuPresenter = new MainMenuPresenter(startView);
        menuPresenter.OpenSettings = () => ToggleSettingsView(true);
        menuPresenter.OpenConnect = () => ToggleConnectView(true);
    }
    private void SetupSettingsView()
    {
        SettingsViewPresenter settingsPresenter = new SettingsViewPresenter (settingsView);
        settingsPresenter.BackAction = () => ToggleSettingsView(false);
    }

    private void SetupConnectView()
    {
        ConnectViewPresenter connectPresenter = new ConnectViewPresenter(connectView);
        connectPresenter.BackAction = () => ToggleConnectView(false);
    }


    private void ToggleSettingsView(bool enable)
    {
        startView.Display(!enable);
        settingsView.Display(enable);   
    }
    private void ToggleConnectView(bool enable)
    {
        startView.Display(!enable);
        connectView.Display(enable);
    }
}
