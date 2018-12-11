using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    public Sprite[] frames;
    public float animationSpeed;
    private int currentFrame = 0;
    float t = 0;
    private Vector3 targetPos;
    private float moveSpeed = 1f;
    private Vector3 offSet;
    private Vector3 originalPos;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        t = Random.Range(0, 1f);
        frames = Resources.LoadAll<Sprite>("Wolf_" + Random.Range(1, 4).ToString());
        StartCoroutine(GetNewTargetPos());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        t += Time.deltaTime;
        if(t > animationSpeed)
        {
            t = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
        }
        spriteRenderer.transform.localPosition = Vector3.MoveTowards(spriteRenderer.transform.localPosition, offSet, moveSpeed * Time.deltaTime);

	}

    IEnumerator GetNewTargetPos()
    {

        yield return new WaitForSeconds(Random.Range(3, 5f));
        offSet = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        StartCoroutine(GetNewTargetPos());
    }
}
