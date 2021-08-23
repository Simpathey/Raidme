using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerToggle : MonoBehaviour
{
    public List<GameObject> colorPickerObjects;
    bool toggle = true;

    public void PressButton()
    {
        foreach (var gameObject in colorPickerObjects)
        {
            gameObject.SetActive(toggle);
        }
        toggle = !toggle;
    }
}
