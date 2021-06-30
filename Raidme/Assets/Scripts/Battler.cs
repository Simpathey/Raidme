using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battler : MonoBehaviour
{
    [SerializeField] GameObject ActiveDefenders;
    [SerializeField] GameObject ActiveRaiders;
    [SerializeField] GameObject DefenderPrefab;
    [SerializeField] GameObject RaiderPrefab;

    List<Raider> raiders;
    List<Defender> defenders;
    float raiderPowerScaling;
    float defenderPowerScaling;


    // Start is called before the first frame update
    void Start()
    {
        raiders = ActiveRaiders.transform.GetComponentsInChildren<Raider>().ToList();
        defenders = ActiveDefenders.transform.GetComponentsInChildren<Defender>().ToList();
        raiders[0].SetTarget(defenders[0].gameObject);
        defenders[0].SetTarget(raiders[0].gameObject);
    }
    public void StartBattle(List<string> defenders, List<string> raiders, int raiderCount, int viewerCount)
    {
        int defenderCount = defenders.Count;
        defenderPowerScaling = defenderCount / viewerCount;
        raiderPowerScaling = raiders.Count / raiderCount;
        for (int i = 0; i < defenderCount; i++)
        {
            var tempDefender = Instantiate(DefenderPrefab);
            tempDefender.transform.parent = ActiveDefenders.transform;
            tempDefender.GetComponent<Defender>().UpdateDefenderText(defenders[i]);
        }
    }
    private void MoreAttackers()
    {

    }
    private void MoreDefenders()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
