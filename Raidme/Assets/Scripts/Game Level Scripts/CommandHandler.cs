using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Client.Events;


public class CommandHandler : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerPrefsManager playerPrefs;

    public void FilterCommand(OnChatCommandReceivedArgs e)
    {
        string command = e.Command.CommandText.ToLower();
        if (command.Contains(playerPrefs.GetDefenderCommand()))
        {
            gameManager.AddDefenderToRaidParams(e);
        }
        else if (command.Contains(playerPrefs.GetRaiderCommand()))
        {
            gameManager.AddRaiderToRaidParams(e);
        }
    }
}
