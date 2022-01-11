using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMachine : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttack;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public PlayerLogic playerLogic;
    public bool startAttack = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        playerLogic = GetComponent<PlayerLogic>();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
        ResetAttack();
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void Chasing()
    {
        agent.SetDestination(player.position);
        ResetAttack();
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!startAttack)
        {
            startAttack = true;
        }
    }

    private void ResetAttack()
    {
        startAttack = false;
    }
    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position,sightRange,whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position,attackRange,whatIsPlayer);
        //playerInSightRange = Physics.Raycast(transform.position,transform.position-player.position,sightRange,whatIsPlayer);
        //playerInAttackRange = Physics.Raycast(transform.position,new Vector3(player.position.x,player.position.y),attackRange,whatIsPlayer);
        //if (playerInSightRange)
            //Debug.Log("player here");
        
        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        if (playerInSightRange && !playerInAttackRange)
            Chasing();
        if (playerInSightRange && playerInAttackRange)
            Attacking();
    }
}
