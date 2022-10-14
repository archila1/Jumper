using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float AttackTimer = 0f;
    bool isAlive = true;
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        if (Vector3.Distance(transform.position, player.transform.position) < 8f && player.GetComponent<PlayerController>().isAlive)
        {
            navMeshAgent.SetDestination(player.transform.position);
            animator.SetBool("PlayerInRange", true);
            Attack();
        }
        else
        {
            animator.SetBool("PlayerInRange", false);
        }
    }


    private void Attack()
    {
        
        if (AttackTimer >= 1.5f && isAlive)
        {
            if(Vector3.Distance(transform.position, player.transform.position) < 1.5f)
            {
                animator.SetTrigger("PlayerInAttackRange");
                player.GetComponent<PlayerController>().PlayerDeath();
                animator.SetBool("PlayerDeath", true);
                AttackTimer = 0f;
            }
            


        }
        else
        {
            AttackTimer += Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            navMeshAgent.enabled = false;
            animator.SetTrigger("EnemyDeath");
            isAlive = false;
            Destroy(other.gameObject);

            Destroy(gameObject, 3f);
        }
    }

}
