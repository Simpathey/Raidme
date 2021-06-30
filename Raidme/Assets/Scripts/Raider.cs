using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raider : MonoBehaviour
{
    float Health = 5;
    float Damage = 5;
    GameObject targetObject;
    float speed = 5;
    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] bool isBounce = false;
    Defender targetScript;
    [SerializeField] Color32 raiderColor;
    SpriteRenderer mySpriteRenderer;
    TextMeshPro raiderText;

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mySpriteRenderer.color = raiderColor;
        raiderText = GetComponentInChildren<TextMeshPro>();
        //myRigidbody = FindObjectOfType<Rigidbody2D>();
    }
    public void UpdateRaiderText(string name)
    {
        raiderText.text = name;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isBounce)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, step);
            Debug.Log("Moving towards my enemy!!!");
        }

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, targetObject.transform.position) <= 0.001f)
        {
            if (!isBounce)
            {
                isBounce = true;
                Vector2 bounce = new Vector2(Random.Range(-7, 7), Random.Range(-7, 7));
                //Debug.Log(bounce);
                targetScript.DefenderBounce(bounce * new Vector2(-1, -1));
                myRigidbody.AddForce(bounce, ForceMode2D.Impulse);
                StartCoroutine(ResetBounce());
            }
        }
    }

    IEnumerator ResetBounce()
    {
        yield return new WaitForSeconds(.5f);
        isBounce = false;
    }


    public void SetTarget(GameObject target)
    {
        targetObject = target;
        targetScript = target.GetComponent<Defender>();
    }

}
