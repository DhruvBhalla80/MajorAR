using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreDetection : MonoBehaviour
{
    public Text score;
    private int scoreInt;

    private void Start()
    {
        scoreInt = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Player")
        {
            scoreInt += 10;
            score.text = "Score :" + scoreInt.ToString();
        }
       
    }
}
