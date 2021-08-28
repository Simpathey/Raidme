using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StreamerConfigCanvas : MonoBehaviour
{
    [SerializeField] PlayerPrefsManager playerPrefs;

    [SerializeField] TMP_InputField clientID;
    [SerializeField] TMP_InputField clientSecret;
    [SerializeField] TMP_InputField botAccessToken;
    [SerializeField] TMP_InputField botRefreshToken;

    [SerializeField] TMP_InputField defenderFilePath;
    [SerializeField] TMP_InputField raiderFilePath;
    [SerializeField] TMP_InputField raidersIncomingmsg;
    [SerializeField] TMP_InputField raidersCommand;
    [SerializeField] TMP_InputField defendersCommand;
    [SerializeField] TMP_InputField communityName;
    [SerializeField] TMP_InputField timeToAdd;
    [SerializeField] TMP_InputField channelName;
    [SerializeField] TMP_InputField botName;

    [SerializeField] TextMeshPro Kofi;

    private void Start() 
    {
        clientID.text = playerPrefs.GetClientID();
        clientSecret.text = playerPrefs.GetClientSecret();
        botAccessToken.text = playerPrefs.GetBotAccessToken();
        botRefreshToken.text = playerPrefs.GetBotRefreshToken();
        defenderFilePath.text = playerPrefs.GetDefenderFilePath();
        raiderFilePath.text = playerPrefs.GetRaiderFilePath();
        raidersIncomingmsg.text = playerPrefs.GetRaiderIncomingMsg();
        raidersCommand.text = playerPrefs.GetRaiderCommand();
        defendersCommand.text = playerPrefs.GetDefenderCommand();
        communityName.text = playerPrefs.GetCommunityName();
        timeToAdd.text = playerPrefs.GetTimeToAdd().ToString();
        channelName.text = playerPrefs.GetChannelName();
        botName.text = playerPrefs.GetBotName();
    }

    public void Link()
    {
        Application.OpenURL("https://ko-fi.com/simpathey");
    }
}
