using UnityEngine;
using UnityEngine.AI;

public class ShowRobotPath : MonoBehaviour {
       
    private NavMeshAgent _robotAgent;

    private LineRenderer _lineRenderer;
         
    // Use this for initialization
    void Start()
    {
        _robotAgent = this.GetComponent<NavMeshAgent>();
        _lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        if (_robotAgent.hasPath)
        {

            _lineRenderer.positionCount = _robotAgent.path.corners.Length;
            _lineRenderer.SetPositions(_robotAgent.path.corners);
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }

    }


}

	

