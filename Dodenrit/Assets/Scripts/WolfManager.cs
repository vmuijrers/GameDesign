using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using VaalsECS;
public class WolfManager : MonoBehaviour {

    private List<Wolf> wolves = new List<Wolf>();
    public float timeToSpawnNewWolf = 10f;
    public int wolvesNeededForAttack = 4;
    private int currentWolvesActive = 0;
    private bool eating = false;
    public float eatingTime = 5f;
    private float xPosWolves = -100;
    public float moveSpeedWolves = 12f;
    private float distancePerWolf = 15;

    public float slowMotionDist = 10f;
    public PlayerController carriage;

    public GameObject UIScore;
    public TextMeshProUGUI distanceTravelledUI;
    public GameObject wolfPrefab;
    private bool isGameOver = false;

    //Feedback for slowmotion
    public GameObject SlowMotionFeedback;
    private KeyCode b1, b2;
    private bool slowMotionActive = false;
    public static float slowmotionFactor = 0.1f;
    private bool needButton1 = true;
    public GameObject whippingSlotPosition;
    public bool lastSaveDone = false;
    public Text buttonOneText, buttonTwoText;
    private float hitPoints = 0;
    public float repairSpeedLastSave = 10;
    private Character chosenOne;
    public Transform greenBarUI;
    private float timer = 5;
    private float distToCarriage;
    public float DistToCarriage{
        get { return distToCarriage; }
        set
        {
            if(distToCarriage > 0 && value <= 0 && !carriage.gameIsLost)
            {
                carriage.gameIsLost = true;
                Message.SendMessage(MessageEnum.GAME_OVER);
            }

            if(distToCarriage > slowMotionDist && value <= slowMotionDist && !carriage.gameIsLost && !lastSaveDone)
            {
                Message.SendMessage(MessageEnum.SLOWMOTION);
                lastSaveDone = true;
            }

            float threshold = (wolvesNeededForAttack - currentWolvesActive) * distancePerWolf + distancePerWolf;
            if (value <= threshold && distToCarriage > threshold && currentWolvesActive != wolvesNeededForAttack)
            {
                ActivateWolf();
            }
            distToCarriage = value;
            distToCarriage = Mathf.Max(distToCarriage, 0);

        }
    }
    void OnEnable()
    {
        Message<Character>.AddListener(MessageEnum.ON_SACRIFICE, gameObject, OnSacrifice);
        Message.AddListener(MessageEnum.GAME_OVER, gameObject, OnGameOver);
        Message.AddListener(MessageEnum.SLOWMOTION, gameObject, OnSlowMotion);
    }

    void OnDisable()
    {
        Message<Character>.RemoveListener(MessageEnum.ON_SACRIFICE, gameObject, OnSacrifice);
        Message.RemoveListener(MessageEnum.GAME_OVER, gameObject, OnGameOver);
        Message.RemoveListener(MessageEnum.SLOWMOTION, gameObject, OnSlowMotion);
    }

    // Use this for initialization
    void Start () {

        slowmotionFactor = 1f;
        SlowMotionFeedback.SetActive(false);
        UIScore.SetActive(false);
        Debug.Log("Game Started!");
        for (int i = 0; i < wolvesNeededForAttack; i++)
        {
            GameObject wolf = Instantiate(wolfPrefab);
            wolves.Add(wolf.GetComponent<Wolf>());
            wolves[i].transform.position = new Vector2(-ScreenSize.halfScreenSize.x - 2f, Random.Range(-ScreenSize.halfScreenSize.y+1,-1f));
            wolves[i].transform.SetParent(transform);
        }
       
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (!eating && !slowMotionActive)
        {
            xPosWolves += moveSpeedWolves * Time.deltaTime;
        }
        DistToCarriage = carriage.distanceTravelled - xPosWolves;

        if(DistToCarriage <= 0)
        {
            carriage.transform.position = Vector3.MoveTowards(carriage.transform.position, new Vector3(-20, carriage.transform.position.y, 0), 10f * Time.deltaTime);
            foreach (Wolf w in wolves)
            {

                w.transform.position = Vector3.MoveTowards(w.transform.position, new Vector2(carriage.transform.position.x, w.transform.position.y), 10f * Time.deltaTime);

            }

        }
        else
        if(DistToCarriage < slowMotionDist)
        {
            
            carriage.transform.position = Vector3.MoveTowards(carriage.transform.position, new Vector3(-5 + Mathf.Min(distToCarriage, 5), carriage.transform.position.y, 0), 2f * Time.deltaTime);
        }
        else
        {
            carriage.transform.position = Vector3.MoveTowards(carriage.transform.position, new Vector3(carriage.xOffSet, carriage.transform.position.y, 0), 2f * Time.deltaTime);
        }

        

    }

    void Update()
    {
        
        
        if (slowMotionActive)
        {
            Debug.Log(distToCarriage);
            if(eating)
            {
                StopSlowmotion();
                return;
            }
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                StopSlowmotion();
                Message.SendMessage(MessageEnum.GAME_OVER);
                return;

            }
            if (hitPoints >= 100)
            {
                StopSlowmotion();
                carriage.distanceTravelled += 100;
            }

            if ((Input.GetKeyDown(b1) && needButton1) || (!needButton1 && Input.GetKeyDown(b2)))
            {
                needButton1 = !needButton1;
                hitPoints += repairSpeedLastSave;
                greenBarUI.localScale = new Vector3(hitPoints / 100f, greenBarUI.transform.localScale.y, 1);
            }
            
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Message.SendMessage(MessageEnum.SLOWMOTION);
        //}
        
    }

    void StopSlowmotion()
    {
        if(chosenOne != null)
        {
            chosenOne.StopWhipping();
            chosenOne.GoToRestingSlot();
        }

        slowMotionActive = false;
        slowmotionFactor = 1f;
        SlowMotionFeedback.SetActive(false);
    }
    void OnSlowMotion()
    {
        Debug.Log("Slow Motion Started!");
        b1 = (KeyCode)Random.Range(97, 97 + 26);
        b2 = (KeyCode)Random.Range(97, 97 + 26);
        string str1 = b1.ToString();
        buttonOneText.text = char.ToUpper(str1[0]).ToString();
        string str2 = b2.ToString();
        buttonTwoText.text = char.ToUpper(str2[0]).ToString();

        List<Entity> allCharacters = EntityManager.GetEntities(null, typeof(Character));
        Entity c = allCharacters[Random.Range(0, allCharacters.Count - 1)];
        chosenOne = c.GetCachedComponent<Character>();
        chosenOne.currentSlot.OnRemoveCharacter(chosenOne);
        chosenOne.transform.position = whippingSlotPosition.transform.position;
        chosenOne.StopRepair();
        chosenOne.StartWhipping();


        hitPoints = 0;
        slowMotionActive = true;
        slowmotionFactor = 0.1f;
        SlowMotionFeedback.SetActive(true);
    }


    void OnGameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            AudioManager.Instance.PlayAudio(transform.position, "Wolf Bite", is3D: false);
            StartCoroutine(EndSequence());
        }

    }

    IEnumerator EndSequence()
    {
        UIScore.SetActive(true);
        string annoy = "";
        if (carriage.distanceTravelled > 3000)
        {
            annoy = "Why did you sleigh past Omsk?";
        }

        distanceTravelledUI.text = "Distance Travelled: " + carriage.distanceTravelled.ToString("F0") + " wersta\n" + "Distance To Omsk: " + (3000 - carriage.distanceTravelled).ToString("F0")+ " wersta\n" + annoy+"\nPress 'R' to Retry";
        distanceTravelledUI.gameObject.SetActive(true);
        //float t = 0;
        //while (t < 1)
        //{
        //    //distanceTravelledUI.transform.position = Vector3.Lerp(distanceTravelledUI.transform.position, new Vector3(-559, -336, 0), t);
        //    yield return null;
        //    t += Time.deltaTime;
        //}
        while (true)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.R))
            {
                
                Message.SendMessage(MessageEnum.RESTART_GAME);
            }
        }

        

    }

    void ActivateWolf()
    {
        currentWolvesActive+=1;
        StartCoroutine(ActivateWolfRoutine());
    }

    IEnumerator ActivateWolfRoutine() { 

        //Play howling wolf sound
        AudioManager.Instance.PlayAudio(transform.position, "Wolf Spawn",volume:0.4f, is3D:false);
        Wolf wolf = wolves[currentWolvesActive - 1];
        //Debug.Log("Current Wolf: " + currentWolvesActive);
        float targetPos = -ScreenSize.halfScreenSize.x + Random.Range(1, 3f);
        wolf.transform.position = new Vector3(-ScreenSize.halfScreenSize.x + -3f, wolf.transform.position.y, 0);
        while (wolf.transform.position.x < targetPos)
        {
            yield return null;
            wolf.transform.position = Vector3.MoveTowards(wolf.transform.position, new Vector2(targetPos, wolf.transform.position.y), 1f *Time.deltaTime);
        }
    }

    void OnSacrifice(Character character)
    {
        if (carriage.gameIsLost) { return; }
        StartCoroutine(EatCharacterRoutine(character));

    }

    IEnumerator EatCharacterRoutine(Character character)
    {
        eating = true;

        currentWolvesActive = 0;
        while (character.gameObject.activeSelf)
        {
            foreach(Wolf w in wolves)
            {
                if(w.transform.position.x < character.transform.position.x)
                {
                    w.transform.position = Vector3.MoveTowards(w.transform.position, new Vector2(character.transform.position.x, w.transform.position.y), 1f * Time.deltaTime);
                }
                else
                {
                    w.transform.position = new Vector2(character.transform.position.x, w.transform.position.y);
                }
            }
            yield return null;
        }

        //Eat The Character!
        //play scary eating sound!
        AudioManager.Instance.PlayAudio(transform.position, "Wolf Bite", volume: 0.6f, is3D: false);
        while(DistToCarriage < wolvesNeededForAttack * distancePerWolf + distancePerWolf)
        {
            yield return null;
        }
        
        currentWolvesActive = 0;
        foreach (Wolf wolf in wolves)
        {
            wolf.transform.position = new Vector3(-ScreenSize.halfScreenSize.x + -3f, wolf.transform.position.y, 0);
        }
        eating = false;

    }

}
