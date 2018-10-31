using UnityEngine;
using UnityEngine.AI;

public class RobotMoveToDestination : MonoBehaviour {

    [SerializeField]
    private Transform _currentDestination;

    NavMeshAgent _navMeshAgent;

	// Use this for initialization
	void Start () {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
	}

    private void SetDestination()
    {
        if(_currentDestination != null)
        {
            Vector3 targetVector = _currentDestination.transform.position;
            //_navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(targetVector);
            _navMeshAgent.isStopped = true;
        }
    }

    // Update is called once per frame
    void Update () {
      
        if (_currentDestination.transform.hasChanged)
        {
            //Debug.Log("New destination");
            SetDestination();
        }
	}
}
