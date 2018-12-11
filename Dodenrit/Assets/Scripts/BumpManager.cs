using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VaalsECS;
public class BumpManager : MonoBehaviour {

    public float minWait = 5f;
    public float maxWait = 10f;
    public float bumpScreenPercentage = 0.75f;
    public SpriteRenderer branchObstacle;

    public GameObject showBranchAlert;
    public GameObject branchUI;
    public KeyCode keyToDuck = KeyCode.Space;
    private float threshold;
    private float xPos;
    public float XPos
    {
        get { return xPos; }
        set
        {
            if (value <= threshold && xPos > threshold)
            {
                CheckForObstacle();
            }
            xPos = value;
        }

    }

    void OnEnable()
    {
        Message.AddListener(MessageEnum.GAME_OVER, gameObject, OnGameOver);
    }
    void OnDisable()
    {
        Message.RemoveListener(MessageEnum.GAME_OVER, gameObject, OnGameOver);
    }
    private List<Slot> breakableSlots = new List<Slot>();
	// Use this for initialization
	void Start () {
        branchUI.SetActive(false);
        showBranchAlert.SetActive(false);
        threshold = bumpScreenPercentage * ScreenSize.halfScreenSize.x;
        List<Entity> ents= EntityManager.GetEntities(null, typeof(BreakableSlot), typeof(Slot));
        foreach(Entity e in ents)
        {
            breakableSlots.Add(e.GetCachedComponent<Slot>());
        }
        
        //xPos = bump.transform.position.x;
        StartCoroutine(SpawnBumpsRoutine());
        StartCoroutine(SpawnObstacleRoutine());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        XPos = branchObstacle.transform.position.x - BackGroundScroller.scrollSpeed * Time.deltaTime;
        branchObstacle.transform.position = new Vector3(XPos, branchObstacle.transform.position.y);
        
	}

    private void SendBump()
    {
        Slot slot;
        int tries = 0;
        do
        {
            tries++;
            slot = breakableSlots[Random.Range(0, breakableSlots.Count)];
        } while (slot.IsBroken() && tries < 10);
        
        if (slot != null && !slot.IsBroken())
        {
            CameraShake.OnShake(0.3f, 0.3f, 0.01f);
            AudioManager.Instance.PlayAudio(transform.position, "Bump", is3D: false);
            Message.SendMessage(MessageEnum.ON_BREAK_CARRIAGE, slot.gameObject);
        }
    }

    IEnumerator SpawnBumpsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            SendBump();
        }
    }
    IEnumerator SpawnObstacleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(40f,60f));
            SendObstacle();
            SpawnObstacle();
        }
    }

    IEnumerator BlinkAlert()
    {
        AudioManager.Instance.PlayAudio(transform.position, "Danger", is3D: false);
        float timeToShowAlert = 3f;
        float t = 0;
        while(t < timeToShowAlert)
        {
            yield return new WaitForSeconds(0.1f);
            showBranchAlert.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            showBranchAlert.SetActive(false);
            t += 0.4f;
        }


    }
    public void SpawnObstacle()
    {
        branchUI.SetActive(true);
        branchObstacle.transform.position = new Vector3(ScreenSize.halfScreenSize.x + 15f, branchObstacle.transform.position.y);
    }
    public void CheckForObstacle()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            AudioManager.Instance.PlayAudio(transform.position, "Tak Whack", is3D: false);
            List<Entity> entities = EntityManager.GetEntities(null, typeof(Character));
            Entity e = entities[Random.Range(0, entities.Count - 1)];
            e.GetCachedComponent<Character>().Sacrifice();
        }
        else
        {
            AudioManager.Instance.PlayAudio(transform.position, "Swoosh", is3D: false);
        }
        branchUI.SetActive(false);

    }

    public void SendObstacle()
    {
        StartCoroutine(BlinkAlert());
    }

    void OnGameOver()
    {
        StopAllCoroutines();
    }
}
