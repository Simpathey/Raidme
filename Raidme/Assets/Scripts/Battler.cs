using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battler : MonoBehaviour
{
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

    //TO DO DELETE
    // public List<Raider> idleRaiders = new List<Raider>();
    //public List<Defender> idleDefenders = new List<Defender>();
    // [SerializeField] GameObject DefenderPrefab;
    //[SerializeField] GameObject RaiderPrefab;

    public void StartBattle(List<string> defenderUserNames, float totalDefenderCount, List<string> raiderUserNames, float totalRaiderCount)
    {
        //calculates the participation rate of each side, clamps value between 0.1 and 1
        float participatingDefenders = defenderUserNames.Count;
        defenderPowerScaling = Mathf.Clamp(participatingDefenders / totalDefenderCount,0.1f,1f);
        float participatingRaiders = raiderUserNames.Count;
        raiderPowerScaling = Mathf.Clamp(participatingRaiders / totalRaiderCount,0.1f,1f);


        for (int i = 0; i < participatingDefenders; i++)
        {
            //creates Defender Game Unit
            var tempDefender = Instantiate(gameUnitPrefab);
            tempDefender.transform.parent = DefendersParent.transform;
            GameUnitController tempDefenderScript = tempDefender.GetComponent<GameUnitController>();
            tempDefenderScript.UpdateGameUnitText(defenderUserNames[i]);
            tempDefenderScript.ScaleGameUnitHealth(defenderPowerScaling);
            tempDefender.transform.position = new Vector3(defenderPosition.position.x, Random.Range(-5, 5), 0);
            tempDefenderScript.myUnitType = Enums.UnitType.defender;
            //Adds to list for easy access 
            defendersList.Add(tempDefenderScript);
        }
        for (int i = 0; i < raiderUserNames.Count; i++)
        {
            //creates Raider Game Unit
            var tempRaider = Instantiate(gameUnitPrefab);
            tempRaider.transform.parent = RaidersParent.transform;
            GameUnitController tempRaiderScript = tempRaider.GetComponent<GameUnitController>();
            tempRaiderScript.UpdateGameUnitText(raiderUserNames[i]);
            tempRaiderScript.ScaleGameUnitHealth(raiderPowerScaling);
            tempRaider.transform.position = new Vector3(raiderPosition.position.x, Random.Range(-5, 5), 0);
            tempRaiderScript.myUnitType = Enums.UnitType.raider;
            //Adds to list for easy access 
            raidersList.Add(tempRaiderScript);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            List<string> fakeDefenders = new List<string> { "BeginBot" ,"Teej_dv","TheAltF4Stream","BashBunni","roxkstar74"};
            List<string> fakeRaiders = new List<string> { "ThePrimeagen", "bun9000", "Mastermndio", "erikdotdev","melkeydev"};
            StartBattle(fakeDefenders, 5,fakeRaiders,5);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            List<string> fakeDefenders = new List<string> { "punchypenguin", "helpfulstranger999", "ToakFodai", "dannyfritz", "ryanKHawkins", "shewhoexists", "third_tier_gaming", "vimlark", "kaioora" };
            List<string> fakeRaiders = new List<string> { "alfonsosmichaelis", "matir_hacks", "heyshadylady", "alekcie", "Lutfisk", "Ericklodocs"};
            StartBattle(fakeDefenders, 20, fakeRaiders, 6);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            List<string> fakeDefenders = new List<string> { "alfonsosmichaelis", "matir_hacks", "heyshadylady", "alekcie", "Lutfisk", "Ericklodocs", "teej_dv" };
            List<string> fakeRaiders = new List<string> { "punchypenguin", "helpfulstranger999", "ToakFodai", "dannyfritz", "ryanKHawkins", "shewhoexists", "third_tier_gaming", "vimlark", "kaioora" };
            StartBattle(fakeDefenders, 6, fakeRaiders, 20);
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
    /*
    private void MoreRaiders()
    {
        Debug.Log("There are more raiders");
        var raiders = ActiveRaiders.GetComponentsInChildren<Raider>().ToList();
        var defenders = ActiveDefenders.GetComponentsInChildren<Defender>().ToList();
        for (int i = 0; i < defenders.Count; i++)
        {
            defenders[i].SetTarget(raiders[i].gameObject); //defender set target
            raiders[i].SetTarget(defenders[i].gameObject);
        }
        idleRaiders = raiders.Skip(defenders.Count).ToList();
        CalculateIdleRaiders();
        //StartCoroutine(NextRaider());
    }
    public void SetRaiderTarget(Defender defender)
    {
        if (idleRaidersCount > 0)
        {
            if (!idleRaiders[0].isIdle)
            {
                defender.isIdle = true;
                defender.SetTarget(idleRaiders[0].gameObject); //defender set target
                idleRaiders[0].SetTarget(defender.gameObject);
                idleRaiders.RemoveAt(0);
                CalculateIdleRaiders();
            }
        }
        else
        {
            idleDefenders.Add(defender);
            CalculateIdleDefenders();
        }
    }
    public void SetDefenderTarget(Raider raider)
    {
        if (idleDefendersCount > 0)
        {
            if (!idleDefenders[0].isIdle)
            {
                raider.isIdle = true;
                raider.SetTarget(idleDefenders[0].gameObject);
                idleDefenders[0].SetTarget(raider.gameObject); //defender set target
                idleDefenders.RemoveAt(0);
                CalculateIdleDefenders();
            }
        }
        else
        {
            idleRaiders.Add(raider);
            CalculateIdleRaiders();
        }
    }

    private void CalculateIdleRaiders()
    {
        idleRaidersCount = idleRaiders.Count;
    }
    public void CalculateIdleDefenders()
    {
        idleDefendersCount = idleDefenders.Count;
    }
    /*
    IEnumerator NextRaider()
    {
        // assign  the next raider for the defender to target should the deffender live;
    }
    

    private void MoreDefenders()
    {
        Debug.Log("There are more raiders");
        var raiders = ActiveRaiders.GetComponentsInChildren<Raider>().ToList();
        var defenders = ActiveDefenders.GetComponentsInChildren<Defender>().ToList();
        for (int i = 0; i < raiders.Count; i++)
        {
            defenders[i].SetTarget(raiders[i].gameObject); //defenders set target 
            raiders[i].SetTarget(defenders[i].gameObject);
        }
        idleDefenders = defenders.Skip(raiders.Count).ToList();
        CalculateIdleDefenders();
        //StartCoroutine(NextRaider());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            List<string> fakeDefenders = new List<string> { "punchypenguin", "helpfulstranger999", "ToakFodai", "dannyfritz", "ryanKHawkins", "shewhoexists", "third_tier_gaming","vimlark", "kaioora" };
            List<string> fakeRaiders = new List<string> { "alfonsosmichaelis", "matir_hacks","heyshadylady","alekcie","Lutfisk","Ericklodocs","teej_dv"};
            StartBattle(fakeDefenders,fakeRaiders,10,22);
        }
    }

    public void RemoveDefenderFromIdleList(Defender defender)
    {
        if (idleDefenders.Contains(defender))
        {
            idleDefenders.Remove(defender);
        }
    }
    public void RemoveRaiderFromIdleList(Raider raider)
    {
        if (idleRaiders.Contains(raider))
        {
            idleRaiders.Remove(raider);
        }
    }*/
}
