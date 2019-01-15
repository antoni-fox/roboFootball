using UnityEngine;
using Goalkeeper.StateStuff;
namespace Goalkeeper
{

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
            Debug.Log("Entering \"Search Ball State\"");
        }

        public override void ExitState(AI _owner)
        {

            Debug.Log("Exiting \"Search Ball State\"");
        }

        public override void UpdateState(AI _owner)
        {
            _owner.isBallDetected = Robot.DetectBall(_owner);
            if (_owner.isBallDetected)
            {
                if (Vector3.Distance(_owner.transform.position, _owner.ball.transform.position) <= 2)
                    Robot.KickBall(_owner);
                Robot.StrafeRobot(_owner);
            }
            else
            {
                Robot.StrafeRobot(_owner);
            }
            Debug.Log("_owner.isBallDetected = " + _owner.isBallDetected);
            Debug.Log("Updating \"Search Ball State\"");
        }
    }
}