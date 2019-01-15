using Forward;
using UnityEngine;

public class Ball : MonoBehaviour {
    public Rigidbody rigidbodyBall;
    public LayerMask layerMask;
    public AI forward;
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
    public static void Kick(AI thisOwner)
    {
        thisOwner.ballClass.forward = thisOwner;
    }
    public void RotateBall(Goalkeeper.AI thisOwner)
    {
        float angleForward2Goalkeeper = Vector3.Angle(forward.transform.position, thisOwner.transform.position);
        angleForward2Goalkeeper += 90 + Random.Range(-45, 45);
        rigidbodyBall.AddRelativeForce(new Vector3(0,angleForward2Goalkeeper,0));
    }
}