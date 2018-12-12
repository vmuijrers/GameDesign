using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbSpawner : MonoBehaviour {

    public Transform ObjecToFollow;
    public float maxTimer;

    public float spawnDistanceFromPlayer;
    public LayerMask layerMask;
    public AudioClip[] spawnSounds;
    public AudioClip[] pickupSounds;
    public Text text;
    int score = 0;
    private float currentTimer = 0;

    private Material mat;
	// Use this for initialization
	void Start () {
        mat = GetComponentInChildren<MeshRenderer>().material;
        Respawn();
    }
	
	// Update is called once per frame
	void Update () {
        currentTimer -= Time.deltaTime;
        mat.color = Color.Lerp(Color.red, Color.green, currentTimer / maxTimer);
        if (currentTimer <= 0)
        {
            Respawn();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickupSounds[Random.Range(0,pickupSounds.Length)], transform.position);
            Debug.Log("Pickedup!");
            score++;
            UpdateScore();
            Respawn();
        }
        
    }
    void UpdateScore()
    {
        text.text = "Score: " + score.ToString();
    }

    void Respawn()
    {
        currentTimer = maxTimer;
        Vector3 rayCastPoint=ObjecToFollow.transform.position + (new Vector3(Random.Range(-1,1),0,Random.Range(-1,1))).normalized * spawnDistanceFromPlayer + new Vector3(0,40,0);
        RaycastHit hit;
        if (Physics.Raycast(rayCastPoint, -Vector3.up,out hit,140, layerMask))
        {
            AudioSource.PlayClipAtPoint(spawnSounds[Random.Range(0, pickupSounds.Length)], transform.position);
            transform.position = hit.point;
        }
        else
        {
            Respawn();
        }
    }
}
