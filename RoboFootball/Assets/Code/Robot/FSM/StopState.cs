using UnityEngine;
using StateStuff;


public class StopState : State<AI>
{
    private static StopState _instance;

    private StopState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public static StopState Instance
    {
        get
        {
            if(_instance == null)
            {
                new StopState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering Stop State");
    }

    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting Stop State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.isBallDetected)
        {
            
        }
        else
        {
            _owner.stateMachine.ChangeState(SearchBallState.Instance);
        }
    }
}
