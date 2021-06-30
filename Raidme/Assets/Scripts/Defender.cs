using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Defender : MonoBehaviour
{
    float Health = 5;
    float Damage = 5;
    GameObject targetObject;
    float speed = 5;
    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] bool isBounce = false;
    [SerializeField] Color32 defenderColor;
    SpriteRenderer mySpriteRenderer;
    TextMeshPro defenderText;

    // Start is called before the first frame update
    void Start()
    {
        defenderText = GetComponentInChildren<TextMeshPro>();
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
        if (!isBounce)
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
            //Debug.Log(bounce);
            StartCoroutine(ResetBounce());
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
    }
}
