using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMoveBehavior : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The time between two navigation path calculations, in seconds.")]
    private float navigationRefreshCycle = 0.5f;

    private float currentMoveTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentMoveTime += Time.deltaTime;

        if (currentMoveTime > navigationRefreshCycle)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent.enabled)
            {
                agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            }

            currentMoveTime = currentMoveTime - navigationRefreshCycle;
        }
    }
}
