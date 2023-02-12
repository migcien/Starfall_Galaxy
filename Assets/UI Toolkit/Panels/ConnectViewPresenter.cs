using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class ConnectViewPresenter
{ 
    public Action BackAction { set => _backButton.clicked += value; }

    private Button _backButton;

    public ConnectViewPresenter (VisualElement root)
    {
        _backButton = root.Q<Button>("BackButton");
    }
}
