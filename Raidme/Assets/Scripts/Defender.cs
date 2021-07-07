using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Defender : MonoBehaviour
{
    [SerializeField] float health = 10; //for debug
    float defenderDamage = 1;
    public GameObject targetObject;
    Raider targetScript;
    float speed = 5;
    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] bool isBounce = false;
    [SerializeField] Color32 defenderColor;
    [SerializeField] SpriteRenderer mySpriteRenderer;
    [SerializeField] TextMeshPro defenderText;
    [SerializeField] Animator defenderAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //myRigidbody = FindObjectOfType<Rigidbody2D>();
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mySpriteRenderer.color = defenderColor;
    }
    public void UpdateDefenderText(string name)
    {
        defenderText.text = name;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isBounce & targetObject)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, step);
        }
        // Check if the position of the cube and sphere are approximately equal.
    }
    public void DefenderBounce(Vector2 bounce)
    {
        if (!isBounce)
        {
            isBounce = true;
            myRigidbody.AddForce(bounce, ForceMode2D.Impulse);
            defenderAnimator.SetTrigger("Hit");
            //sqash animation play
            //flash white
            StartCoroutine(ResetBounce());
        }
    }

    IEnumerator ResetBounce()
    {
        yield return new WaitForSeconds(1f);
        isBounce = false;
    }


    public void SetTarget(GameObject target)
    {
        targetObject = target;
        targetScript = targetObject.GetComponent<Raider>();
    }

    public void DamageDefender(float damage)
    {
        targetScript.DamageRaider(defenderDamage);
        health = health - damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void ScaleHealth(float scaling)
    {
        //scales the health to the percentage participation
        health = health * scaling; 
    }
}
