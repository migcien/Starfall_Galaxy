using UnityEngine;
using UnityEngine.UIElements;


public class StartViewPresenter : MonoBehaviour
{
    // Public variables to store references to the different views
    public VisualElement startView;
    public VisualElement settingsView;
    public VisualElement connectView;

    void Start()
    {
        // Get a reference to the root visual element of the UI Document
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Get references to the MainMenu, SettingsView, and ConnectView elements in the UI
        startView = root.Q("MainMenu");
        settingsView = root.Q("SettingsView");
        connectView = root.Q("ConnectView");

        // Call the setup methods for each view
        SetupStartView();
        SetupSettingsView();
        SetupConnectView();
    }

    private void SetupStartView()
    {
        // Create a new MainMenuPresenter and pass it the startView visual element
        MainMenuPresenter menuPresenter = new MainMenuPresenter(startView);

        // Set the open settings and open connect actions to toggle the settings and connect views
        menuPresenter.OpenSettings = () => ToggleSettingsView(true);
        menuPresenter.OpenConnect = () => ToggleConnectView(true);
    }

    private void SetupSettingsView()
    {
        // Create a new SettingsViewPresenter and pass it the settingsView visual element
        SettingsViewPresenter settingsPresenter = new SettingsViewPresenter(settingsView);

        // Set the back action to toggle the settings view off
        settingsPresenter.BackAction = () => ToggleSettingsView(false);
    }

    // Setup method for the connect view
    private void SetupConnectView()
    {
        // Create a new ConnectViewPresenter and pass it the connectView visual element
        ConnectViewPresenter connectPresenter = new ConnectViewPresenter(connectView);

        // Set the back action to toggle the connect view off
        connectPresenter.BackAction = () => ToggleConnectView(false);
    }

    // Method to toggle the visibility of the settings view
    private void ToggleSettingsView(bool enable)
    {
        // Hide or show the start view and the settings view based on the enable parameter
        startView.Display(!enable);
        settingsView.Display(enable);
    }

    // Method to toggle the visibility of the connect view
    private void ToggleConnectView(bool enable)
    {
        // Hide or show the start view and the connect view based on the enable parameter
        startView.Display(!enable);
        connectView.Display(enable);
    }
}