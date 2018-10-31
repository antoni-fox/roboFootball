using UnityEngine;
using StateStuff;


public class SearchBallState : State<AI>
{
    private static SearchBallState _instance;

    private SearchBallState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public static SearchBallState Instance
    {
        get
        {
            if (_instance == null)
            {
                new SearchBallState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Search Ball State");
    }

    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting Search Ball State");
    }

    public override void UpdateState(AI _owner)
    {

        _owner.isBallDetected = Robot.detectBall(_owner);
        if (!_owner.isBallDetected) {
            _owner.navMeshAgent.ResetPath();
            Robot.getRandomFreePointInVisionAngle(_owner);
        }
        else
        {
            Robot.setDestination(_owner.ball.transform.position, _owner.navMeshAgent);
            //_owner.stateMachine.ChangeState(MoveToBallState.Instance);
        }
        Robot.visualizePath(_owner.navMeshAgent, _owner.lineRenderer);
        Debug.Log("Ball is detected == " + _owner.isBallDetected);
        

    }


}
