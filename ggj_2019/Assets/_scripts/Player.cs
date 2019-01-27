using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cam,camfoward,angleDetection,slideIndicator,spriteObject,jumpObj,slideObj,shadowObj,proximityObj,lastscoredobj;
    public ScoreKeeper scoreKeeper;
    public Transform currentCheckpoint,checkPoints;
    public int currentCheckPoint;
    public float pullSpeed;
    private Rigidbody rb;
    private Animator anim;
    public float maxSpeed,currentFriction,currentVelMag, gravity,frictionApplySpeed,groundCheckDistance, sideslidespeed,currentSlideSpeed,driftTimer,jumpTimer,powerSlideTimer,slideTimer,sideRun,run;
    public Vector3 currentVelocity,currentSlideVelocity,currentJumpVelocity, currentPowerSlideVelocity;
    public bool canJump,sliding,controllerOn,bumped;

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
        if (transform.position.y < 0.9f)
        { transform.position = new Vector3(transform.position.x,0.9f,transform.position.z); }

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
      
         if (powerSlideTimer > 0)
        {
            CheckGround(-1);
            slideObj.active = true;
            jumpObj.active = false;
          // powerSlideTimer -= Time.deltaTime;
            shadowObj.active = false;

            run = Mathf.Lerp(run, 1.0f, Time.deltaTime * 0.2f);
            rb.velocity = currentPowerSlideVelocity;
            currentPowerSlideVelocity = Vector3.Lerp(currentPowerSlideVelocity, Vector3.zero, 0.5f * Time.deltaTime);
            if (currentPowerSlideVelocity.magnitude < 1) { slideIndicator.active = false; }
            
            anim.SetBool("powerSliding", true);
            anim.Play("powerSlide");
            if (Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                powerSlideTimer = 0; anim.SetBool("powerSliding", false);
            }
            // rb.velocity = Vector3.Lerp(rb.velocity, currentJumpVelocity, 2.0f * Time.deltaTime); 
        }
       else if (jumpTimer > 0)
        {
            CheckGround(1);

            jumpObj.active = true;
            slideObj.active = false;
            shadowObj.active = true;
            jumpTimer -= Time.deltaTime;
            rb.velocity = currentJumpVelocity;
            // rb.velocity = Vector3.Lerp(rb.velocity, currentJumpVelocity, 2.0f * Time.deltaTime); 
        }
        else {

            jumpObj.active = true;
            slideObj.active = true;
            shadowObj.active = false;
            anim.SetBool("jumping", false);
            anim.SetBool("powerSliding", false);




            if (Input.GetAxis("3rd Axis") != 0  && bumped == false)
            {
                

                anim.SetFloat("slideSpeed", Input.GetAxis("3rd Axis"));
                driftTimer = Mathf.Lerp(driftTimer, 2.0f, Time.deltaTime);
                transform.Rotate(0, Mathf.Sign(Input.GetAxis("3rd Axis")) * 75 * Time.deltaTime, 0);
                rb.AddForce(Time.deltaTime * transform.right.normalized * -Mathf.Sign(Input.GetAxis("3rd Axis")) * 0.2f, ForceMode.Impulse);
                slideIndicator.transform.parent.transform.localScale = new Vector3(Mathf.Sign(Input.GetAxis("3rd Axis")), 1, 1);
                if (slideIndicator.active == false)
                { slideIndicator.active = true; slideIndicator.GetComponent<Animator>().Play("IntialFire"); }

            }
           else
            {
                if (Input.GetAxis("3rd Axis") == 0)
                { bumped = false; }
                
                anim.SetFloat("slideSpeed", 0);
                if (driftTimer >= 0.5f) {
                    // rb.velocity = transform.forward.normalized * rb.velocity.magnitude;
                    run += (run * 0.1f) * driftTimer;
                    currentVelocity = transform.forward.normalized * currentVelocity.magnitude * 1.2f;
                    rb.AddForce(Time.deltaTime * transform.forward.normalized * driftTimer * 60.0f, ForceMode.Impulse);

                }
                driftTimer = 0;



                //transform.LookAt(new Vector3(camfoward.transform.position.x, transform.position.y, camfoward.transform.position.z));
                slideIndicator.active = false;
                
                if (Input.GetKey(KeyCode.Joystick1Button1))
                {
                    if (run < 0.5f) { run = 0.5f; }

                    run = Mathf.Lerp(run, maxSpeed, 2.0f * Time.deltaTime);
                }
                else { run = Mathf.Lerp(run, 1.0f, Time.deltaTime * 0.2f); }
                if (Input.GetKey(KeyCode.Joystick1Button2)) { run = Mathf.Lerp(run, 1.0f, Time.deltaTime * 0.2f); }

                    anim.SetFloat("speed", (1.0f / maxSpeed) * run);
                currentVelocity = Vector3.Lerp(currentVelocity, (run * transform.forward.normalized), Time.deltaTime);
                rb.velocity = Vector3.Lerp(rb.velocity, currentVelocity, 2.0f * Time.deltaTime);
            }
          


            if (Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                jumpTimer = 0.8f;
                if (driftTimer <= 0) { currentJumpVelocity = rb.velocity * run * 0.05f; } else { currentJumpVelocity = rb.velocity * driftTimer; }

                //Debug.Log("1 jump mag:" + currentJumpVelocity.magnitude);
                if (currentJumpVelocity.magnitude < 5) { currentJumpVelocity *= 1.5f; }
                if (currentJumpVelocity.magnitude < 15) { currentJumpVelocity *= 1.5f; }
                if (currentJumpVelocity.magnitude > 35) { currentJumpVelocity *= 0.3f; }
               // Debug.Log("2 jump mag:" + currentJumpVelocity.magnitude);
                anim.SetBool("jumping", true);
                anim.Play("jump");
                slideIndicator.active = false;
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                powerSlideTimer = 0.8f;
                if (driftTimer <= 1) { currentPowerSlideVelocity = rb.velocity * (1 + run) * 0.05f; } else { currentPowerSlideVelocity = rb.velocity * driftTimer; }

                Debug.Log("1 jump mag:" + currentPowerSlideVelocity);
                if (currentPowerSlideVelocity.magnitude < 15) { currentPowerSlideVelocity *= 1.5f; }
                if (currentPowerSlideVelocity.magnitude > 35) { currentPowerSlideVelocity *= 0.3f; }
                 Debug.Log("2 jump mag:" + currentPowerSlideVelocity);
                anim.SetBool("powerSliding", true);
                anim.Play("powerSlide");
                //slideIndicator.active = false;
            }
            if (Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                powerSlideTimer = 0; anim.SetBool("powerSliding", false);
            }
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            targetRotation = Quaternion.LookRotation(new Vector3(camfoward.transform.right.normalized.x * 15 * Mathf.Sign(Input.GetAxis("Horizontal")) + camfoward.transform.position.x, transform.position.y, camfoward.transform.right.normalized.z * 15 * Mathf.Sign(Input.GetAxis("Horizontal")) + camfoward.transform.position.z) - transform.position);
            step = Mathf.Min(2 * Time.deltaTime, 1.5f);

        }

        else
        {
            targetRotation = Quaternion.LookRotation(new Vector3(camfoward.transform.position.x, transform.position.y, camfoward.transform.position.z) - transform.position);
            step = Mathf.Min(2 * Time.deltaTime, 1.5f);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step);

    }
    public void KeyboardControls()
    {
        if (Input.GetAxis("MouseSlide") == 0)
        {
            anim.SetFloat("slideSpeed", 0);
            if (driftTimer >= 0.5f)
            {
                // rb.velocity = transform.forward.normalized * rb.velocity.magnitude;
                run += (run * 0.1f) * driftTimer;
                currentVelocity = transform.forward.normalized * currentVelocity.magnitude * 1.2f;
                rb.AddForce(Time.deltaTime * transform.forward.normalized * driftTimer * 60.0f, ForceMode.Impulse);

            }
            driftTimer = 0;



            //transform.LookAt(new Vector3(camfoward.transform.position.x, transform.position.y, camfoward.transform.position.z));
            slideIndicator.active = false;
            if (Input.GetAxis("Horizontal") != 0)
            {
                targetRotation = Quaternion.LookRotation(new Vector3(camfoward.transform.right.normalized.x * 15 * Mathf.Sign(Input.GetAxis("Horizontal")) + camfoward.transform.position.x, transform.position.y, camfoward.transform.right.normalized.z * 15 * Mathf.Sign(Input.GetAxis("Horizontal")) + camfoward.transform.position.z) - transform.position);
                step = Mathf.Min(2 * Time.deltaTime, 1.5f);

            }

            else
            {
                targetRotation = Quaternion.LookRotation(new Vector3(camfoward.transform.position.x, transform.position.y, camfoward.transform.position.z) - transform.position);
                step = Mathf.Min(2 * Time.deltaTime, 1.5f);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step);
            if (Input.GetKey(KeyCode.W))
            {


                run = Mathf.Lerp(run, maxSpeed, 2.0f * Time.deltaTime);
            }
            else { run = Mathf.Lerp(run, 1.0f, Time.deltaTime * 0.2f); }
            anim.SetFloat("speed", (1.0f / maxSpeed) * run);
            currentVelocity = Vector3.Lerp(currentVelocity, (run * transform.forward.normalized), Time.deltaTime);
            rb.velocity = Vector3.Lerp(rb.velocity, currentVelocity, 2.0f * Time.deltaTime);
        }
        else
        {
            anim.SetFloat("slideSpeed", Input.GetAxis("MouseSlide"));
            driftTimer = Mathf.Lerp(driftTimer, 3.0f, Time.deltaTime);
            transform.Rotate(0, Mathf.Sign(Input.GetAxis("MouseSlide")) * 75 * Time.deltaTime, 0);
            rb.AddForce(Time.deltaTime * transform.right.normalized * -Mathf.Sign(Input.GetAxis("MouseSlide")) * 0.2f, ForceMode.Impulse);
            slideIndicator.transform.parent.transform.localScale = new Vector3(Mathf.Sign(Input.GetAxis("MouseSlide")), 1, 1);
            if (slideIndicator.active == false)
            { slideIndicator.active = true; slideIndicator.GetComponent<Animator>().Play("IntialFire"); }

        }

    }

    public void CheckGround(int upordown)
    {
   
        RaycastHit hit;



        if (Physics.Raycast(transform.position, -transform.up, out hit, upordown * groundCheckDistance))
        {

            if (hit.transform.tag == "checkpoint" && lastscoredobj != hit.transform.gameObject)
            {
                lastscoredobj = hit.transform.gameObject;
                scoreKeeper.CollectPickup(10);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "pickup")
        {
            scoreKeeper.CollectPickup(100);
            Destroy(other.gameObject);
           
        }

    }
    public void OnCollisionEnter(Collision other)
    {
        jumpTimer = 0;
        if (other.transform.tag != "basic") {
            bumped = true;
            proximityObj.GetComponent<proximity>().Crashed();
        }
      
        anim.SetBool("jumping", false);
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
