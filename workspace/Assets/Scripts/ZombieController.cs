using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ZombieController : MonoBehaviour
{
    [SerializeField] [Tooltip("The time between two navigation path calculations, in seconds.")]
    private float navigationRefreshCooldown = 0.1f;

    [SerializeField] [Tooltip("The range within which the zombie will wander before seeing the player.")]
    private float wanderRange = 10.0f;

    [SerializeField] [Tooltip("Minimum time between two consecutive attacks.")]
    private float attackCooldown = 1.0f;

    [FormerlySerializedAs("attackRange")]
    [SerializeField]
    [Tooltip("The range within which the zombie can attack the player.")]
    private float attackEnterDistance = 1.0f;

    [SerializeField]
    [Tooltip("If the distance between the player and the zombie is greater than this, the zombie will start moving.")]
    private float attackExitDistance = 1.5f;

    [SerializeField] [Tooltip("The damage dealt to the player when attacked.")]
    private float attackDamage = 10.0f;

    [SerializeField] [Tooltip("The time from the start of an attack to the actual damage.")]
    private float attackDamageDelay = 0.5f;

    [SerializeField] [Tooltip("The height of the head above the origin.")]
    private float headOffset = 0.9f;

    private float currentDestinationRefreshCooldown = 0;
    private Vector3? lastPlayerPosition = null;
    private float currentAttackCooldown = 0;

    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private Animator animator;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackToRun = Animator.StringToHash("AttackToRun");

    private bool enteredAttackRange = false;

    private void Awake()
    {
        if (attackDamageDelay > attackCooldown)
        {
            throw new ArgumentException("Attack damage delay must be smaller than attack cooldown.");
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _refreshDestination();
    }

    // Update is called once per frame
    void Update()
    {
        var playerOffset = player.transform.position - transform.position;
        if (Mathf.Abs(playerOffset.y) < 1.8f)
        {
            playerOffset.y = 0;
        }

        var playerDistance = playerOffset.magnitude;

        if (playerDistance < attackEnterDistance)
        {
            enteredAttackRange = true;
            animator.SetTrigger(Attack);

            // player in range, perform attack
            navMeshAgent.isStopped = true;

            // look at the player while still standing upright
            var lookAtPosition = player.transform.position;
            lookAtPosition.y = transform.position.y;
            transform.LookAt(lookAtPosition);

            if (currentAttackCooldown > 0)
            {
                currentAttackCooldown -= Time.deltaTime;
            }
            else
            {
                animator.SetTrigger(Attack);

                Invoke(nameof(TryDamagePlayer), attackDamageDelay);
                currentAttackCooldown += attackCooldown;

                currentDestinationRefreshCooldown = 0;
            }
        }
        else
        {
            if (playerDistance > attackExitDistance || !enteredAttackRange)
            {
                enteredAttackRange = false;

                navMeshAgent.isStopped = false;
                animator.SetTrigger(AttackToRun);
                currentDestinationRefreshCooldown -= Time.deltaTime;

                if (currentDestinationRefreshCooldown <= 0)
                {
                    _refreshDestination();

                    currentDestinationRefreshCooldown += navigationRefreshCooldown;
                }
            }
        }
    }

    private void TryDamagePlayer()
    {
        var playerOffset = player.transform.position - transform.position;
        if (Mathf.Abs(playerOffset.y) < 1.8f)
        {
            playerOffset.y = 0;
        }

        var playerDistance = playerOffset.magnitude;

        if (playerDistance < attackExitDistance)
        {
            player.GetComponent<TakeDamageInterface>().TakeDamage(attackDamage);
        }
    }

    void _refreshDestination()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, headOffset, 0),
                (player.transform.position - transform.position).normalized,
                out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                lastPlayerPosition = player.transform.position;
            }
        }

        if (lastPlayerPosition.HasValue)
        {
            navMeshAgent.SetDestination(lastPlayerPosition.Value);
        }
        else
        {
            navMeshAgent.SetDestination(
                NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * wanderRange,
                    out NavMeshHit navMeshHit, 2 * wanderRange, NavMesh.AllAreas)
                    ? navMeshHit.position
                    : transform.position);
        }
    }
}