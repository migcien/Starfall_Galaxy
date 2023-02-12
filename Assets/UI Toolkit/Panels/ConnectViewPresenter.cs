using System;
using UnityEngine.UIElements;


public class ConnectViewPresenter
{ 
    public Action BackAction { set => backButton.clicked += value; }

    private Button backButton;

    public ConnectViewPresenter (VisualElement root)
    {
        backButton = root.Q<Button>("BackButton");
    }
}
