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
        if (targetPoint != null)
        {
            //_navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetPoint);
            navMeshAgent.isStopped = true;
        }
    }



    //Функция генерирует массив точек, на основе центральной точки и радиуса
    private static List<Vector3> generate2DPointsAroundCircle(Vector3 center, float radius, int pointsAmount)
    {
        List<Vector3> points = new List<Vector3>();
        float slice = 2 * Mathf.PI / pointsAmount;
        for (int i=0; i < pointsAmount; i++)
        {
            float angle = slice * i;
            float x = (float)(center.x + radius * Mathf.Cos(angle));
            float z = (float)(center.z + radius * Mathf.Sin(angle));
            points.Add(new Vector3(x, 0, z));
        }
        return points;
    }


    public static void getRandomFreePointInVisionAngle(AI _owner)
    {
        List<Vector3> points = generate2DPointsAroundCircle(_owner.transform.position, _owner.visionRadius, 10);
        RaycastHit hit;
        //Vector3 freePoint;
        foreach (Vector3 point in points)
        {
            Physics.Raycast(_owner.transform.position, _owner.transform.TransformDirection(point), out hit, _owner.visionRadius);
            Debug.DrawRay(_owner.transform.position, _owner.transform.TransformDirection(point) * _owner.visionRadius, Color.red);
        }

        //return freePoint;
    }

    public static void visualizePath(NavMeshAgent robot, LineRenderer line)
    {

        if (robot.hasPath) { 

            line.positionCount = robot.path.corners.Length;
            line.SetPositions(robot.path.corners);
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }



}
