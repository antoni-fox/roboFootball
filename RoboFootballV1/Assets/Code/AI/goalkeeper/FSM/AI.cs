using UnityEngine;
using Goalkeeper.StateStuff;
using UnityEngine.AI;
using UnityEngine.UI;
namespace Goalkeeper
{
    public struct DisAndName//Дистанция и имя
    {
        public float distance;//дистанция
        public string name;//имя
    }
    public class AI : MonoBehaviour
    {
        public Ball ballClass;
        public KickGoalkeeper kickGoalkeeper;
        public NavMeshAgent navMeshAgent;

        public bool isBallDetected = false;

        public LineRenderer lineRenderer;
        public LineRenderer lineRendererBall;
        public Vector3 ballCurrentPosition;
        public Vector3 ballLastPosition;

        public int ballLayerMask;
        public int[] staticObstaclesMasks;
        public int[] dynamicObstaclesMasks;
        public Vector3[] lastPositionBall;
        public GameObject ball;

        public float visionRadius = 7;
        public float visionAngle = 150;

        public float rotateAngle;
        public Vector3 fieldSize;

        public StateMachine<AI> StateMachine { get; set; }

        //новая логика
        public Transform spawnPointRobot, spawnPointBall;
        public Transform waitingPoint;
        public Transform emptyForwardRobot;//Позиция для движение вперёд 
        public Transform emptyBallPosition;//Позиция для удара
        public Transform gateway1;//Ворота №1
        public Transform gateway2;//Ворота №2

        public bool isKick;//Был удар?
        public bool isStrike;//Было попадание?
        public int randomAngleCoof;//Коэфициент рандомного поворота, используется в функции RandomRotateRobot
        public int randomMoveCoof;//Коэфициент рандомного поворота, используется в функции RandomRotateRobot
        public int score = 0;//кол-во очков
        public int speedBall;//скорость удара
        public float timeRestartSearch;//Время, предназначенное для проверки того что не завис ли персонаж?
        public LayerMask gatewayLayerMaskl;//Маска ворот
        public Text scoreText;//счётчик очков, UI

        public Transform[] gatewayPatrol;
        public float timePatrolOneToTwo, timePatrolTwoToOne, maxTimePatrol;
        public bool isRightOrLeftPatrol;//в какую сторону стрейфит вратарь, фолс в левую, тру в правую
        void Start()
        {
            //Search ball by tag
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            //Add ball object
            ball = balls[0];
            ballClass = ball.GetComponent<Ball>();
            //Create ball layerMask
            ballLayerMask = 1 << balls[0].layer;

            lineRenderer = GetComponent<LineRenderer>();

            fieldSize = new Vector3(0, 23.5f, 23.5f);

            navMeshAgent = GetComponent<NavMeshAgent>();

            StateMachine = new StateMachine<AI>(this);
            StateMachine.ChangeState(StopState.Instance);

        }

        // Update is called once per frame
        void Update()
        {
            StateMachine.Update();
        }
    }
}
