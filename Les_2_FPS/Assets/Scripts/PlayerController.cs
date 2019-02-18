using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform gunPosition;

    private Gun currentGun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot if you have a gun in hand
        if(currentGun != null) {
            if (Input.GetMouseButtonDown(0)) {
                currentGun.Shoot();
            }
        }
    }

    public void OnTriggerEnter(Collider col) {
        //Pick up the gun
        Gun gun = col.gameObject.GetComponent<Gun>();
        if (gun != null) {
            currentGun = gun;
            currentGun.OnPickup(gunPosition);
        }
    }
}
