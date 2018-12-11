using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateHead : MonoBehaviour {
    private Vector3 localPosition;
    public Vector3 offset;
    public float speed;
	// Use this for initialization
	void Start () {
        localPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = localPosition+ new Vector3(0, offset.y* Mathf.Sin(speed*Time.time), 0);
	}
}
