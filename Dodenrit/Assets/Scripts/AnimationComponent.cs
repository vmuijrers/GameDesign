using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimationComponent : MonoBehaviour {


    private SpriteRenderer spriteRenderer;
    public Sprite[] frames;
    public float animationSpeed;
    private int currentFrame = 0;
    float t = 0;
    public string baseAnimationName = "Horse_1";
    // Use this for initialization
    public void Start () {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        t = Random.Range(0, 1f);
        frames = Resources.LoadAll<Sprite>(baseAnimationName);

    }

    // Update is called once per frame
    public void FixedUpdate () {
        t += Time.deltaTime;
        if(t > animationSpeed)
        {
            t = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
        }
        

	}
}
