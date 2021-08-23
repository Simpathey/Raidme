using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaidStartingScene : MonoBehaviour
{
    public PlayerPrefsManager playerPrefs;
    public TextMeshPro raidMsg;
    public TextMeshPro raiderCommand;
    public TextMeshPro defenderCommand;
    public TextMeshPro raiderNames;
    public Timer timer;

    void Start()
    {
        UpdateRaidStartingScene();
    }
    public void UpdateRaidStartingScene()
    {
        raidMsg.text = playerPrefs.GetRaiderIncomingMsg();
        raiderCommand.text = "!" + playerPrefs.GetRaiderCommand();
        defenderCommand.text = "!" + playerPrefs.GetDefenderCommand();
    }
    
    public void SetRaiderName(string listOfRaiders)
    {
        raiderNames.text = listOfRaiders;
    }
    
    public void AddTime(float time)
    {
        timer.AddTime(time);
    }
    public void StartClock()
    {
        timer.StartClock();
    }

}
