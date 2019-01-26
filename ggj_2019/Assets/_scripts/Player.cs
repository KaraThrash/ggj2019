using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cam,camfoward,angleDetection,slideIndicator;
    public Transform currentCheckpoint,checkPoints;
    public int currentCheckPoint;
    public float pullSpeed;
    private Rigidbody rb;
    public float maxSpeed,currentFriction,currentVelMag, gravity,frictionApplySpeed,groundCheckDistance, sideslidespeed,currentSlideSpeed,driftTimer,sideRun,run;
    public Vector3 currentVelocity,currentSlideVelocity;
    public bool canJump,sliding;

    public Quaternion targetRotation;
    public float step;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (sliding == false)
        {
           
            targetRotation = Quaternion.LookRotation(new Vector3(camfoward.transform.position.x, transform.position.y, camfoward.transform.position.z) - transform.position);
            step = Mathf.Min(2 * Time.deltaTime, 1.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step);

            //transform.LookAt(new Vector3(camfoward.transform.position.x, transform.position.y, camfoward.transform.position.z));
            slideIndicator.active = false;
            if (Input.GetKey(KeyCode.A))
            {
                sideRun = -sideslidespeed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                sideRun = sideslidespeed;
            }
            else { sideRun = 0;  }
            if (Input.GetKey(KeyCode.W))
            {
                run = Mathf.Lerp(run, 10.0f , 0.5f * Time.deltaTime);
            }
            else { run = Mathf.Lerp(run, 1.0f,  Time.deltaTime * 0.2f); }

            currentVelocity = (transform.right.normalized * sideRun) + (run * transform.forward.normalized);
            rb.velocity = Vector3.Lerp(rb.velocity, currentVelocity, Time.deltaTime);
        }
        else {
            if (Input.GetKey(KeyCode.A))
            {
                sideRun = -sideslidespeed;
                rb.AddForce(Time.deltaTime * transform.right.normalized  * -sideRun, ForceMode.Impulse);
            }
             if (Input.GetKey(KeyCode.D))
            {
                sideRun = sideslidespeed;
                rb.AddForce(Time.deltaTime * transform.right.normalized  * -sideRun, ForceMode.Impulse);
            }
            transform.Rotate(0, sideRun * 15 * Time.deltaTime,0);


            //rb.AddForce(Time.deltaTime * currentVelocity * transform.right.normalized, ForceMode.Impulse);
            slideIndicator.active = true;
        }
       
        if (Input.GetMouseButton(0))
        { sliding = true; }
    
        if (Input.GetMouseButtonUp(0) && sliding == true)
        { sliding = false;
            currentVelocity = currentVelocity.magnitude * 4 * transform.forward.normalized;
            rb.AddForce(  Time.deltaTime * currentVelocity, ForceMode.Impulse);
        }
        //currentVelMag = currentVelocity.magnitude;
        //float newy = currentVelocity.y;
        //float newx =  Mathf.Lerp(currentVelocity.x,0, currentFriction * Time.deltaTime);
        //float newz = Mathf.Lerp(currentVelocity.z,0, currentFriction * Time.deltaTime);
        //currentVelocity = new Vector3(newx, newy, newz);

        //if (CheckGround() == true)
        //{
        //    if (Input.GetKey(KeyCode.A) )
        //    {
        //        driftTimer += Time.deltaTime;
        //        // rb.AddForce(cam.transform.right * -sideslidespeed * Time.deltaTime,ForceMode.Impulse);
        //        // currentVelocity -= (new Vector3(cam.transform.right.normalized.x, 0, cam.transform.right.normalized.z) * 3 * Time.deltaTime);
        //        currentSlideSpeed = Mathf.Lerp(currentSlideSpeed, -sideslidespeed, 5 * Time.deltaTime);
        //    }
        //     else if (Input.GetKey(KeyCode.D) )
        //    {
        //        driftTimer += Time.deltaTime;
        //        currentSlideSpeed = Mathf.Lerp(currentSlideSpeed, sideslidespeed, 5 * Time.deltaTime);
        //        // rb.AddForce(cam.transform.right * sideslidespeed * Time.deltaTime, ForceMode.Impulse);
        //        // currentVelocity += (new Vector3(cam.transform.right.normalized.x, 0, cam.transform.right.normalized.z) * 3 * Time.deltaTime);
        //    }
        //    else {  currentSlideSpeed = Mathf.Lerp(currentSlideSpeed,0, Time.deltaTime); }


        //     if (Input.GetKeyUp(KeyCode.D) && driftTimer >= 2)
        //    {
        //     // transform.Rotate(0,90,0);

        //        currentVelocity = transform.forward * currentVelMag;
        //    }
        //    if (Input.GetKeyUp(KeyCode.A) && driftTimer >= 2)
        //    {
        //       // transform.Rotate(0, -90, 0);
        //        currentVelocity = transform.forward * currentVelMag;
        //    }
        //    currentSlideVelocity = Vector3.Lerp(currentSlideVelocity, angleDetection.transform.GetChild(0).transform.forward  , 10.0f * Time.deltaTime);
        //    //newcurrentVelocity = new Vector3(cam.transform.right.normalized.x * currentSlideSpeed, 0, cam.transform.right.normalized.z * currentSlideSpeed); ;
        //    newy = Mathf.Clamp(newy - (gravity * Time.deltaTime), -1.0f, 32.0f);
        //  //  currentVelocity = new Vector3(currentVelocity.x * currentFriction, Mathf.Clamp(currentVelocity.y - (gravity * Time.deltaTime), -1.0f, 32.0f), currentVelocity.z * currentFriction);
        //}
        //else
        //{
        //    newy =  Mathf.Clamp(newy - (gravity * Time.deltaTime), -20.0f, 32.0f);
        //  //  currentVelocity = new Vector3(currentVelocity.x * currentFriction, Mathf.Clamp(currentVelocity.y - (gravity * Time.deltaTime), -15.0f, 32.0f), currentVelocity.z * currentFriction);
        //}


        //currentSlideVelocity = Vector3.Lerp(currentSlideVelocity, angleDetection.transform.GetChild(0).transform.forward * currentSlideSpeed, 10.0f * Time.deltaTime);
        //angleDetection.transform.LookAt(transform.position + new Vector3(currentVelocity.normalized.x, 0, currentVelocity.z));
        //rb.velocity = currentVelocity + currentSlideVelocity;



        //// if (rb.velocity.magnitude < maxSpeed) { rb.AddForce(currentVelocity * 5.0f * Time.deltaTime, ForceMode.Impulse); }

        //if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        //{
        //    canJump = false; currentFriction = 1;
        //    //transform.position = new Vector3(transform.position.x,transform.position.y + 2.0f,transform.position.z);
        //    currentVelocity = new Vector3(currentVelocity.x, ( (Mathf.Abs(rb.velocity.x) + rb.velocity.z) * 2 ), currentVelocity.z);
        //   //todo?: foward force when jumping? currentVelocity += (transform.forward * 2 * Time.deltaTime);
        //}

    }
    public bool CheckGround()
    {
   
        RaycastHit hit;



        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
        {

            if (hit.transform.tag == "lava")
            {
                currentFriction = 0.9f;
            }
            else if (hit.transform.tag == "basic")
            {
                currentFriction = 0.1f;
            }
            else { currentFriction = 0; }

            return true;
        }
        else { return false; }
       
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform == currentCheckpoint)
        {
            currentCheckPoint++;
            if (currentCheckPoint >= checkPoints.transform.childCount)
            { currentCheckPoint = 0; }
             currentCheckpoint = checkPoints.transform.GetChild(currentCheckPoint);
            cam.GetComponent<ThirdpersonCamera>().checkpoint = currentCheckpoint.gameObject;
        }

    }
    public void OnCollisionEnter(Collision other)
    {
        canJump = true;
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
