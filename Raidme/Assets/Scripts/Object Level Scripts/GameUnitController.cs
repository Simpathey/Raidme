using System.Collections;
using UnityEngine;
using TMPro;

public class GameUnitController : MonoBehaviour
{
    //Unit properties 
    public float health = 10;
    public float damage = 1;
    public float speed = 5;

    //Components on Unit, Serialized in Inspector
    [SerializeField] TextMeshPro gameUnitText;
    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] Animator myAnimator;
    [SerializeField] SpriteRenderer mySpriteRenderer;

    [SerializeField] private bool seachingForOpponent = false; //to do unserialize 
    private bool bouncing = false;
    public bool start = false;
    public GameUnitController target;
    public Enums.UnitType myUnitType;
    public Enums.UnitState myState;
    Coroutine searching;

    Battler battler;
    
    // Start is called before the first frame update
    void Start()
    {
        battler = FindObjectOfType<Battler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!start) { return; }
        Move();
        UpdateState();
    }
    private void UpdateState()
    {
        if (!seachingForOpponent)
        {
            seachingForOpponent = true;
            searching = StartCoroutine(SearchForOpponent());
        }
    }

    private void Move()
    {
        if (target)
        {
            if (!bouncing)
            {
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

                //Checks if we reached target object after moving
                if (Vector3.Distance(transform.position, target.transform.position) <= 0.001f)
                {
                    bouncing = true;
                    if (myUnitType == Enums.UnitType.raider)
                    {
                        //triggers dealing damage for both defender and attacker
                        DealDamage(target.damage);
                        target.DealDamage(damage);
                        //triggers the bounce for both defender and attacker
                        Vector2 bounce = new Vector2(Random.Range(-7, 7), Random.Range(-7, 7));
                        StartCoroutine(Bounce(bounce, 1));
                        StartCoroutine(target.Bounce(bounce, -1));
                    }
                }
            }
        }
    }

    IEnumerator SearchForOpponent()
    {
        yield return new WaitForSeconds(Random.Range(0, 4f));
        battler.AskToFindOpponent(this,myUnitType);
    }

    public void SetTarget(GameUnitController myTarget)
    {
        StopCoroutine(searching);
        target = myTarget;
    }

    public void SetStateToIdle()
    {
        myState = Enums.UnitState.idle;
    }

    public void SetSprite(Sprite newSprite)
    {
        mySpriteRenderer.sprite = newSprite;
    }

    public void UpdateGameUnitText(string name)
    {
        gameUnitText.text = name;
    }

    public void ScaleGameUnitHealth(float scalingFactor)
    {
        health = health * scalingFactor;
    }

    public void DealDamage(float damage)
    {
        health = health - damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Bounce(Vector2 bounceVector, int direction)
    {
        myRigidbody.AddForce(bounceVector * direction, ForceMode2D.Impulse);
        myAnimator.SetTrigger("Hit");
        yield return new WaitForSeconds(1);
        bouncing = false;
    }

    private void OnDestroy()
    {
        battler.RemoveUnitFromList(this);
        battler.CheckIfBattleOver();
        if (target)
        {
            target.seachingForOpponent = false;
            target.myState = Enums.UnitState.idle;
            target.target = null;
        }
    }
}
