using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Forward
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
            Ball.Kick(_owner);
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
			Robot.SetDestination(_owner.emptyForwardRobot.position, _owner.navMeshAgent);
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
			SetDestination(_owner.emptyBallPosition.position, _owner.navMeshAgent);
		}

		public static void UpdateSetDestinationToPrepareKick(AI _owner)
		{
			SetDestination(_owner.emptyBallPosition.position, _owner.navMeshAgent);
			if (Vector3.Distance(_owner.emptyBallPosition.position, _owner.transform.position) < 1)
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
				_owner.StateMachine.ChangeState(StopState.Instance);
			}
			else if (_owner.isKick & !_owner.isStrike)
			{
				_owner.time += Time.deltaTime;
				if (_owner.time > 10)
				{
					ResetVariables(_owner);
					_owner.StateMachine.ChangeState(SearchBallState.Instance);
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
}