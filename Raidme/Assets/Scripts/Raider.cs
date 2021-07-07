using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raider : MonoBehaviour
{
    [SerializeField] float health = 10;
    float damage = 1;
    GameObject targetObject;
    float speed = 5;
    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] bool isBounce = false;
    Defender targetScript;
    [SerializeField] Color32 raiderColor;
    [SerializeField] SpriteRenderer mySpriteRenderer;
    [SerializeField] TextMeshPro raiderText;
    [SerializeField] Animator raiderAnimator;

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mySpriteRenderer.color = raiderColor;
        //myRigidbody = FindObjectOfType<Rigidbody2D>();
    }
    public void UpdateRaiderText(string name)
    {
        raiderText.text = name;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isBounce & targetObject)
        {
            if (Vector3.Distance(transform.position, targetObject.transform.position) <= 0.001f)
            {
                isBounce = true;

                Vector2 bounce = new Vector2(Random.Range(-7, 7), Random.Range(-7, 7));
                raiderAnimator.SetTrigger("Hit");
                //Stretch animation
                //White flash color
                targetScript.DefenderBounce(bounce * new Vector2(-1, -1));
                targetScript.DamageDefender(damage);
                myRigidbody.AddForce(bounce, ForceMode2D.Impulse);
                StartCoroutine(ResetBounce());
            }

            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, step);
        }
        // Check if the position of the cube and sphere are approximately equal.
    }

    IEnumerator ResetBounce()
    {
        yield return new WaitForSeconds(1f);
        isBounce = false;
    }


    public void SetTarget(GameObject target)
    {
        targetObject = target;
        targetScript = target.GetComponent<Defender>();
    }

    public void DamageRaider(float damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void ScaleHealth(float healthScaling)
    {
        health = health * healthScaling;
    }
}
