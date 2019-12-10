using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoboVac : MonoBehaviour
{
    public float accelerationTime = 2f;
    public float maxSpeed = 5f;
    private Vector2 movement;
    private float timeLeft;
    private Rigidbody rb;
    private void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
     

    }

    void FixedUpdate()
    {
        Vector3 tempVect = new Vector3(-1, 0, 0);
        tempVect = tempVect.normalized * maxSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);
    }

    void OnCollisionEnter(Collision collision)
    {
        Quaternion tempcurrent = this.gameObject.transform.localRotation;
        print("ey!!!");
        ContactPoint contact = collision.contacts[0];

        // Rotate the object so that the y-axis faces along the normal of the surface
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
      
        tempcurrent.y -= 180f;
        this.gameObject.transform.localRotation = tempcurrent;



        Vector3 pos = contact.point;
       
    }
}
