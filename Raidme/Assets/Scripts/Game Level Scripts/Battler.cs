using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

public class Battler : MonoBehaviour
{
    //Set up in inspector
    [SerializeField] PlayerPrefsManager playerPrefsManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] RaidEndingScene raidEndingScene;

    //Defenders and Raiders will spawn as children of these game objects
    [SerializeField] GameObject DefendersParent;
    [SerializeField] GameObject RaidersParent;

    //Spawn Location of the raiders and defenders
    [SerializeField] Transform raiderPosition;
    [SerializeField] Transform defenderPosition;

    //Prefab for Game Units
    [SerializeField] GameObject gameUnitPrefab;

    //Lists of all the defenders and raiders we spawn into the game 
    public List<GameUnitController> raidersList = new List<GameUnitController>();
    public List<GameUnitController> defendersList= new List<GameUnitController>();

    //Power scalings calculated by the percentage participation of each side 
    private float raiderPowerScaling;
    private float defenderPowerScaling;

    //Stored Sprites for Defender and Raider
    [SerializeField] Sprite defenderSprite; //serialized for debug only
    [SerializeField] Sprite raiderSprite;

    bool battleOver;

    private void Start()
    {
        battleOver = true;
        LoadDefenderSprites();
        LoadRaiderSprites();
    }

    public void LoadDefenderSprites()
    {
        try
        {
            //string[] filePath = Directory.GetFiles(Application.dataPath + "/DefenderSprite/");
            //defenderSprite = LoadPNG(filePath[0]);
            defenderSprite = LoadPNG(playerPrefsManager.GetDefenderFilePath());
        }
        catch (Exception)
        {
            //if exception give user feedback that they need to enter a valid defender file path
        }
    }
    public void LoadRaiderSprites()
    {
        try
        {
            //string[] filePath = Directory.GetFiles(Application.dataPath + "/RaiderSprite/*.png");
            //raiderSprite = LoadPNG(filePath[0]);
            raiderSprite = LoadPNG(playerPrefsManager.GetRaiderFilePath());
        }
        catch (Exception)
        {
            //if exception give user feedback that they need to enter a valid raider file path
        }
    }

    public void StartBattle
        (BattleParams battleParams)
    {
        battleOver = false;
        //calculates the participation rate of each side, clamps value between 0.1 and 1
        float participatingDefenders = battleParams.defenderUserNames.Count;
        defenderPowerScaling = Mathf.Clamp(participatingDefenders / battleParams.totalDefenderCount, 0.1f,1f);
        float participatingRaiders = battleParams.raiderUserNames.Count;
        raiderPowerScaling = Mathf.Clamp(participatingRaiders / battleParams.totalRaiderCount, 0.1f,1f);

        Debug.Log("defenders count "+participatingDefenders);
        Debug.Log("defender power "+defenderPowerScaling);
        Debug.Log("raiders count " + participatingRaiders);
        Debug.Log("raider power " + raiderPowerScaling);

        for (int i = 0; i < participatingDefenders; i++)
        {
            //creates Defender Game Unit
            var tempDefender = Instantiate(gameUnitPrefab);
            tempDefender.transform.parent = DefendersParent.transform;
            GameUnitController tempDefenderScript = tempDefender.GetComponent<GameUnitController>();
            tempDefenderScript.UpdateGameUnitText(battleParams.defenderUserNames[i]);
            tempDefenderScript.ScaleGameUnitHealth(defenderPowerScaling);
            tempDefenderScript.SetSprite(defenderSprite);
            tempDefender.transform.position = new Vector3(defenderPosition.position.x, UnityEngine.Random.Range(-5, 5), 0);
            tempDefenderScript.myUnitType = Enums.UnitType.defender;
            tempDefenderScript.transform.localScale *= Mathf.Lerp(0.45f, 1, defenderPowerScaling);
            //Adds to list for easy access 
            defendersList.Add(tempDefenderScript);
        }
        for (int i = 0; i < battleParams.raiderUserNames.Count; i++)
        {
            if (i > battleParams.totalRaiderCount-1)
            {
                raiderPowerScaling = 0.1f;
            }
            //creates Raider Game Unit
            var tempRaider = Instantiate(gameUnitPrefab);
            tempRaider.transform.parent = RaidersParent.transform;
            GameUnitController tempRaiderScript = tempRaider.GetComponent<GameUnitController>();
            tempRaiderScript.UpdateGameUnitText(battleParams.raiderUserNames[i]);
            tempRaiderScript.ScaleGameUnitHealth(raiderPowerScaling);
            tempRaiderScript.SetSprite(raiderSprite);
            tempRaider.transform.position = new Vector3(raiderPosition.position.x, UnityEngine.Random.Range(-5, 5), 0);
            tempRaiderScript.myUnitType = Enums.UnitType.raider;
            tempRaiderScript.transform.localScale *= Mathf.Lerp(0.45f,1,raiderPowerScaling);
            //Adds to list for easy access 
            raidersList.Add(tempRaiderScript);
        }
        CheckIfBattleOver();
    }

    public void AskToFindOpponent(GameUnitController gameUnit, Enums.UnitType unitType)
    {
        switch (unitType)
        {
            case Enums.UnitType.defender: //search for avaiable raider
                foreach (var raider in raidersList)
                {
                    if (raider.myState == Enums.UnitState.idle)
                    {
                        //sets the state of the units to engaged
                        raider.myState = Enums.UnitState.engaged;
                        gameUnit.myState = Enums.UnitState.engaged;

                        //sets eachother as targets
                        raider.SetTarget(gameUnit);
                        gameUnit.SetTarget(raider);
                        break;
                    }
                }
                break;
            case Enums.UnitType.raider: //search for available defender
                foreach (var defender in defendersList)
                {
                    if (defender.myState == Enums.UnitState.idle)
                    {
                        //sets the state of the units to engaged
                        defender.myState = Enums.UnitState.engaged;
                        gameUnit.myState = Enums.UnitState.engaged;

                        //sets eachother as targets
                        defender.SetTarget(gameUnit);
                        gameUnit.SetTarget(defender);
                        break;
                    }
                }
                break;
            default:
                break;
        }
    }
    public void RemoveUnitFromList(GameUnitController unit)
    {
        if(unit.myUnitType == Enums.UnitType.defender)
        {
            defendersList.Remove(unit);
        }
        if (unit.myUnitType == Enums.UnitType.raider)
        {
            raidersList.Remove(unit);
        }
    }
    
    IEnumerator DestroyAllUnits()
    {
        Debug.Log("Destroying all units");
        yield return new WaitForSeconds(3);
        foreach (var defender in defendersList)
        {
            Destroy(defender.gameObject);
            Debug.Log("Destroying defender " + defender);
        }
        foreach (var raider in raidersList)
        {
            Destroy(raider.gameObject);
            Debug.Log("Destroying raider " + raider);
        }
        StartCoroutine(gameManager.RaidEnded());
    }

    public void CheckIfBattleOver()
    {
        if (!battleOver)
        {
            if (raidersList.Count == 0 && defendersList.Count == 0)
            {
                //draw condition!
                Debug.Log("draw");
                StartCoroutine(DestroyAllUnits());
                raidEndingScene.Draw(); 
                battleOver = true;
            }
            else if (raidersList.Count == 0 && defendersList.Count > 0)
            {
                //defenders win!
                Debug.Log("defenders");
                StartCoroutine(DestroyAllUnits());
                raidEndingScene.DefendersWin();
                battleOver = true;
            }
            else if (raidersList.Count > 0 && defendersList.Count == 0)
            {
                //raiders win!
                Debug.Log("raiders");
                StartCoroutine(DestroyAllUnits());
                raidEndingScene.RaidersWin();
                battleOver = true;
            }
        }
    }

    public static Sprite LoadPNG(string filePath)
    {
        Debug.Log("I AM LOADING A SPRITE I THINK?");
        Debug.Log(filePath);
        Debug.Log(File.Exists(filePath) + " the file exists");
        Texture2D SpriteTexture = null;
        byte[] fileData;
        fileData = File.ReadAllBytes(filePath);
        SpriteTexture = new Texture2D(2, 2);
        SpriteTexture.LoadImage(fileData); 
        Sprite NewSprite = Sprite.Create(SpriteTexture,
            new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), 2000, 0, SpriteMeshType.Tight);
        return NewSprite;
        
    }
    

}
