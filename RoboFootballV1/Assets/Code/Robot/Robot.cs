using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Robot
{

    public static bool detectBall(AI _owner)
    {
        //Получаем только коллайдеры с маской layerMask, принадлежащие GameObject ball
        Collider[] hitColliders = Physics.OverlapSphere(_owner.transform.position, _owner.visionRadius, _owner.ballLayerMask);
        if (hitColliders.Length > 0)
        {
            
            //Вычисляем угол между направлением движения робота и центральной точкой мяча 
            float angle = Vector3.Angle(hitColliders[0].transform.position - _owner.transform.position, _owner.transform.forward);

            if (angle <= _owner.visionAngle / 2)
            {

                //Вычисляем конечную точку нормализованного вектора по направлению движения
                Vector3 endPoint = _owner.transform.position + _owner.transform.forward.normalized;
                getPointPosition2D(_owner.transform.position, endPoint, hitColliders[0].transform.position);
                _owner.lastPositionBall[1] = _owner.lastPositionBall[0];
                _owner.lastPositionBall[0] = hitColliders[0].transform.position;
                return true;
            }

        }

        return false;
    }

    // Функция проверяет положение точки относительно линии, построенной первыми двумя точками
    public static int getPointPosition2D(Vector3 LinePoint1, Vector3 LinePoint2, Vector3 checkPoint)
    {
        //Вычисляем позицию точки относительно линии, состоящей из точек (LinePoint1, LinePoint2)
        float position = (checkPoint.x - LinePoint1.x) * (LinePoint2.z - LinePoint1.z) -
            (checkPoint.z - LinePoint1.z) * (LinePoint2.x - LinePoint1.x);

        // Вычисляем с какой стороны находится точка
        //Cправа
        if (position > 0) return 1;
        //Слева
        if (position < 0) return -1;
        //На линии
        return 0;

    }


    public static void setDestination(Vector3 targetPoint, NavMeshAgent navMeshAgent)
    {
       // Debug.Log("targetPoint = " + targetPoint);
        if (targetPoint != null)
        {
            //_navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetPoint);
            navMeshAgent.isStopped = false;
        }
    }

    public static void KickBall(AI _owner)
    {
        _owner.ball.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * _owner.speedBall);
        _owner.isKick = true;
    }

    //--------------------------------------------------------->Начало логики для SearchBallState
    public static void RandomRotateRobot(AI _owner)
    {
        int angle = UnityEngine.Random.Range(-_owner.randomAngleCoof, _owner.randomAngleCoof);
        Quaternion turn = Quaternion.Euler(0, angle, 0); // Создаём новый поворот 
        _owner.transform.rotation = _owner.transform.rotation * turn;
    }

    public static void RandomMoveRobot(AI _owner)
    {
        _owner.emptyForwardRobot.localPosition = new Vector3(0, 0, UnityEngine.Random.Range(0, _owner.randomMoveCoof));
        Robot.setDestination(_owner.emptyForwardRobot.position, _owner.navMeshAgent);
    }

    public static bool CheckRemainingDistance(AI _owner)
    {
        NavMeshAgent nMA = _owner.navMeshAgent;
        if (nMA.remainingDistance > nMA.stoppingDistance)
            return false;
        else
            return true;
    }

    public static void RandomMoveAndRotateManager(AI _owner)
    {
        if (CheckRemainingDistance(_owner))
        {
            Robot.RandomRotateRobot(_owner);
            Robot.RandomMoveRobot(_owner);
        }
        else
        {

        }
    }

    //<---------------------------------------------------------Конец логики для SearchBallState

    //--------------------------------------------------------->Начало логики для MoveToBallState

    public static void RotateBallToGateway(AI _owner)
    {
        DisAndName disAndName = CalculatingDistanceToGateway(_owner);
        if (disAndName.name == "HitPoint")
        {
            _owner.ball.transform.LookAt(_owner.gateway1);
        }
        else
        {
            _owner.ball.transform.LookAt(_owner.gateway2);
        }
    }

    public static void ManagerForPrepareKick()
    {

    }
    public static void StartSetDestinationToPrepareKick(AI _owner)
    {
        setDestination(_owner.emptyBallPosition.position, _owner.navMeshAgent);
    }

    public static void UpdateSetDestinationToPrepareKick(AI _owner)
    {     
        if(Vector3.Distance(_owner.emptyBallPosition.position, _owner.transform.position) < 0.5)
        {
            KickBall(_owner);
        }
    }
    public static DisAndName CalculatingDistanceToGateway(AI _owner)
    {
        float distanceToGateway1 = 0, distanceToGateway2 = 0;

        distanceToGateway1 = Vector3.Distance(_owner.ball.transform.position, _owner.gateway1.position);
        distanceToGateway2 = Vector3.Distance(_owner.ball.transform.position, _owner.gateway2.position);
        if (distanceToGateway1 > distanceToGateway2)
            return new DisAndName() { distance = distanceToGateway2, name = "HitPoint1" };
        else if (distanceToGateway1 < distanceToGateway2)
            return new DisAndName() { distance = distanceToGateway1, name = "HitPoint" };
        else
        {
            if (UnityEngine.Random.Range(0, 1) == 0)
                return new DisAndName() { distance = distanceToGateway1, name = "HitPoint" };
            else
                return new DisAndName() { distance = distanceToGateway2, name = "HitPoint1" };
        }
    }

    public static void ManagerForStartStateMTBS(AI _owner)
    {
        RotateBallToGateway(_owner);
        StartSetDestinationToPrepareKick(_owner);
    }
    public static void ManagerForUpdateStateMTBS(AI _owner)
    {
        UpdateSetDestinationToPrepareKick(_owner);
        if (_owner.isStrike)
        {
            _owner.stateMachine.ChangeState(StopState.Instance);
        }
        else if (_owner.isKick & !_owner.isStrike)
        {
            _owner.time += Time.deltaTime;
            if (_owner.time > 10)
            {
                ResetVariables(_owner);
                _owner.stateMachine.ChangeState(SearchBallState.Instance);
            }
        }
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
        _owner.time = 0;
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
    //<---------------------------------------------------------Конец логики для MoveToBallState

}