using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAutoDepthLayer : MonoBehaviour {

    private SpriteRenderer[] renderers;
    private int[] baseSortLayer;
	// Use this for initialization
	void Start () {
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        baseSortLayer = new int[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            baseSortLayer[i] = renderers[i].sortingOrder;
        } 
       
	}
	
	// Update is called once per frame
	void LateUpdate () {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sortingOrder = -(int)(transform.position.y * 1000) + baseSortLayer[i];
        }

	}
}
