using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public Rigidbody rigidbodyBall;
    public LayerMask layerMask;
    void CheckOnWall()
    {
        RaycastHit[] hits = new RaycastHit[4];
        if (Physics.Raycast(this.transform.position, Vector3.back, out hits[0], 1, layerMask))
        {
            rigidbodyBall.AddRelativeForce(Vector3.forward * 20);
        }
        else if (Physics.Raycast(this.transform.position, Vector3.forward, out hits[0], 1, layerMask))
        {
            rigidbodyBall.AddRelativeForce(Vector3.back * 20);
        }
        else if (Physics.Raycast(this.transform.position, Vector3.right, out hits[0], 1, layerMask))
        {
            rigidbodyBall.AddRelativeForce(Vector3.left * 20);
        }
        else if(Physics.Raycast(this.transform.position, Vector3.left, out hits[0], 1, layerMask))
        {
            rigidbodyBall.AddRelativeForce(Vector3.right * 20);
        }
    }
    private void Update()
    {
        CheckOnWall();
    }
}