using UnityEngine;
using StateStuff;
using UnityEngine.AI;

public class AI : MonoBehaviour {

    public NavMeshAgent navMeshAgent;

    public bool isBallDetected = false;

    public  LineRenderer lineRenderer;

    public Vector3 ballCurrentPosition;
    public Vector3 ballLastPosition;

    public int ballLayerMask;
    public int[] staticObstaclesMasks;
    public int[] dynamicObstaclesMasks;

    public GameObject ball;

    public float visionRadius = 10;
    public float visionAngle = 90;

    public Vector3 fieldSize;    

    public StateMachine<AI> stateMachine { get; set; }

	void Start () {
        //Search ball by tag
        GameObject [] balls = GameObject.FindGameObjectsWithTag("Ball");
        //Add ball object
        ball = balls[0];
        //Create ball layerMask
        ballLayerMask = 1 << balls[0].layer;

        lineRenderer = GetComponent<LineRenderer>();

        fieldSize = new Vector3(0, 23.5f, 23.5f);

        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(StopState.Instance);

	}
	
	// Update is called once per frame
	void Update ()
    {
            stateMachine.Update();
	}
}
