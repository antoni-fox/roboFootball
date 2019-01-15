using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Forward;
public class Goal : MonoBehaviour {
    public AI aI;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTrigger(Collider other)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other.tag ->>>>>>>>>>>>>>" + other.tag);
        if (other.gameObject.tag == "Ball")
        {
            if (aI.isKick)
            {
                aI.isStrike = true;
                aI.score++;
                aI.scoreText.text = aI.score + "";
            }
        }
    }
}
