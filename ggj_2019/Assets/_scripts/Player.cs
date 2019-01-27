using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cam,camfoward,angleDetection,slideIndicator,spriteObject;
    public Transform currentCheckpoint,checkPoints;
    public int currentCheckPoint;
    public float pullSpeed;
    private Rigidbody rb;
    private Animator anim;
    public float maxSpeed,currentFriction,currentVelMag, gravity,frictionApplySpeed,groundCheckDistance, sideslidespeed,currentSlideSpeed,driftTimer,sideRun,run;
    public Vector3 currentVelocity,currentSlideVelocity;
    public bool canJump,sliding,controllerOn;

    public Quaternion targetRotation;
    public float step;

    // Start is called before the first frame update
    void Start()
    {
        anim = spriteObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        transform.position = new Vector3(Random.Range(-20,21),0.765f, Random.Range(-20, 21));
    }

    // Update is called once per frame
    void Update()
    {
        spriteObject.transform.position = transform.position;
        spriteObject.transform.rotation = cam.transform.rotation;
        if (controllerOn == true) { ControllerControls(); }
        else
        {
            KeyboardControls();
            

        }

       

        
        

    }
    public void ControllerControls()
    {
        
        if (Input.GetAxis("3rd Axis") == 0)
        {
            anim.SetFloat("slideSpeed", 0);
            if (driftTimer >= 0.5f) {
                // rb.velocity = transform.forward.normalized * rb.velocity.magnitude;
                run += (run * 0.1f) * driftTimer;
               currentVelocity = transform.forward.normalized * currentVelocity.magnitude  * 1.2f;
                rb.AddForce(Time.deltaTime * transform.forward.normalized * driftTimer *  60.0f, ForceMode.Impulse);

            }
            driftTimer = 0;

            

            //transform.LookAt(new Vector3(camfoward.transform.position.x, transform.position.y, camfoward.transform.position.z));
            slideIndicator.active = false;
            if (Input.GetAxis("Horizontal") != 0)
            {
                targetRotation = Quaternion.LookRotation(new Vector3(camfoward.transform.right.normalized.x * 15 * Mathf.Sign(Input.GetAxis("Horizontal")) + camfoward.transform.position.x, transform.position.y, camfoward.transform.right.normalized.z * 15 * Mathf.Sign(Input.GetAxis("Horizontal")) + camfoward.transform.position.z) - transform.position);
                step = Mathf.Min(2 * Time.deltaTime, 1.5f);
                
            }
            
            else {
                targetRotation = Quaternion.LookRotation(new Vector3(camfoward.transform.position.x , transform.position.y, camfoward.transform.position.z) - transform.position);
                step = Mathf.Min(2 * Time.deltaTime, 1.5f);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step);
            if ( Input.GetKey(KeyCode.Joystick1Button1))
            {

                
                run = Mathf.Lerp(run, maxSpeed, 2.0f * Time.deltaTime);
            }
            else { run = Mathf.Lerp(run, 1.0f, Time.deltaTime * 0.2f); }
            anim.SetFloat("speed", (1.0f / maxSpeed) * run );
            currentVelocity = Vector3.Lerp(currentVelocity,(run * transform.forward.normalized), Time.deltaTime);
            rb.velocity = Vector3.Lerp(rb.velocity, currentVelocity, 2.0f * Time.deltaTime);
        }
        else
        {
            anim.SetFloat("slideSpeed", Input.GetAxis("3rd Axis"));
            driftTimer = Mathf.Lerp(driftTimer,3.0f,Time.deltaTime);
            transform.Rotate(0, Mathf.Sign(Input.GetAxis("3rd Axis")) * 75 * Time.deltaTime, 0);
            rb.AddForce(Time.deltaTime * transform.right.normalized * -Mathf.Sign(Input.GetAxis("3rd Axis")) * 0.2f, ForceMode.Impulse);
            slideIndicator.transform.parent.transform.localScale = new Vector3(Mathf.Sign(Input.GetAxis("3rd Axis")),1,1);
            if (slideIndicator.active == false)
            { slideIndicator.active = true; slideIndicator.GetComponent<Animator>().Play("IntialFire"); }
           
        }


        //if (Input.GetAxis("3rd Axis") != 0)
        //{
        //    sliding = true;
        //    transform.Rotate(0, Input.GetAxis("3rd Axis") * 15 * Time.deltaTime, 0);
        //    rb.AddForce(Time.deltaTime * transform.right.normalized * Input.GetAxis("3rd Axis"), ForceMode.Impulse);
        //}
        //else { sliding = false; }

    }
    public void KeyboardControls()
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
            else { sideRun = 0; }
            if (Input.GetKey(KeyCode.W))
            {
                run = Mathf.Lerp(run, 10.0f, 0.5f * Time.deltaTime);
            }
            else { run = Mathf.Lerp(run, 1.0f, Time.deltaTime * 0.2f); }

            currentVelocity = (transform.right.normalized * sideRun) + (run * transform.forward.normalized);
            rb.velocity = Vector3.Lerp(rb.velocity, currentVelocity, Time.deltaTime);
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                sideRun = -sideslidespeed;
                rb.AddForce(Time.deltaTime * transform.right.normalized * -sideRun, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.D))
            {
                sideRun = sideslidespeed;
                rb.AddForce(Time.deltaTime * transform.right.normalized * -sideRun, ForceMode.Impulse);
            }
            transform.Rotate(0, sideRun * 15 * Time.deltaTime, 0);


            //rb.AddForce(Time.deltaTime * currentVelocity * transform.right.normalized, ForceMode.Impulse);
            slideIndicator.active = true;
        }
        if (Input.GetMouseButton(0))
        { sliding = true; }

        if (Input.GetMouseButtonUp(0) && sliding == true)
        {
            sliding = false;
            currentVelocity = currentVelocity.magnitude * 4 * transform.forward.normalized;
            rb.AddForce(Time.deltaTime * currentVelocity, ForceMode.Impulse);
        }
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
