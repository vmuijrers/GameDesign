using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour {

    public Texture2D[] textures;
    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<Renderer>().material.SetTexture("_Mask",textures[Random.Range(0, textures.Length)]);
    }
}
