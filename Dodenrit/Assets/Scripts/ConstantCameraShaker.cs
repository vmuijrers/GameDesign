using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantCameraShaker : MonoBehaviour {
    public Vector3 offSet;
    private Vector3 originalPosition;
	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        

    }

    void OnEnable()
    {
        Message.AddListener(MessageEnum.GAME_OVER, gameObject, StopShaking);
    }
    void OnDisable()
    {
        Message.RemoveListener(MessageEnum.GAME_OVER, gameObject, StopShaking);
    }
    public void FixedUpdate()
    {
        transform.position = originalPosition + new Vector3(Random.Range(-offSet.x, offSet.x), Random.Range(-offSet.y, offSet.y));
    }

    void StopShaking()
    {
        enabled = false;
    }
}
