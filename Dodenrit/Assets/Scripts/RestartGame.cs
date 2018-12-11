using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        		
	}

    void OnEnable()
    {
        Message.AddListener(MessageEnum.RESTART_GAME, gameObject, OnRestart);
    }

    void OnDisable()
    {
        Message.RemoveListener(MessageEnum.RESTART_GAME, gameObject, OnRestart);
    }
    void OnRestart()
    {
        StartCoroutine(RestartSequence());
    }

    IEnumerator RestartSequence()
    {
        yield return SceneManager.UnloadSceneAsync("Game");
        yield return SceneManager.LoadSceneAsync("Game",LoadSceneMode.Additive);
        Message.SendMessage(MessageEnum.GAME_START);
    }

	

}
