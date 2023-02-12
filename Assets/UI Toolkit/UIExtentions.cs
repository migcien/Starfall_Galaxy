using System;
using System.IO;
using UnityEngine.UIElements;


public static class UIExtentions
{
    public static void Display(this VisualElement element, bool enabled)
    {
        if (element == null) return;
        element.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
    }

}
