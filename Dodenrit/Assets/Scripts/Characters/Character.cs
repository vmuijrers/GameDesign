using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VaalsECS;
public class Character : AbstractComponent {


    public CharacterStats stats;
    public Slot currentSlot;
    public GameObject repair;
    public GameObject whip;
    private GameObject person;

    public Rigidbody2D head;
    private SpriteRenderer art;
    private Text textMesh;
    internal bool canBeSelected = true;
    private Collider2D col;
    private AudioSource aSource;
    private Slot hoveringSlot;
    private Rigidbody2D rb;
    private Slot restingSlot;
    private Rigidbody2D[] ragdoll;
    //private float energy = 100;
    //public float Energy {
    //    get { return energy; }
    //    set
    //    {
    //        if(value ==0 && energy != 0)
    //        {
    //            OnExhausted();
    //        }
    //        if (value == 100 && energy != 100)
    //        {
    //            OnRested();
    //        }
    //        energy = value;
    //    }
    //}

	// Use this for initialization

	public virtual void Start () {

        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        FindSpriteForCharacter();

        if (!stats.isHorse)
        {
            List<Entity> restingSlots = EntityManager.GetEntities(null, typeof(Slot), typeof(Resting));
            restingSlot = restingSlots[0].GetCachedComponent<Slot>();
        }
        aSource = GetComponentInChildren<AudioSource>();
        if (aSource != null)
        {
            aSource.clip = stats.instrumentClip;
            aSource.Play();
        }

        col = GetComponent<Collider2D>();
        art = GetComponentInChildren<SpriteRenderer>();
        textMesh = GetComponentInChildren<Text>();
        textMesh.text = stats.characterName;
        textMesh.gameObject.SetActive(false);

        if(currentSlot != null)
        {
            currentSlot.OnAddCharacter(this);
        }
        else
        {
            GoToRestingSlot();
        }
        StopRepair();
    }

    public void FindSpriteForCharacter()
    {
        if (stats.isHorse) return;
        person = new GameObject("Person");
        string str = stats.isMale ? "Man" : "Vrouw";
        GameObject personArt = Instantiate(Resources.Load<GameObject>(str));
        personArt.transform.SetParent(person.transform);
        personArt.transform.localPosition = new Vector3(0, 1, 0);
        repair.transform.SetParent(person.transform);
        repair.transform.localPosition = new Vector3(0, 1, 0);
        whip.transform.SetParent(person.transform);
        whip.transform.localPosition = new Vector3(0, 1, 0);
        whip.SetActive(false);
        ragdoll = person.GetComponentsInChildren<Rigidbody2D>(true);
        person.transform.SetParent(transform);
        person.transform.localPosition = Vector3.zero;
        person.AddComponent<SpriteAutoDepthLayer>();

    }

    public void DoRepair()
    {
        if (repair != null)
        {
            repair.SetActive(true);
            StartCoroutine(PlayWorkAudio());
        }
    }
    public void StopRepair()
    {
        if(repair != null)
        {
            repair.SetActive(false);
        }
    }

    void SetRagdollActive()
    {
        if(ragdoll != null && ragdoll.Length> 0)
        {
            foreach (Rigidbody2D rb in ragdoll)
            {
                if (rb.tag != "Head")
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }

            }
        }

    }
    void SetRagdollInActive()
    {
        foreach (Rigidbody2D rb in ragdoll)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;
            rb.transform.rotation = Quaternion.identity;

        }
    }
    public void GoToRestingSlot()
    {
        if (stats.isHorse) return;
        if(currentSlot != null)
        {
            currentSlot.OnRemoveCharacter(this);
        }
        currentSlot = restingSlot;
        currentSlot.OnAddCharacter(this);
    }

    public void OnHover()
    {
        textMesh.gameObject.SetActive(true);
    }
    public void BecomeIdle()
    {
        art.color = Color.white;
        StopRepair();
    }

    public void OnStopHover()
    {
        textMesh.gameObject.SetActive(false);
    }
    
    public void OnPickedUp()
    {
        //enable ragdoll
        AudioManager.Instance.PlayAudio(transform.position, "Pickup", pitch: Random.Range(0.9f,1.1f), volume: 0.6f);
        if(currentSlot != null)
        {
            currentSlot.OnRemoveCharacter(this);
            currentSlot = null;
        }
            
        rb.bodyType = RigidbodyType2D.Dynamic;
        SetRagdollActive(); 
        StopRepair();
    }

    public void OnDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,0));
        Collider2D col = Physics2D.OverlapCircle(mousePos, 0.25f, stats.slotLayer);
        if (col != null)
        {

            Slot slot = col.GetComponent<Slot>();
            if (slot != null)
            {
                    
                if(hoveringSlot != null && hoveringSlot != slot)
                {
                    hoveringSlot.UnShowSprite();
                }
                hoveringSlot = slot;
                hoveringSlot.ShowSprite();
            }
        }
        else
        {
            if (hoveringSlot != null)
            {
                hoveringSlot.UnShowSprite(); 
            }
            hoveringSlot = null;
        }
    }

    public void StartWhipping()
    {
        whip.SetActive(true);
        StartCoroutine(PlayWhipAudio());

    }
    public void StopWhipping()
    {
        whip.SetActive(false);
        
    }

    public void OnReleased(Vector3 releaseDir)
    {
        
        if (hoveringSlot != null && !stats.isHorse)
        {

                hoveringSlot.OnAddCharacter(this);
                AudioManager.Instance.PlayAudio(transform.position, "Release", pitch: Random.Range(0.9f, 1.1f), volume: 0.6f);
                currentSlot = hoveringSlot;
                hoveringSlot = null;
                currentSlot.UnShowSprite();

                person.transform.localScale = new Vector3(currentSlot.facingSide == FacingSide.RIGHT? 1: -1, person.transform.localScale.y, person.transform.localScale.z);

                if (currentSlot.IsBroken())
                {
                    DoRepair();
                }
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                SetRagdollInActive();
        }
        else
        {
            rb.AddForce(releaseDir.normalized* 600);
            Sacrifice();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        AudioManager.Instance.PlayAudio(transform.position, "Thud", pitch: Random.Range(0.8f, 1.2f), volume: 0.4f);
    }

    public void OnRested()
    {
        art.color = Color.white;
    }

    public void OnExhausted()
    {
        art.color = Color.red;
        if(currentSlot != null)
        {
            currentSlot.OnRemoveCharacter(this);
        }
        StopRepair();
    }

    public void Sacrifice()
    {
        if (canBeSelected)
        {
            canBeSelected = false;

            StartCoroutine(SacrificeRoutine());
        }
        
    }


    public IEnumerator MoveToPosition(Vector3 position)
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;
            if (t > 1)
            {
                t = 0;
            }
            Debug.Log(stats.name + "IsMoving!");       
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, Vector3.up), 360);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, Vector3.up), t);
            //transform.position = Vector3.MoveTowards(transform.position, position, 10f *Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, position, t);
            yield return null;
            
        }
    }

    IEnumerator SacrificeRoutine()
    {
        if (stats.isHorse)
        {
            AudioManager.Instance.PlayAudio(transform.position, "HorseDead", is3D: true);
        }
        else
        {
            AudioManager.Instance.PlayAudio(transform.position, stats.isMale ? "DeadMale" : "DeadFemale", is3D: true);
        }
        if (currentSlot != null)
        {
            currentSlot.OnRemoveCharacter(this);
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        StopRepair();
        Message<Character>.SendMessage(MessageEnum.ON_SACRIFICE, this);
        transform.parent = null;
        textMesh.gameObject.SetActive(false);
        while (transform.position.x > -ScreenSize.screenSize.x-2f)
        {
            //art.transform.localRotation = Quaternion.RotateTowards(art.transform.localRotation, Quaternion.LookRotation(Vector3.forward, Vector3.right), 5f);
            transform.position += -Vector3.right * BackGroundScroller.scrollSpeed * Time.deltaTime;
            //transform.Rotate(new Vector3(0, 0, 15f));
            yield return null;
        }
        gameObject.SetActive(false);
        Message<Character>.SendMessage(MessageEnum.ON_CHARACTER_DEAD, this);

        
    }

    IEnumerator PlayWorkAudio()
    {

        AudioManager.Instance.PlayAudio(transform.position, "Work",volume:0.6f, pitch: Random.Range(0.9f,1.1f));
        yield return new WaitForSeconds(0.8f);
        if (currentSlot != null && currentSlot.IsBroken())
        {
            StartCoroutine(PlayWorkAudio());
        }
        
    }
    IEnumerator PlayWhipAudio()
    {

        AudioManager.Instance.PlayAudio(transform.position, "Whip", volume: 0.8f, pitch: Random.Range(0.9f, 1.1f));
        yield return new WaitForSeconds(0.8f);
        if (whip.gameObject.activeSelf)
        {
            StartCoroutine(PlayWorkAudio());
        }

    }

}
