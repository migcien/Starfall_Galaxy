// UIExtentions is a static class that contains the Display method, which can be called on any VisualElement object

using System;
using System.IO;
using UnityEngine.UIElements;

public static class UIExtentions
{
    // Extension method to show or hide a VisualElement
    public static void Display(this VisualElement element, bool enabled)
    {
        // Return if the element is null
        if (element == null) return;

        // Set the display style of the element to either "Flex" or "None" based on the enabled parameter
        element.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
    }
}