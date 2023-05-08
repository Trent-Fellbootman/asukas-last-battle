using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMoveBehavior : MonoBehaviour
{
    [SerializeField] [Tooltip("The time between two navigation path calculations, in seconds.")]
    private float navigationRefreshCycle = 0.5f;

    [SerializeField] [Tooltip("The range within which the zombie will wander before seeing the player.")]
    private float wanderRange = 10.0f;

    private float _currentMoveTime = 0;
    private Vector3? _lastPlayerPosition = null;

    // Start is called before the first frame update
    void Start()
    {
        _refreshDestination();
    }

    // Update is called once per frame
    void Update()
    {
        _currentMoveTime += Time.deltaTime;

        if (_currentMoveTime > navigationRefreshCycle)
        {
            _refreshDestination();

            _currentMoveTime = _currentMoveTime - navigationRefreshCycle;
        }
    }

    void _refreshDestination()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized,
                out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _lastPlayerPosition = player.transform.position;
            }
        }

        if (_lastPlayerPosition.HasValue)
        {
            agent.destination = _lastPlayerPosition.Value;
        }
        else
        {
            agent.destination =
                NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * wanderRange,
                    out NavMeshHit navMeshHit, 2 * wanderRange, NavMesh.AllAreas)
                    ? navMeshHit.position
                    : transform.position;
        }
    }
}