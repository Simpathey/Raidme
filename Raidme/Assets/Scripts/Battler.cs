using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battler : MonoBehaviour
{
    public GameObject ActiveDefenders;
    public GameObject ActiveRaiders;
    [SerializeField] GameObject DefenderPrefab;
    [SerializeField] GameObject RaiderPrefab;
    [SerializeField] Transform raiderPosition;
    [SerializeField] Transform defenderPosition;

    public List<Raider> raidersList = new List<Raider>();
    public List<Defender> defenders= new List<Defender>();
    float raiderPowerScaling;
    float defenderPowerScaling;


    // Start is called before the first frame update
    void Start()
    {
    }
    public void StartBattle(List<string> defenders, List<string> raiders, int raiderCount, int viewerCount)
    {
        int defenderCount = defenders.Count;
        defenderPowerScaling = defenderCount / (float)viewerCount; 
        raiderPowerScaling = raiders.Count / (float)raiderCount;
        for (int i = 0; i < defenderCount; i++)
        {
            var tempDefender = Instantiate(DefenderPrefab);
            tempDefender.transform.parent = ActiveDefenders.transform;
            Defender tempDefenderScript = tempDefender.GetComponent<Defender>();
            tempDefenderScript.UpdateDefenderText(defenders[i]);
            tempDefenderScript.ScaleHealth(defenderPowerScaling);
            tempDefender.transform.position = new Vector3(defenderPosition.position.x, Random.Range(-5, 5), 0);
          
        }
        for (int i = 0; i < raiders.Count; i++)
        {
            var tempRaider = Instantiate(RaiderPrefab);
            tempRaider.transform.parent = ActiveRaiders.transform;
            Raider tempRaiderScript = tempRaider.GetComponent<Raider>();
            tempRaiderScript.UpdateRaiderText(raiders[i]);
            tempRaiderScript.ScaleHealth(raiderPowerScaling);
            tempRaider.transform.position = new Vector3(raiderPosition.position.x, Random.Range(-5, 5), 0);
        }
        if (raiderCount >= defenderCount)
        {
            MoreRaiders();
        }
    }
    private void MoreRaiders()
    {
        var raiders = ActiveRaiders.GetComponentsInChildren<Raider>().ToList();
        var defenders = ActiveDefenders.GetComponentsInChildren<Defender>().ToList();
        for (int i = 0; i < defenders.Count; i++)
        {
            defenders[i].SetTarget(raiders[i].gameObject);
            raiders[i].SetTarget(defenders[i].gameObject);
        }
        //StartCoroutine(NextRaider());
    }
    /*
    IEnumerator NextRaider()
    {
        // assign  the next raider for the defender to target should the deffender live;
    }
    */

    private void MoreDefenders()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            List<string> fakeDefenders = new List<string> { "punchypenguin", "helpfulstranger999", "ToakFodai", "dannyfritz" };
            List<string> fakeRaiders = new List<string> { "salmonmoose", "ecoder", "alfonsosmichaelis", "matir_hacks","heyshadylady"};
            StartBattle(fakeDefenders,fakeRaiders,10,4);
        }
    }

}
