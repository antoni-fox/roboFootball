using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    private Vector3 _getPos = Vector3.zero;

    // Use this for initialization
    void Start () {
        Debug.Log(this.transform.hasChanged);
        _getPos = this.transform.position;
        StartCoroutine(WaitAndCheck(0.1f));

    }

    IEnumerator WaitAndCheck(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        if (_getPos == this.transform.position)
        {
            this.transform.hasChanged = false;
            Debug.Log(this.transform.hasChanged);

        }
        else
        {
            Debug.Log(this.transform.hasChanged);

        }
    }


}
