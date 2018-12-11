using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteUIAnimationComponent : MonoBehaviour {

    private Image spriteRenderer;
    public Sprite[] frames;
    public float animationSpeed;
    public int currentFrame = 0;
    internal float t = 0;
    public string baseAnimationName = "Horse_1";
    public System.Action OnSwapSprite;
    public bool useRandomStartT = true;
    // Use this for initialization
    public void Awake()
    {
        spriteRenderer = GetComponentInChildren<Image>();
        if(useRandomStartT)
            t = Random.Range(0, 1f);
        frames = Resources.LoadAll<Sprite>(baseAnimationName);
        spriteRenderer.sprite = frames[currentFrame];
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        t += Time.fixedDeltaTime;
        if (t >= animationSpeed)
        {
            if(OnSwapSprite != null)
            {
                OnSwapSprite();
            }
            t = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
        }


    }
}
