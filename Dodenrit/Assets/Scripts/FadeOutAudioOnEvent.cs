using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAudioOnEvent : MonoBehaviour {

    public MessageEnum eventName;
    private AudioSource aSource;
    public float fadeTime;
	// Use this for initialization
	void Start () {
        aSource = GetComponent<AudioSource>();
	}

    void OnEnable()
    {
        Message.AddListener(eventName, gameObject, OnEvent);
    }

    void OnDisable()
    {
        Message.RemoveListener(eventName, gameObject, OnEvent);
    }

    void OnEvent()
    {
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        float t = 0;
        float startVolume = aSource.volume;
        while(t< 1)
        {
            aSource.volume = Mathf.Lerp(startVolume, 0, t);
            yield return null;
            t += Time.deltaTime *1f/fadeTime;
        }
    }

}
