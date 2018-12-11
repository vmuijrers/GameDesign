using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour {

    private List<GameObject> scrollObjects = new List<GameObject>();
    public int trees = 10;
    public int bushes = 6;
    public float startScrollSpeed = 10f;
    public float gameScrollSpeed = 10f;
    public static float scrollSpeed;
    public float HorizontalBorder = 9f;
    private float currentScrollSpeed;

    void OnEnable()
    {
        Message.AddListener(MessageEnum.GAME_OVER, gameObject, OnGameOver);
        Message.AddListener(MessageEnum.GAME_START, gameObject, OnGameStart);
    }
    void OnDisable()
    {
        Message.RemoveListener(MessageEnum.GAME_OVER, gameObject, OnGameOver);
        Message.RemoveListener(MessageEnum.GAME_START, gameObject, OnGameStart);
    }
    // Use this for initialization
    void Start () {
        currentScrollSpeed = startScrollSpeed;
        Random.InitState(Random.Range(0, 100000));


        for(int i =0; i < trees; i++)
        {
            GameObject go = ObjectPooler.instance.RequestObjectOfType<Tree>();
            float yScale = Random.Range(1, 1);
            go.transform.localScale = new Vector3(yScale, yScale, 1);
            go.transform.localScale = Vector3.Scale(go.transform.localScale, new Vector3(Random.value > 0.5f ? -1 : 1, 1, 1));
            float Y = Random.Range(-1f, 0.25f);

            go.GetComponent<Tree>().PickTexture(Y>-0.5f);
            go.transform.position = new Vector3( trees/ (HorizontalBorder) * i + Random.Range(-1, 1), Y, 0);
            //go.GetComponent<SpriteRenderer>().sortingOrder = (int)-Y;
            go.transform.parent = transform;
            scrollObjects.Add(go);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        scrollSpeed = currentScrollSpeed * WolfManager.slowmotionFactor;

        foreach (GameObject go in scrollObjects)
        {
            go.transform.position -= Vector3.right * scrollSpeed *Time.deltaTime;
            if (go.transform.position.x < -HorizontalBorder-2f)
            {

                float yScale = Random.Range(1, 1);
                go.transform.localScale = new Vector3(yScale, yScale, 1);
                go.transform.localScale = Vector3.Scale(go.transform.localScale, new Vector3(Random.value > 0.5f ? -1 : 1, 1, 1));
                float Y = Random.Range(-1f, 0.25f);
                Tree t = go.GetComponent<Tree>();
                if(t != null)
                {
                    go.GetComponent<Tree>().PickTexture(Y > -0.5f);
                }
                

                go.transform.position = new Vector3(HorizontalBorder + 1f+ Random.Range(-1f, 1f), Y, 0);
            }
        }
	}

    public float Remap(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
    }

    void OnGameOver()
    {
        StartCoroutine(OnGameOverRoutine());
    }

    void OnGameStart()
    {
        StopAllCoroutines();
        StartCoroutine(IncreaseScrollSpeed(gameScrollSpeed));
    }

    IEnumerator IncreaseScrollSpeed(float target)
    {
        float t = 0;
        
        while(t < 1)
        {
            currentScrollSpeed = Mathf.Lerp(currentScrollSpeed, target, t);
            yield return null;
            t += Time.deltaTime;
        }
        
    }
    

    IEnumerator OnGameOverRoutine()
    {
        float t = 0;
        float value = gameScrollSpeed;
        while (t < 1)
        {
            t += Time.deltaTime * 1/20f;
            yield return null;
            currentScrollSpeed = Mathf.Lerp(value, startScrollSpeed, t); 
        }
    }
}
