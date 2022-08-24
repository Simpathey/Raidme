using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Battler : MonoBehaviour
{
    //Set up in inspector
    [SerializeField] PlayerPrefsManager playerPrefsManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] RaidEndingScene raidEndingScene;
    [SerializeField] Sprite defaultDefenderSprite;
    [SerializeField] Sprite defaultRaiderSprite;

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
    bool checkingBattleState;

    private void Start()
    {
        battleOver = true;
        checkingBattleState = false;
        LoadDefenderSprites();
        LoadRaiderSprites();
    }

    public void LoadDefenderSprites()
    {
        defenderSprite = defaultDefenderSprite;
        try
        {
            string[] filePath = Directory.GetFiles(Application.streamingAssetsPath + "/DefenderSprite/");
            defenderSprite = LoadPNG(filePath[0]);
        }
        catch (Exception)
        {
            //if exception give user feedback that they need to put a valid png in here!
        }
    }
    public void LoadRaiderSprites()
    {
        raiderSprite = defaultRaiderSprite;
        try
        {
            string[] filePath = Directory.GetFiles(Application.streamingAssetsPath + "/RaiderSprite/");
            raiderSprite = LoadPNG(filePath[0]);
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
        defenderPowerScaling = Mathf.Clamp(participatingDefenders / battleParams.totalDefenderCount, 0.1f, 1f);
        float participatingRaiders = battleParams.raiderUserNames.Count;
        raiderPowerScaling = Mathf.Clamp(participatingRaiders / battleParams.totalRaiderCount, 0.1f, 1f);

        Debug.Log("defenders count " + participatingDefenders);
        Debug.Log("total defenders count " + battleParams.totalDefenderCount);
        Debug.Log("defender power " + defenderPowerScaling);
        Debug.Log("raiders count " + participatingRaiders);
        Debug.Log("total raiders count " + battleParams.totalRaiderCount);
        Debug.Log("raider power " + raiderPowerScaling);

        StartCoroutine(StartBattleSpawn(battleParams, participatingDefenders));
    }

    IEnumerator StartBattleSpawn(BattleParams battleParams, float participatingDefenders)
    {
        yield return SpawnDefenders(battleParams, participatingDefenders);
        yield return SpawnAttackers(battleParams);
        foreach (var raider in raidersList)
        {
            raider.start = true;
        }
        foreach (var defender in defendersList)
        {
            defender.start = true;
        }
        CheckIfBattleOver();
    }

    IEnumerator SpawnAttackers(BattleParams battleParams)
    {
        for (int i = 0; i < battleParams.raiderUserNames.Count; i++)
        {
            if (i > battleParams.totalRaiderCount - 1) //if more people typed raid then there are raiders the excess are debuffed
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
            tempRaider.transform.position = new Vector3(-1.5f, 0, 0);
            tempRaiderScript.myUnitType = Enums.UnitType.raider;
            tempRaiderScript.transform.localScale = new Vector3(2,2,2);
            //Adds to list for easy access 
            raidersList.Add(tempRaiderScript);
            float duration = 0.5f;
            if (battleParams.raiderUserNames.Count > 30)
            {
                duration = (30f / battleParams.raiderUserNames.Count) / 2;
            }
            float time = 0;
            Vector3 targetPos = new Vector3(raiderPosition.position.x, UnityEngine.Random.Range(-4.5f, 4.5f), 0);
            float targetSizeParam = Mathf.Lerp(0.45f, 1, raiderPowerScaling);
            Vector3 targetSize = new Vector3(targetSizeParam, targetSizeParam, targetSizeParam);
            yield return new WaitForSeconds(duration);
            while (time<duration)
            {
                tempRaider.transform.position = Vector3.Lerp(tempRaider.transform.position, targetPos, time / duration);
                tempRaiderScript.transform.localScale = Vector3.Lerp(tempRaiderScript.transform.localScale, targetSize, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
          
        }
    }

    IEnumerator SpawnDefenders(BattleParams battleParams, float participatingDefenders)
    {
        for (int i = 0; i < participatingDefenders; i++)
        {
            if (i > battleParams.totalDefenderCount - 1) //if more people typed deffend then there are ppl in chat the excess are debuffed
            {
                defenderPowerScaling = 0.1f;
            }
            //creates Defender Game Unit
            var tempDefender = Instantiate(gameUnitPrefab);
            tempDefender.transform.parent = DefendersParent.transform;
            GameUnitController tempDefenderScript = tempDefender.GetComponent<GameUnitController>();
            tempDefenderScript.UpdateGameUnitText(battleParams.defenderUserNames[i]);
            tempDefenderScript.ScaleGameUnitHealth(defenderPowerScaling);
            tempDefenderScript.SetSprite(defenderSprite);
            tempDefender.transform.position = new Vector3(1.5f, 0, 0);
            tempDefenderScript.myUnitType = Enums.UnitType.defender;
            tempDefenderScript.transform.localScale *= Mathf.Lerp(0.45f, 1, defenderPowerScaling);
            //Adds to list for easy access 
            defendersList.Add(tempDefenderScript);
            float time = 0;
            float duration = 0.5f;
            if (participatingDefenders > 30)
            {
                duration = (30f/participatingDefenders)/2;   
            }
            Vector3 targetPos = new Vector3(defenderPosition.position.x, UnityEngine.Random.Range(-4.5f, 4.5f), 0);
            float targetSizeParam = Mathf.Lerp(0.45f, 1, defenderPowerScaling);
            Vector3 targetSize = new Vector3(targetSizeParam, targetSizeParam, targetSizeParam);
            yield return new WaitForSeconds(duration);
            while (time < duration)
            {
                tempDefender.transform.position = Vector3.Lerp(tempDefender.transform.position, targetPos, time / duration);
                tempDefenderScript.transform.localScale = Vector3.Lerp(tempDefenderScript.transform.localScale, targetSize, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
        }
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
        if (checkingBattleState) { return; }
        checkingBattleState = true;
        StartCoroutine(BattleOverCoroutine());
    }

    IEnumerator BattleOverCoroutine()
    {
        if (!battleOver)
        {
            yield return new WaitForSeconds(0.1f);
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
        checkingBattleState = false;
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

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A)) // THIS IS FAKE I WILL DELETE LATER LOLO LOLO LOOL LOOOOOOOOOL CATJAM
        {
            BattleParams fakeRaid = new BattleParams();
            fakeRaid.defenderUserNames = new List<string> { "reallydecent", "jashmead", "tap_ghoul", "aietes__", "rossTB", "tr0ydf", "jashmead", "tap_ghoul", "aietes__", "rossTB", "tr0ydf", "jashmead", "tap_ghoul", "aietes__", "rossTB", "tr0ydf", "jashmead", "tap_ghoul", "aietes__", "rossTB", "tr0ydf", "jashmead", "tap_ghoul", "aietes__", "rossTB", "tr0ydf", "jashmead", "tap_ghoul", "aietes__", "rossTB", "tr0ydf", "jashmead", "tap_ghoul", "aietes__", "rossTB", "tr0ydf", "jashmead", "tap_ghoul", "aietes__", "rossTB", "tr0ydf" };
            fakeRaid.raiderUserNames = new List<string> { "RyanKHawkins", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67", "Leo_churrasquerio", "Moosedoesstuff", "getfisted", "cyberangel67" };
            fakeRaid.totalDefenderCount = 50;
            fakeRaid.totalRaiderCount = 120;
            StartBattle(fakeRaid);
        }
    }
}
