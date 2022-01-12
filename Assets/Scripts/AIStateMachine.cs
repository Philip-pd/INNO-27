using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMachine : MonoBehaviour
{
    private enum States
    {
        Patroling,
        Chasing,
        Attacking
    }
    private States _state;
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
    [SerializeField] private Transform pfBullet;
    [SerializeField] private GameObject Enemy;

    private void Awake()
    {
        _state = States.Patroling;
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
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!startAttack)
        {
            startAttack = true;
            Vector3 dir = new Vector3(1f, 0f, 0f);
            Transform bulletTransform = Instantiate(pfBullet, gameObject.transform.position + gameObject.transform.forward * 1.5f, Quaternion.identity);
            bulletTransform.GetComponent<BulletLogic>().Setup(dir, Enemy, gameObject);
            Invoke(nameof(ResetAttack), 2);
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

        switch (_state)
        {
            case States.Patroling:
                Patroling();
                if (playerInSightRange)
                    _state = States.Chasing;
                break;
            case States.Chasing:
                Chasing();
                if (playerInAttackRange)
                    _state = States.Attacking;
                if (!playerInSightRange)
                    _state = States.Patroling;
                break;
            case States.Attacking:
                Attacking();
                if (!playerInAttackRange)
                    _state = States.Chasing;
                break;
            default:
                break;
        }
    }
}
