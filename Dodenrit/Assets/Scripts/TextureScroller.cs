using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour {

    MeshRenderer spriteRenderer;
    public string TextureName;
    public float multiplier = 1f;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        spriteRenderer.material.mainTextureOffset += new Vector2(BackGroundScroller.scrollSpeed *Time.deltaTime * multiplier, 0);
    }
}
