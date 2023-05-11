using UnityEngine;
using UnityEngine.AI;

public class ZombieTakeDamage : MonoBehaviour, TakeDamageInterface
{
    [SerializeField]
    private float health = 100.0f;

    private Animator animator;
    private NavMeshAgent agent;
    private CapsuleCollider capsuleCollider;
    private ZombieController zombieController;
    private static readonly int Die1 = Animator.StringToHash("Die");

    // Start is called before the first frame update
    private void Awake()
    {
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        zombieController = GetComponent<ZombieController>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die() {
        animator.SetTrigger(Die1);

        agent.enabled = false;
        zombieController.enabled = false;
        capsuleCollider.enabled = false;
    }
}
