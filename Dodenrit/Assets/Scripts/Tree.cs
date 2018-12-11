using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    public Sprite[] textures;
	// Use this for initialization
	void Start () {
        
       
    }

    public void PickTexture(bool isBackgroundTree)
    {
        if (isBackgroundTree)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = textures[Random.Range(0, 3)];
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().sprite = textures[Random.Range(3, textures.Length)];
        }
        
    }

}
