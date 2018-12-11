using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

    public GameObject title;
    private bool buttonPressed = false;
    // Use this for initialization
    void Start () {

        OnLoadScene("Background", true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !buttonPressed)
        {
            buttonPressed = true;
            StartCoroutine(Sequencer(

                MoveTitleScreenAway(title, title.transform.position + new Vector3(0,Screen.height/2 + 400,0),4f),  
                OnStartRoutine("Game", true, () => {
                        SceneManager.UnloadSceneAsync("Intro");
                        Message.SendMessage(MessageEnum.GAME_START);
                    }))
                    );
          
        }
    }
	
    public void OnLoadScene(string sceneName, bool setActive, System.Action callBack= null)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            StartCoroutine(OnStartRoutine(sceneName, setActive, callBack));
        }
    }

    IEnumerator MoveTitleScreenAway(GameObject obj, Vector3 targetPos, float time)
    {
        float step = 1f / time;
        float t = 0;
        while (t < 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPos, t);
            yield return null;
            t += step*Time.deltaTime;
        }
    }

    IEnumerator OnStartRoutine(string sceneName, bool setActive, System.Action callBack=null)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return null;
        }
        if (setActive)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
        }
        if (callBack != null)
        {
            callBack();
        }
;
    }

    IEnumerator Sequencer(params IEnumerator[] input)
    {
        foreach(IEnumerator routine in input)
        {
            yield return StartCoroutine(routine);
        }
    }
}
