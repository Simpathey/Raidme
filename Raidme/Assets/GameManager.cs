using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Client.Events;
using System;

public class GameManager : MonoBehaviour
{
    //downtime, raidfight or raidtimer 
    public Enums.GameState gameState;

    //Hooked up in the inspector 
    public GameObject raidStartingScene;
    public GameObject streamerConfigCanvas;
    public bool configCanvasToggle;
    public RaidEndingScene endingScene;
    public RaidStartingScene startingScene;
    public PlayerPrefsManager playerPrefs;
    public TwitchAPI twitchAPI;
    public Battler battler;

    Queue<BattleParams> commandQueueRaid = new Queue<BattleParams>();
    public BattleParams currentBattleParams = new BattleParams();
    public string raiderNames;
    public List<string> raiderNameList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        endingScene.gameObject.SetActive(false);
        gameState = Enums.GameState.downtime;
    }

    public void RaidReceived(OnRaidNotificationArgs e)
    {

        string currentRaiderName = e.RaidNotificaiton.DisplayName;
        //start the timer if no raid is in progress, set raiding streamer name 
        if (gameState == Enums.GameState.downtime && !raiderNameList.Contains(currentRaiderName))
        {
            startingScene.gameObject.SetActive(true);
            UpdateRaiderNames(currentRaiderName);
            raiderNameList.Add(currentRaiderName);
            startingScene.SetRaiderName(raiderNames);
            gameState = Enums.GameState.raidTimer;
            startingScene.AddTime(playerPrefs.GetTimeToAdd());
            startingScene.StartClock();
            StartBattleParams(e);
        }
        //add time to clock if timer is ongoing, add new streamer name to list
        else if (gameState == Enums.GameState.raidTimer && !raiderNameList.Contains(currentRaiderName))
        { 
            startingScene.AddTime(playerPrefs.GetTimeToAdd());
            UpdateRaiderNames(e.RaidNotificaiton.DisplayName);
            raiderNameList.Add(currentRaiderName);
            startingScene.SetRaiderName(raiderNames);
        }
        //queue the raid to happen after the fight is over
        else if (gameState == Enums.GameState.raidFight)
        {
            //to do future feature
            //queue the raid and check to deque after raid fight over
        }
    }


    public void StartBattleParams(OnRaidNotificationArgs e)
    {
        currentBattleParams = new BattleParams();
        currentBattleParams.totalRaiderCount = float.Parse(e.RaidNotificaiton.MsgParamViewerCount);
        currentBattleParams.totalDefenderCount = twitchAPI.GetViewerCount();
    }
    public void AddDefenderToRaidParams(OnChatCommandReceivedArgs e)
    {
        string userName = e.Command.ChatMessage.Username;

        if (!currentBattleParams.defenderUserNames.Contains(userName) 
            && !currentBattleParams.raiderUserNames.Contains(userName))
        {
            currentBattleParams.defenderUserNames.Add(userName);
            Debug.Log(userName + " defender added");
        }
        Debug.Log(currentBattleParams.defenderUserNames);
    }
    public void AddRaiderToRaidParams(OnChatCommandReceivedArgs e)
    {
        string userName = e.Command.ChatMessage.Username;
        Debug.Log(userName);
        if (!currentBattleParams.defenderUserNames.Contains(userName)
            && !currentBattleParams.raiderUserNames.Contains(userName))
        {
            currentBattleParams.raiderUserNames.Add(userName);
            Debug.Log(userName + " raider added");
        }
        Debug.Log(currentBattleParams.raiderUserNames);
    }

    public void TimerHitZero()
    {
        raidStartingScene.SetActive(false);
        battler.StartBattle(currentBattleParams);
        gameState = Enums.GameState.raidFight;
        Debug.Log(currentBattleParams);
    }

    public IEnumerator RaidEnded()
    {
        twitchAPI.OnRaidEnded();
        endingScene.SetRaiderNames(raiderNames);
        endingScene.gameObject.SetActive(true);
        yield return new WaitForSeconds(6);
        endingScene.gameObject.SetActive(false);
        ResetRaiderNames();
        gameState = Enums.GameState.downtime;
    }

    private void ResetRaiderNames()
    {
        raiderNames = "";
        raiderNameList.Clear();
    }

    public void UpdateRaiderNames(string raiderName)
    {
        raiderNames += raiderName + " ";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            configCanvasToggle = !configCanvasToggle;
            streamerConfigCanvas.SetActive(configCanvasToggle);
        }
    }
}
