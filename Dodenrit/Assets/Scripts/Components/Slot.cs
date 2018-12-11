using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace VaalsECS 
{ 
    public enum FacingSide { RIGHT, LEFT}
	public class Slot : AbstractComponent
	{
        public Vector3 diffOffSet = new Vector3(-0.25f, 0.4f,0);
        public Vector3[] characterPositionOffSet;

        public FacingSide facingSide = FacingSide.RIGHT;

        public List<Character> characters = new List<Character>();

        public SpriteRenderer spriteRenderer;

        public GameObject brokenSprite;
        public SpriteRenderer highlightSprite;

        public GameObject feedback;
        public Text buttonToPress;
        public RawImage healthBar;

		public int maxCharacters = 2;
        public float hitPoints = 100;
        public float HitPoints
        {
            get { return hitPoints; }
            set
            {
                if(hitPoints < maxHP && value >= maxHP)
                {
                    OnRepaired();
                }
                hitPoints = value;
                hitPoints = Mathf.Clamp(hitPoints, 0, maxHP);
            }
        }
        //public float energyDrain;
        public float repairSpeed = 1;

        private float baseRepairWorkers = 4;
        private float baseRepairPlayer = 4;
        internal float maxHP = 100;
        private KeyCode currentKey;

        private void Start()
        {
            OnRepaired();
            UnShowSprite();
        }



        protected override void OnEnable()
        {
            base.OnEnable();
            Message.AddListener(MessageEnum.ON_BREAK_CARRIAGE,gameObject, OnBreak);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Message.RemoveListener(MessageEnum.ON_BREAK_CARRIAGE,gameObject, OnBreak);
        }


        private void FixedUpdate()
        {
            if (HitPoints < maxHP)
            {
                float baseWork = (characters.Count * baseRepairWorkers) * Time.deltaTime;
                float playerWork = 0;
                if (Input.GetKeyDown(currentKey) && characters.Count >0)
                {
                    AudioManager.Instance.PlayAudio(transform.position, "Work", volume: 0.6f, pitch: Random.Range(0.9f, 1.1f));
                    playerWork = baseRepairPlayer + repairSpeed * characters.Count;
                }
                HitPoints += baseWork + playerWork;

                healthBar.transform.localScale = new Vector3(HitPoints / maxHP, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

                //for (int i= characters.Count-1; i >=0 ; i--)
                //{
                //    Character c = characters[i];
                //    if(c.Energy > 0)
                //    {
                //        c.Energy += energyDrain * Time.deltaTime;
                //        c.Energy = Mathf.Clamp(c.Energy, 0, 100);


                //        HitPoints += repairSpeed * Time.deltaTime;
                //        HitPoints = Mathf.Clamp(HitPoints, 0, maxHP);
                //    }
                //}

                //spriteRenderer.color = Color.Lerp(Color.red, Color.white, hitPoints / maxHP);
            }
        }


        public bool IsBroken()
        {
            return HitPoints < maxHP;
        }

        public void OnBreak()
        {
            currentKey = (KeyCode)Random.Range(97, 97 + 26);
            string str = currentKey.ToString();
            buttonToPress.text = char.ToUpper(str[0]).ToString();
            
            highlightSprite.enabled = true;

            if(characters.Count > 0)
            {
                highlightSprite.color = Color.green;
            }
            else
            {
                highlightSprite.color = Color.red;
            }


            hitPoints = 0;
            feedback.SetActive(true);
            brokenSprite.SetActive(true);
            foreach (Character c in characters)
            {
                c.DoRepair();
            }
        }

        public void OnRepaired()
        {
            for (int i = characters.Count-1; i >=0 ; i--)
            {
                Character c = characters[i];
                c.StopRepair();
                c.GoToRestingSlot();
            } 

            if (brokenSprite == null || highlightSprite == null) return;

            feedback.SetActive(false);
            brokenSprite.SetActive(false);
            highlightSprite.enabled = false;

        }
        private void OnDrawGizmos()
        {
            for (int i = 0; i < characterPositionOffSet.Length; i++)
            {
                if (i == 0)
                {
                    Gizmos.DrawLine(transform.position, transform.position + characterPositionOffSet[0]);
                    Gizmos.DrawWireSphere(transform.position + characterPositionOffSet[0], 0.1f);
                }
                else
                {
                    Gizmos.DrawWireSphere(transform.position + characterPositionOffSet[i], 0.1f);
                    Gizmos.DrawLine(transform.position + characterPositionOffSet[i-1], transform.position + characterPositionOffSet[i]);
                }

                
            }
            
            
        }

        public bool OnAddCharacter(Character character) 
        {
            if (!characters.Contains(character))
            {
                characters.Add(character);
                OnUpdateCharacterPositions();
            }
            return true;
        }

        public void OnRemoveCharacter(Character character)
        {
            if (characters.Contains(character))
            {
                characters.Remove(character);
                OnUpdateCharacterPositions();
            }
        }

        //For Hovering
        public void ShowSprite()
        {
            if (highlightSprite == null) return;
            highlightSprite.enabled = true;
            highlightSprite.color = Color.green;
        }
        public void UnShowSprite()
        {
            if (highlightSprite == null) return;
            if (brokenSprite == null) return;
            if (hitPoints < maxHP)
            {
                brokenSprite.SetActive(true);
                highlightSprite.enabled = true;
                if (characters.Count > 0)
                {
                    highlightSprite.color = Color.green;
                }
                else
                {
                    highlightSprite.color = Color.red;
                }
            }
            else
            {
                brokenSprite.SetActive(false);
                highlightSprite.enabled = false;
            }
        }

        public void OnUpdateCharacterPositions()
        {
            StopAllCoroutines();
            if (characterPositionOffSet.Length > 1)
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    characters[i].transform.position = transform.position + characterPositionOffSet[i];
                    characters[i].transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                    //StartCoroutine(characters[i].MoveToPosition(transform.position + characterPositionOffSet[i]));
                }
            }
            else
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    characters[i].transform.position = transform.position + characterPositionOffSet[0] + diffOffSet * i;
                    characters[i].transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                    //StartCoroutine(characters[i].MoveToPosition(transform.position + characterPositionOffSet[0] + diffOffSet * i));
                }
            }

        }
    }

}
