using UnityEngine;
using StateStuff;


public class MoveToBallState : State<AI>
{
    private static MoveToBallState _instance;

    private MoveToBallState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public static MoveToBallState Instance
    {
        get
        {
            if (_instance == null)
            {
                new MoveToBallState();
            }

            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering \"Move to ball State\"");
        Robot.ManagerForStartStateMTBS(_owner);
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting \"Move to ball State\"");
    }

    public override void UpdateState(AI _owner)
    {
        Robot.ManagerForUpdateStateMTBS(_owner);
        Debug.Log("Update \"Move to ball State\"");
    }
}
