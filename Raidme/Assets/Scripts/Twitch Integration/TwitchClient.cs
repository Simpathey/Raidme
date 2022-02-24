using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Client.Models;
using System;
using TwitchLib.Client.Events;

public class TwitchClient : MonoBehaviour
{
    public Client client;
    public TwitchAPI twitchAPI;
    public PlayerPrefsManager playerPrefsManager;
    public CommandHandler commandHandler;
    public GameManager gameManager;
    public UserFeedback userFeedback;
    // Start is called before the first frame update
    void Start()
    {
        //I was here when Simpa 9 slice'd
        ConnectClient();
    }

    public void ConnectClient()
    {
        Debug.Log("client is attempting to connect");
        Application.runInBackground = true;

        ConnectionCredentials creds = new ConnectionCredentials(playerPrefsManager.GetBotName(), playerPrefsManager.GetBotAccessToken()); //BOT NAME
        client = new Client();

        client.Initialize(creds, playerPrefsManager.GetChannelName());  //this is where we will fail

        //subscribe to events
        client.OnChatCommandReceived += CommandMessageReceived;
        client.OnRaidNotification += Raid;
        client.OnConnected += ClientConnected;
        client.OnConnectionError += ClientConnectionError;

        client.Connect();
        if (!client.IsConnected) { userFeedback.ClientFailed(); }
    }

    private void ClientConnectionError(object sender, OnConnectionErrorArgs e)
    {
        Debug.Log("Failed to connect");
        Debug.Log(e.Error);
    }

    private void Raid(object sender, OnRaidNotificationArgs e)
    {
        if (int.Parse(e.RaidNotificaiton.MsgParamViewerCount) >= playerPrefsManager.GetRaiderLimit())
        { 
            FindObjectOfType<TwitchAPI>().OnRaidReceived();
            gameManager.RaidReceived(e);
            Debug.Log("CLIENT RAID CALL");
        }
    }

    private void ClientConnected(object sender, OnConnectedArgs e)
    {
        userFeedback.ClientSuccess();
        Debug.Log("CLIENT CONNECTED");
        Debug.Log(e.BotUsername);
        Debug.Log(e.AutoJoinChannel);
        Debug.Log(client.JoinedChannels.Count);
        twitchAPI.APIConnection();
        client.SendMessage(client.JoinedChannels[0],"Raid Me Connected to Chat :D");
    }

    private void CommandMessageReceived(object sender, OnChatCommandReceivedArgs e)
    {
        commandHandler.FilterCommand(e);
    }

}
