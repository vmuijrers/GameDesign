using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUI : MonoBehaviour {

    public GameObject objToAnimate;
    private Vector3 originalPos;
    public Vector3 offSet = new Vector3(0, 0, 0);
    public float animationSpeed = 0.5f;

    private bool isOffSet= false;
    private SpriteUIAnimationComponent spriteUI;
    // Use this for initialization
    void Start () {
        spriteUI = GetComponentInChildren<SpriteUIAnimationComponent>();
        
        originalPos = objToAnimate.transform.localPosition;
        Debug.Log(originalPos);
        if (spriteUI != null)
            spriteUI.OnSwapSprite += OnSwapSprite;
    }
    void OnDestroy()
    {
        if(spriteUI != null)
            spriteUI.OnSwapSprite -= OnSwapSprite;
    }
	
	// Update is called once per frame
	void OnSwapSprite() {

            isOffSet = !isOffSet;
            if(isOffSet)
            {
                objToAnimate.transform.localPosition = originalPos + offSet;
            }
            else
            {
                objToAnimate.transform.localPosition = originalPos;
            }
    }
}
