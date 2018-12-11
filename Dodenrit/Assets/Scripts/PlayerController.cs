using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VaalsECS;
public class PlayerController : MonoBehaviour {

    private Character currentCharacter;
    private List<Entity> allCharacters = new List<Entity>();
    private List<Entity> allHorses = new List<Entity>();
    private Character hoverOverCharacter;
    public LayerMask characterLayer;
    internal float xOffSet;
    /// <summary>
    /// Carriage
    /// </summary>
    private float carriageRepair = 0;
    private bool carriageBroken = false;
    private SpriteRenderer carriageSprite;
    private List<Character> RepairingCharacters = new List<Character>();

    public bool gameIsLost = false;
    public float minDistForSwipe = 100;
    private Vector2 startPoint, endPoint;

    public float distanceTravelled = 0;
    public float carriageMoveSpeed = 10f;
    public float moveSpeedMultiplier = 1;
    public float horseSpeedMultiplier = 1f;
    //public TrailRenderer trailRenderer;

    
    private List<Entity> breakableSlots = new List<Entity>();
    public DistanceJoint2D joint;

    void OnEnable()
    {
        Message<Character>.AddListener(MessageEnum.ON_CHARACTER_DEAD, gameObject, CheckLoseGame);
        Message<Character>.AddListener(MessageEnum.ON_SACRIFICE, gameObject, OnSacrifice);
        Message.AddListener(MessageEnum.GAME_OVER, gameObject, OnGameOver);
    }
    void OnDisable()
    {
        Message<Character>.RemoveListener(MessageEnum.ON_CHARACTER_DEAD, gameObject, CheckLoseGame);
        Message<Character>.RemoveListener(MessageEnum.ON_SACRIFICE, gameObject, OnSacrifice);
        Message.RemoveListener(MessageEnum.GAME_OVER, gameObject, OnGameOver);
    }

    // Use this for initialization
    void Start () {
        allCharacters = EntityManager.GetEntities(null, typeof(Character));
        allHorses = EntityManager.GetEntities(null, typeof(Horse));
        breakableSlots = EntityManager.GetEntities(null, typeof(BreakableSlot), typeof(Slot));
    }
	
	// Update is called once per frame
	void Update () {

        if (gameIsLost) return;
        Ray r = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        if (hoverOverCharacter != null)
        {
            hoverOverCharacter.OnStopHover();
            hoverOverCharacter = null;
        }

        Collider2D hit = Physics2D.OverlapPoint(r.origin,characterLayer);
        if (hit != null)
        {
            Character person = hit.gameObject.GetComponent<Character>();
            if (person != null && person.canBeSelected)
            {
                if(hoverOverCharacter != null && hoverOverCharacter != person)
                {
                    hoverOverCharacter.OnStopHover();
                }
                hoverOverCharacter = person;
                hoverOverCharacter.OnHover();
                if (Input.GetMouseButtonDown(0))
                {
                    if(currentCharacter != null && currentCharacter != person)
                    {
                        //Deselect currentCharacter
                        currentCharacter.OnStopHover();
                    }
                    currentCharacter = person;
                }
            }
            
        }

        //Always show selected character
        if (currentCharacter != null)
        {
            currentCharacter.OnHover();
        }

        if(currentCharacter != null && Input.GetMouseButton(0))
        {
            currentCharacter.OnDrag();
        }

        if (Input.GetMouseButtonDown(0) && currentCharacter != null)
        {
            joint.connectedBody = currentCharacter.head;
            currentCharacter.OnPickedUp();
            startPoint = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && currentCharacter != null)
        {
            joint.connectedBody = null;
            endPoint = Input.mousePosition;

            Vector2 mouseDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            currentCharacter.OnReleased(mouseDir);
            currentCharacter = null;
        }



        CheckDamage();


        distanceTravelled += carriageMoveSpeed * moveSpeedMultiplier * horseSpeedMultiplier * Time.deltaTime;
        //Debug.Log("Distance Travelled: " + distanceTravelled);
    }

    void FixedUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        joint.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
    /// <summary>
    /// Checks for damage to the carriage and reduces movespeedMultiplier
    /// </summary>
    void CheckDamage()
    {
        horseSpeedMultiplier = allHorses.Count * 0.5f;
        moveSpeedMultiplier = 1f;
        float step = 1/(carriageMoveSpeed/2f);
        float distXMultiplier = 1f;
        foreach (Entity slot in breakableSlots)
        {
            Slot s = slot.GetCachedComponent<Slot>();
            if (s.IsBroken())
            {
                moveSpeedMultiplier -= step;
                distXMultiplier -= 0.5f;
                moveSpeedMultiplier = Mathf.Clamp01(moveSpeedMultiplier);
            }
        }
        xOffSet = distXMultiplier * horseSpeedMultiplier * 4;
    }

    void CheckLoseGame(Character deadCharacter)
    {
        if(deadCharacter is Horse)
        {
            allHorses.Remove(deadCharacter.Entity);
        }
        else
        {
            allCharacters.Remove(deadCharacter.Entity);
        }

        //Debug.Log(deadCharacter.stats.characterName + " Died!");
        //var horses = allCharacters.Where(x => x.GetType() == typeof(Horse)).ToList();
        //var people = allCharacters.Where(x => x.GetType() != typeof(Horse)).ToList();
        if ((allHorses.Count == 0 || allCharacters.Count == 0) && !gameIsLost)
        {
            //Lose
            Debug.Log("Lost the game!");
            gameIsLost = true;
            Message.SendMessage(MessageEnum.GAME_OVER);
        }
    }

    public void OnSacrifice(Character character)
    {
        if(character == currentCharacter)
        {
            joint.connectedBody = null;
            currentCharacter = null;
        }
    }

    void OnGameOver()
    {
        joint.connectedBody = null;
        currentCharacter = null;
    }
}
