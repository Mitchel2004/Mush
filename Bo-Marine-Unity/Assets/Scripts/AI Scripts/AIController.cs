using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] float chaseRange = 5f;

    [Header("Transforms")]
    public Transform target;

    [Header("Speeds")]
    [SerializeField] float turnSpeed = 5f;

    [Header("Health")]
    //AI geeft damage aan de speler
    public float playerDamage = 100f;
    [SerializeField] float AIHealth = 100f;

    //hit the player with AI
    private bool ableToHit;
    internal bool hit;

    NavMeshAgent agent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvroked = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvroked)
        {
            EngageTarget();
        }
        else if(distanceToTarget <= chaseRange)
        {
            isProvroked = true;
        }
    }

    void EngageTarget()
    {
        FaceTarget();
        if (distanceToTarget >= agent.stoppingDistance)
        {
            ChaseTarget();
        }
        if (distanceToTarget <= agent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("Attack", false);
        GetComponent<Animator>().SetBool("Walking", true);
        agent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            if(agent.remainingDistance < 1.5f)
            {
                GetComponent<Animator>().SetBool("Walking", false);
                GetComponent<Animator>().SetBool("Attack", true);
            }
        }
        Debug.Log(name + " has seeked and is destroying " + target.name);
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public void TakeDamage(float damageAmount)
    {
        AIHealth -= damageAmount;
        if (AIHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        
        target target = other.transform.GetComponent<target>();
        if (other.gameObject.tag == "enemy")
        {
            hit = true;
           
            if (ableToHit == true)
            {
                ableToHit = false;
            }
            target.takeDamage(playerDamage);
        }
    }*/
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
