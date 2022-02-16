using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Api.Models.Undocumented.Chatters;
using System;
using TwitchLib.Api.Models.Undocumented.ChatProperties;

public class TwitchAPI : MonoBehaviour
{
    public Api api;
    public TwitchClient twitchClient;
    public PlayerPrefsManager playerPrefs;
    public float viewerCount = 1;
    bool countViewers;

    public void APIConnection()
    {
        Application.runInBackground = true;
        InitilizeAPI();

        //we will count viewers until we get raided
        countViewers = true;

        //after client connected we start making api calls
    }

    private void InitilizeAPI()
    {
        //make an api class and connect it to our tokens 
        api = new Api();
        api.Settings.AccessToken = playerPrefs.GetBotAccessToken();
        api.Settings.ClientId = playerPrefs.GetClientID();
        StartCoroutine(CheckViewerCountCoroutine());
    }

    public void OnRaidReceived()
    {
        //stop counting viewers when 
        countViewers = false;
    } 
    
    public void OnRaidEnded()
    {
        countViewers = true;
    }

    private void InitiateAPICount()
    {
        //ask api how many users are connected to chat
        //once it has an answer it will send the result to our callback method
        if (twitchClient.client.JoinedChannels.Count>0) { Debug.Log("Abort api count due to null client channel"); return; }
        api.Invoke(api.Undocumented.GetChattersAsync
        (twitchClient.client.JoinedChannels[0].Channel), CountChattersListCallback);
    }

    private void CountChattersListCallback(List<ChatterFormatted> obj)
    {
        viewerCount = obj.Count;
        Debug.Log("chat connections " + obj.Count);
    }

    IEnumerator CheckViewerCountCoroutine()
    {
        while (true)
        {
            if (twitchClient.client.JoinedChannels != null && countViewers) { InitiateAPICount(); }
            yield return new WaitForSeconds(60);
        }
    }



    public float GetViewerCount()
    {
        return viewerCount;
    }
}
