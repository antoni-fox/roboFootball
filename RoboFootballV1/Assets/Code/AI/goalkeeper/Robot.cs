using UnityEngine;
using UnityEngine.AI;
namespace Goalkeeper
{
    public static class Robot
    {
        public static bool DetectBall(AI _owner)
        {
            //Получаем только коллайдеры с маской layerMask, принадлежащие GameObject ball
            Collider[] hitColliders = Physics.OverlapSphere(_owner.transform.position, _owner.visionRadius, _owner.ballLayerMask);
            if (hitColliders.Length > 0)
            {
                //Вычисляем угол между направлением движения робота и центральной точкой мяча 
                float angle = Vector3.Angle(hitColliders[0].transform.position - _owner.transform.position, _owner.transform.forward);
                if (angle <= _owner.visionAngle / 2)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SetDestination(Vector3 targetPoint, NavMeshAgent navMeshAgent)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(targetPoint);
            navMeshAgent.isStopped = false;
        }

        public static void KickBall(AI _owner)
        {
            _owner.ball.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * _owner.speedBall);
            _owner.isKick = true;
            _owner.kickGoalkeeper.Kick();
            _owner.ballClass.RotateBall(_owner);
        }     

        public static void ResetPositionRobotAndBall(AI _owner)
        {
            _owner.transform.position = _owner.spawnPointRobot.position;
            _owner.transform.rotation = _owner.spawnPointRobot.rotation;

            _owner.ball.transform.position = _owner.spawnPointBall.position;
            _owner.ball.transform.rotation = _owner.spawnPointBall.rotation;
        }
        public static void ResetVariables(AI _owner)
        {
            _owner.isBallDetected = false;
            _owner.isKick = false;
            _owner.isStrike = false;
            _owner.timeRestartSearch = 0;
            _owner.navMeshAgent.ResetPath();
            _owner.navMeshAgent.velocity = Vector3.zero;
            _owner.ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _owner.ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            _owner.ball.GetComponent<Rigidbody>().Sleep();
        }
        public static void ManagerForEnterStateSTS(AI _owner)
        {
            ResetPositionRobotAndBall(_owner);
            ResetVariables(_owner);
        }

        public static void StrafeRobot(AI _owner)
        {
            if (!_owner.isRightOrLeftPatrol)
            {
                _owner.transform.position = Vector3.Lerp(_owner.gatewayPatrol[1].position, _owner.gatewayPatrol[0].position, _owner.timePatrolTwoToOne);
            }
            else
            {
                _owner.transform.position = Vector3.Lerp(_owner.gatewayPatrol[0].position, _owner.gatewayPatrol[1].position, _owner.timePatrolOneToTwo);
            }

            switch(_owner.isRightOrLeftPatrol)
            {
                case false:
                    if (_owner.timePatrolTwoToOne <= 1)
                    {
                        _owner.timePatrolTwoToOne += (Time.deltaTime / _owner.maxTimePatrol );
                    }
                    else
                    {
                        _owner.timePatrolTwoToOne = 0;
                        _owner.isRightOrLeftPatrol = true;
                    }
                    break;
                case true:
                    if (_owner.timePatrolOneToTwo <= 1)
                    {
                        _owner.timePatrolOneToTwo += (Time.deltaTime / _owner.maxTimePatrol);
                    }
                    else
                    {
                        _owner.timePatrolOneToTwo = 0;
                        _owner.isRightOrLeftPatrol = false;
                    }
                    break;
            }
           
        }
    }
}