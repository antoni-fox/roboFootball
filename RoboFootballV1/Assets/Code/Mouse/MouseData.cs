using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseData : MonoBehaviour {

    float sensitivityVert = 9.0f;
    float _rotationX = 0f;
    float mouseX = 0;
    float mouseY = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float currentMouseX =  Input.GetAxis("Mouse X");
        float currentMouseY = Input.GetAxis("Mouse Y");

        //Вращение по оси Y
        if (currentMouseX != mouseX)
        {
            mouseX = currentMouseX;

            //Debug.Log("X:"+ mouseX);
        }
        //Вращение по оси X
        if (currentMouseY != mouseY)
        {
            mouseY = currentMouseY;
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, -45.0f, 45.0f);
            float rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
            Debug.Log("Y:" + _rotationX);
        }
    }
}
