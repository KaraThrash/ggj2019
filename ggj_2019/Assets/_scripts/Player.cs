using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cam;
    public float pullSpeed;
    private Rigidbody rb;
    public float maxSpeed,currentFriction,currentVelMag;
    public Vector3 currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentVelMag = currentVelocity.magnitude;
        if (currentVelocity.magnitude > 3)
        { currentVelocity *= currentFriction; }
        rb.velocity = currentVelocity;
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "speed")
        {
            if (Input.GetMouseButtonDown(0))
            {
               // rb.velocity = (cam.transform.forward * pullSpeed * Time.deltaTime); //Vector3.zero;
                currentVelocity = (new Vector3(cam.transform.forward.x,transform.position.y,cam.transform.forward.z) * pullSpeed * Time.deltaTime);
                currentVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
               // rb.AddForce(cam.transform.forward * pullSpeed * Time.deltaTime,ForceMode.Impulse);
            }
        }

    }
    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "lava")
        {
            currentFriction = 0.3f;
        }
        else if (other.transform.tag == "ice")
        {
            currentFriction = 1;
        }
        else { currentFriction = 0.9f; }
    }
}
