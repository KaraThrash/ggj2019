using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public float score;
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CollectPickup(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }
    public void ProximityPoints(float scored)
    {
        score += scored;
        scoreText.text = score.ToString();
    }
}
