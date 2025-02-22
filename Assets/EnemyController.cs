using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    CharacterState stats;
    //public Transform Player;
    NavMeshAgent agent;
    Animator anim;
    public float attackRaduis = 5;
    bool canAttack = true;
    float attackCooldown = 3f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<CharacterState>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
        float distance = Vector3.Distance(transform.position, LevelManager.instance.Player.position);
        if (distance < attackRaduis)
        {
            agent.SetDestination(LevelManager.instance.Player.position);
            if (distance <= agent.stoppingDistance)
            {
                if (canAttack)
                {
                    StartCoroutine(cooldown());
                    anim.SetTrigger("Attack");
                }

            }
        }
    }
    IEnumerator cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player Contacted");
            stats.ChangeHealth(-other.GetComponentInParent<CharacterState>().power);
            //Reduce Health
            // Destroy(gameObject);
        }
    }
    public void DamagePlayer()
    {
        LevelManager.instance.Player.GetComponent<CharacterState>().ChangeHealth(-stats.power);
    }
}
