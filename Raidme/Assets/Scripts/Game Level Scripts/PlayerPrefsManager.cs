using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HSVPicker;

public class PlayerPrefsManager : MonoBehaviour
{
    private void Start()
    {
        if (!PlayerPrefs.HasKey("DefenderCommand"))
        {
            PlayerPrefs.SetString("DefenderCommand", "defend");
        }
        if (!PlayerPrefs.HasKey("RaiderCommand"))
        {
            PlayerPrefs.SetString("RaiderCommand", "raid");
        }
        if (!PlayerPrefs.HasKey("RaiderIncomingMsg"))
        {
            PlayerPrefs.SetString("RaiderIncomingMsg", "Raiders Incoming");
        }
        if (!PlayerPrefs.HasKey("CommunityName"))
        {
            PlayerPrefs.SetString("CommunityName", "Defenders");
        }
        if (!PlayerPrefs.HasKey("TimeToAdd"))
        {
            PlayerPrefs.SetInt("TimeToAdd", 30);
        }
        if (!PlayerPrefs.HasKey("ChannelName"))
        {
            PlayerPrefs.SetString("ChannelName", "MyChannel");
        }
        if (!PlayerPrefs.HasKey("BotName"))
        {
            PlayerPrefs.SetString("BotName", "MyBotName");
        }
        if (!PlayerPrefs.HasKey("TextColor"))
        {
            PlayerPrefs.SetString("TextColor", "#ffffffff");
        }
        if (!PlayerPrefs.HasKey("OutlineColor"))
        {
            PlayerPrefs.SetString("OutlineColor", "#000000ff");
        }
        if (!PlayerPrefs.HasKey("RaidLimit"))
        {
            PlayerPrefs.SetInt("RaidLimit", 0);
        }
    }

    //creating methods so that we can set player prefs 
    public void SetGameDefenderPNG(TMPro.TMP_InputField inputString)
    {
        string filepath = inputString.text;
        Debug.Log("defender png location saving:" + filepath);
        PlayerPrefs.SetString("DefenderPNGLocalLocation", filepath);
    }
    public void SetGameRaiderPNG(TMPro.TMP_InputField inputString)
    {
        string filepath = inputString.text;
        Debug.Log("raider png location saving:" + filepath);
        PlayerPrefs.SetString("RaiderPNGLocalLocation", filepath);
    }
    public void SetDefenderCommand(TMPro.TMP_InputField inputString)
    {
        string defenderCommand = inputString.text.ToLower();
        defenderCommand = defenderCommand.Replace("!", "");
        Debug.Log("defender command saving:" + defenderCommand);
        PlayerPrefs.SetString("DefenderCommand", defenderCommand);
    }
    public void SetRaiderCommand(TMPro.TMP_InputField inputString)
    {
        string raiderCommand = inputString.text.ToLower();
        raiderCommand = raiderCommand.Replace("!", "");
        Debug.Log("raider command saving:" + raiderCommand);
        PlayerPrefs.SetString("RaiderCommand", raiderCommand);
    }
    public void SetRaiderIncomingMsg(TMPro.TMP_InputField inputString)
    {
        string raiderIncomingMsg = inputString.text;
        Debug.Log("raider incoming msg saving:" + raiderIncomingMsg);
        PlayerPrefs.SetString("RaiderIncomingMsg", raiderIncomingMsg);
    }
    public void SetDefenderName(TMPro.TMP_InputField inputString)
    {
        string defenderName = inputString.text;
        Debug.Log("defender name saved:" + defenderName);
        PlayerPrefs.SetString("DefenderName", defenderName);
    }
    public void SetCommunityName(TMPro.TMP_InputField inputString)
    {
        string communityName = inputString.text;
        Debug.Log("community name saved:" + communityName);
        PlayerPrefs.SetString("CommunityName", communityName);
    }
    public void SetTimeToAdd(TMPro.TMP_InputField inputString)
    {
        try
        {
            int timeToAdd = int.Parse(inputString.text);
            Debug.Log("Time added saved:" + timeToAdd);
            PlayerPrefs.SetInt("TimeToAdd", timeToAdd);
        }
        catch (System.Exception)
        {
            //lol lolo lol olol lolo lo ldont judge me =^.^=
        }
    }
    public void SetClientID(TMPro.TMP_InputField inputString)
    {
        if (inputString.text == null)
        {
            return;
        }
        string clientIDText = inputString.text;
        Debug.Log("clientID name saved");
        PlayerPrefs.SetString("ClientID", clientIDText);
    }
    public void SetClientSecret(TMPro.TMP_InputField inputString)
    {
        if (inputString.text == null)
        {
            return;
        }
        string clientSecretText = inputString.text;
        Debug.Log("client secret saved");
        PlayerPrefs.SetString("ClientSecret", clientSecretText);
    }
    public void SetBotAccessToken(TMPro.TMP_InputField inputString)
    {
        if (inputString.text == null)
        {
            return;
        }
        string botAccessText = inputString.text;
        Debug.Log("client secret saved");
        PlayerPrefs.SetString("BotAccessToken", botAccessText);
    }
    public void SetBotRefreshToken(TMPro.TMP_InputField inputString)
    {
        if (inputString.text == null)
        {
            return;
        }
        string botRefreshText = inputString.text;
        Debug.Log("client secret saved");
        PlayerPrefs.SetString("BotRefreshToken", botRefreshText);
    }
    public void SetChannelName(TMPro.TMP_InputField inputString)
    {
        string channelName = inputString.text;
        PlayerPrefs.SetString("ChannelName", channelName);
    }
    public void SetBotName(TMPro.TMP_InputField inputString)
    {
        string channelName = inputString.text;
        PlayerPrefs.SetString("BotName", channelName);
    }
    public void SetTextColor(string colorHex)
    {
        PlayerPrefs.SetString("TextColor", colorHex);
    }
    public void SetOutlineColor(string colorHex)
    {
        PlayerPrefs.SetString("OutlineColor", colorHex);
    }
    public void SetRaiderLimit(TMPro.TMP_InputField inputString)
    {
        try
        {
            int raiderLimit = int.Parse(inputString.text);
            PlayerPrefs.SetInt("RaiderLimit", raiderLimit);
        }
        catch (System.Exception)
        {
            //lol lolo lol olol lolo lo ldont judge me =^.^=
        }
    }

    public string GetChannelName()
    {
        if (PlayerPrefs.HasKey("ChannelName"))
        {
            return PlayerPrefs.GetString("ChannelName");
        }
        else
        {
            return "";
        }
    }
    public string GetBotName()
    {
        if (PlayerPrefs.HasKey("BotName"))
        {
            return PlayerPrefs.GetString("BotName");
        }
        else
        {
            return "";
        }
    }
    public string GetDefenderFilePath()
    {
        if (PlayerPrefs.HasKey("DefenderPNGLocalLocation"))
        {
            return PlayerPrefs.GetString("DefenderPNGLocalLocation");
        }
        else
        {
            return ("hey");
        }
    }
    public string GetRaiderFilePath()
    {
        if (PlayerPrefs.HasKey("RaiderPNGLocalLocation"))
        {
            return PlayerPrefs.GetString("RaiderPNGLocalLocation");
        }
        else
        {
            return ("lol");
        }
    }
    public string GetRaiderIncomingMsg()
    {
        return PlayerPrefs.GetString("RaiderIncomingMsg");
    }
    public int GetTimeToAdd()
    {
        return PlayerPrefs.GetInt("TimeToAdd");
    }
    public string GetRaiderCommand()
    {
        return PlayerPrefs.GetString("RaiderCommand");
    }
    public string GetDefenderCommand()
    {
        return PlayerPrefs.GetString("DefenderCommand");
    }
    public string GetCommunityName()
    {
        return PlayerPrefs.GetString("CommunityName");
    }
    public string GetClientID()
    {
        return PlayerPrefs.GetString("ClientID");
    }
    public string GetClientSecret()
    {
        return PlayerPrefs.GetString("ClientSecret");
    }
    public string GetBotAccessToken()
    {
        return PlayerPrefs.GetString("BotAccessToken");
    }
    public string GetBotRefreshToken()
    {
        return PlayerPrefs.GetString("BotRefreshToken");
    }
    public string GetTextColor()
    {
        return PlayerPrefs.GetString("TextColor");
    }
    public string GetOutlineColor()
    {
        return PlayerPrefs.GetString("OutlineColor");
    }
    public int GetRaiderLimit()
    {
        return PlayerPrefs.GetInt("RaidLimit");
    }
} 
