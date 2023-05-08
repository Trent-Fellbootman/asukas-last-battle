using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

public class ZombieTakeDamage : MonoBehaviour, TakeDamageInterface
{
    [SerializeField]
    private float health = 100.0f;

    // Start is called before the first frame update
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
            die();
        }
    }

    private void die() {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Die");

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }
}
