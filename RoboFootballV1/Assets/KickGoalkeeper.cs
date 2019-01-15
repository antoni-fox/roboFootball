using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KickGoalkeeper : MonoBehaviour {
    public int score = 0;
    public Text scoreUI;
    public void Kick()
    {
        score++;
        scoreUI.text = score + "";
    }
}
