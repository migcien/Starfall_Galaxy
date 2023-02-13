using System;
using UnityEngine.UIElements;


// Class that manages the behavior of the Connect View UI element
public class ConnectViewPresenter
{
    // Event that gets triggered when the back button is clicked
    public Action BackAction { set => backButton.clicked += value; }

    // The back button in the Connect View UI
    private Button backButton;

    // Constructor that finds the back button element in the Connect View UI
    public ConnectViewPresenter(VisualElement root)
    {
        // Find the back button in the Connect View UI using its name
        backButton = root.Q<Button>("BackButton");
    }
}