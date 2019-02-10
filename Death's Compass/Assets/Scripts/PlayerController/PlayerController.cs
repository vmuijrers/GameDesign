﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    private GameObject camera;

    [Header("CollisionSettings")]
    public LayerMask worldMask;

    private float rayCastToGround = 0.05f;

    [Header("Movement Settings")]
    public AnimationCurve speedCurve;
    public float timeToTopSpeed;
    public Vector3 jumpVelocity = new Vector3(0, 1, 0);

    private float height = 1.4f;
    private float radius = 0.25f;
    private float offSet = 0.1f;

    [Tooltip("This is a percentage that the velocity loses per frame")]
    public float noMoveFriction;
    public float maxSpeed;
    public int numDoubleJumps;
    public float gravityAmount;

    [Header("Camera Settings")]
    public float sensitivityX = 15;
    public float sensitivityY = 15;
    public float maxYAngle = 85f;

    private Rigidbody rb;
    private float camAngleX;
    private float camAngleY;
    private Vector3 velocity;
    private float speedModifier;

    public CharacterController characterController;

    private bool grounded = false;
    public bool Grounded {
        get { return grounded; }
        set {
            if(value && !grounded)
            {
                OnBecomeGrounded();
            }
            grounded = value;
        }

    }
    private int doubleJump = 0;
    private float gravity;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>().gameObject;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {

        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        Grounded = characterController.isGrounded;
        float rayDistance = radius + 0.1f;
            
        bool collisionWithWall = Physics.CheckCapsule(velocity.normalized * offSet + transform.position + transform.up * (radius), velocity.normalized * offSet + transform.position + transform.up * (height - radius), radius, worldMask);

        if (collisionWithWall)
        {
            Debug.Log("Colliding with wall!");
            velocity = new Vector3(0, velocity.y, 0);
            speedModifier = Mathf.Min(speedModifier, 0.1f);
        }
        //Apply forward velocity
        if(vert != 0 || hor != 0)
        {
            speedModifier += (1f / Mathf.Max(0.001f,timeToTopSpeed)) * Time.deltaTime;
            speedModifier = Mathf.Clamp01(speedModifier);
            float speed = maxSpeed * speedCurve.Evaluate(speedModifier);
            Vector3 velXZ = (vert * transform.forward + hor * transform.right).normalized * speed;
            velocity = new Vector3(velXZ.x, velocity.y, velXZ.z);
        }
        else
        {
            velocity -= Vector3.Scale(velocity, new Vector3(1,0,1) * (1-noMoveFriction));
            speedModifier = 0;
        }

        //Apply Gravity
        if (!grounded)
        {
            gravity = gravityAmount * Time.deltaTime;
            velocity -= new Vector3(0, 1, 0) * gravity;
        }
        else
        {
             gravity = 0;
        }
        //Apply Jump Force
        if (Input.GetKeyDown(KeyCode.Space) && doubleJump < numDoubleJumps)
        {
            velocity = new Vector3(velocity.x, jumpVelocity.y , velocity.z) + transform.forward * +jumpVelocity.z * Mathf.Max(0.5f, (velocity.magnitude / maxSpeed));
            doubleJump++;
            gravity = 0;
        }

        //Apply rotation and update camera
        float camX = Input.GetAxis("Mouse X");
        float camY = Input.GetAxis("Mouse Y");

        camAngleX += camX * sensitivityX;
        camAngleY += camY * sensitivityY;
        camAngleY = Mathf.Clamp(camAngleY, -maxYAngle, maxYAngle);
        camera.transform.localRotation = Quaternion.Euler(-camAngleY, 0, 0);

        transform.rotation = Quaternion.Euler(0, camAngleX, 0);
    }

    private void OnBecomeGrounded()
    {
        if (velocity.y <= 0)
        {
            doubleJump = 0;
        }
        velocity = Vector3.Scale(velocity , new Vector3(1,0,1));
    }

    private void FixedUpdate()
    {
        characterController.Move(velocity *Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(velocity.normalized * offSet + transform.position + transform.up * (radius), radius);
        Gizmos.DrawWireSphere(velocity.normalized * offSet + transform.position + transform.up * (height - radius), radius);
    }
}
