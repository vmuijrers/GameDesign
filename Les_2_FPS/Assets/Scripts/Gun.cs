using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingTransform;
    public float shootingForce = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Shoot() {
        //Spawn a bullet and add a force
        GameObject bullet = Instantiate(bulletPrefab, shootingTransform.position, shootingTransform.rotation);
        Rigidbody bulletRigidBody = bullet.GetComponent<Rigidbody>();
        bulletRigidBody.AddForce(shootingForce * bullet.transform.forward);
    }

    public void OnPickup(Transform parentTransform) {
        //Parent the gun to the camera and set in the right position
        transform.SetParent(parentTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
