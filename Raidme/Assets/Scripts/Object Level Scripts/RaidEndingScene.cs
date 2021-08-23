using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaidEndingScene : MonoBehaviour
{
    public TextMeshPro raiderNames;
    public TextMeshPro communityName;
    public TextMeshPro draw;
    public TextMeshPro win;
    public PlayerPrefsManager playerPrefs;

    void Start()
    {
        UpdateRaidEndScene();
    }

    public void UpdateRaidEndScene()
    {
        communityName.text = playerPrefs.GetCommunityName();
    }

    //Call this when raid is over
    public void SetRaiderNames(string raiderNamesInput)
    {
        raiderNames.text = "";
        raiderNames.text = raiderNamesInput;
    }

    public void RaidersWin()
    {
        raiderNames.enabled = true;
        communityName.enabled = false;
        draw.enabled = false;
        win.enabled = true;
    }
    public void DefendersWin()
    {
        raiderNames.enabled = false;
        draw.enabled = false;
        communityName.enabled = true;
        win.enabled = true;
    }
    public void Draw()
    {
        raiderNames.enabled = false;
        communityName.enabled = false;
        win.enabled = false;
        draw.enabled = true;
    }
}
