using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using HSVPicker;

public class TextManager : MonoBehaviour
{
    [SerializeField] List<TextMeshPro> gameText;
    [SerializeField] PlayerPrefsManager playerPrefs;
    [SerializeField] ColorPicker textColorPicker;
    [SerializeField] ColorPicker textOutlinePicker;
    private Color textColor;
    private Color textOutlineColor;

    void Start()
    {
        SetColorPickerColor();
        LoadAndSetColors();
    }

    private void SetColorPickerColor()
    {
        ColorUtility.TryParseHtmlString(playerPrefs.GetTextColor(), out textColor);
        ColorUtility.TryParseHtmlString(playerPrefs.GetOutlineColor(), out textOutlineColor);
        textColorPicker.AssignColor(textColor);
        textOutlinePicker.AssignColor(textOutlineColor);
    }

    public void SaveAndSetColors()
    {
        playerPrefs.SetTextColor("#"+ColorUtility.ToHtmlStringRGBA(textColorPicker.CurrentColor));
        playerPrefs.SetOutlineColor("#" + ColorUtility.ToHtmlStringRGBA(textOutlinePicker.CurrentColor));
        LoadAndSetColors();
    }

    private void LoadAndSetColors()
    {
        Debug.Log(playerPrefs.GetTextColor() + " TEXT COLOR");
        Debug.Log(playerPrefs.GetOutlineColor() + " OUTLINE COLOR");
        ColorUtility.TryParseHtmlString(playerPrefs.GetTextColor(), out textColor);
        ColorUtility.TryParseHtmlString(playerPrefs.GetOutlineColor(), out textOutlineColor);
        UpdateTextColor();
    }

    public void UpdateTextColor()
    {
        foreach (var text in gameText)
        {
            Debug.Log(textColor);
            text.color = textColor;
            text.fontMaterial.SetColor("_UnderlayColor", textOutlineColor);
        }
    }
}
