using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFitter
{
    public static void SetSize(ref GameObject _uiElement, float _newSize)
    {
        SetWidth(ref _uiElement, _newSize);
        SetHeight(ref _uiElement, _newSize);
    }

    public static void SetWidth(ref GameObject _uiElement, float _newWidth)
    {
        Vector2 offsetMin = _uiElement.GetComponent<RectTransform>().offsetMin;
        Vector2 offsetMax = _uiElement.GetComponent<RectTransform>().offsetMax;
        float oldWidth = _uiElement.GetComponent<RectTransform>().rect.width;

        float deltaWidth = _newWidth - oldWidth;

        float leftDeltaWidth = deltaWidth * 0.5f; // TODO : pouvoir régler le pourcentage : left : p ; right : 1 - p
        float rightDeltaWidth = deltaWidth * 0.5f;

        offsetMin[0] -= leftDeltaWidth;
        offsetMax[0] += rightDeltaWidth;

        _uiElement.GetComponent<RectTransform>().offsetMin = offsetMin;
        _uiElement.GetComponent<RectTransform>().offsetMax = offsetMax;
    }

    public static void SetHeight(ref GameObject _uiElement, float _newHeight)
    {
        Vector2 offsetMin = _uiElement.GetComponent<RectTransform>().offsetMin;
        Vector2 offsetMax = _uiElement.GetComponent<RectTransform>().offsetMax;
        float oldHeight = _uiElement.GetComponent<RectTransform>().rect.height;

        float deltaHeight = _newHeight - oldHeight;

        float topDeltaHeight = deltaHeight * 0.5f; // TODO : pouvoir régler le pourcentage : left : p ; right : 1 - p
        float botDeltaHeight = deltaHeight * 0.5f;

        offsetMin[1] -= botDeltaHeight;
        offsetMax[1] += topDeltaHeight;

        _uiElement.GetComponent<RectTransform>().offsetMin = offsetMin;
        _uiElement.GetComponent<RectTransform>().offsetMax = offsetMax;
    }
}