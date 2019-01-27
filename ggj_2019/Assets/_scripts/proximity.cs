using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class proximity : MonoBehaviour
{
    public float timer,bumptimer;
    public ScoreKeeper scoreKeeper;
    public bool bumped;
    public Player player;
    public Text possibleScoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bumptimer > 0)
        {
            bumptimer -= Time.deltaTime;
            if (bumptimer <= 0)
            {
                bumped = false;
                possibleScoreText.gameObject.active = false;
                possibleScoreText.text = "";
            }
        }
        else
        { if (player.run > 4) { possibleScoreText.gameObject.active = true; } else { possibleScoreText.gameObject.active = false; } }
    }
    public void Crashed()
    {
        bumped = true;
        bumptimer = 1;
        timer = 0;
        possibleScoreText.text = "crash";
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "checkpoint" && bumped == false && player.run > 4)
        {
            possibleScoreText.gameObject.active = true;
            timer += Time.deltaTime;
            possibleScoreText.text = Mathf.Round(timer).ToString();
        }

    }
    public void OnTriggerExit(Collider other)
    {
       
        if (other.transform.tag == "checkpoint" && bumped == false)
        {
            scoreKeeper.ProximityPoints(timer);
            possibleScoreText.gameObject.active = false;

        }

    }

}
