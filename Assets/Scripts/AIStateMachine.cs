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
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer, whatIsObstacle;

    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField]private float walkPointRange;

    [SerializeField]private float timeBetweenAttack;

    [SerializeField] private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    public PlayerLogic playerLogic;
    private bool startAttack = false;
    [SerializeField] private Transform pfBullet;
    private GameObject _enemy;

    private void Awake()
    {
        _state = States.Patroling;
        player = GameObject.Find("Player").transform;
        _enemy = GameObject.Find("Player");
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
        if (distanceToWalkPoint.magnitude < 3f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Vector3 randDir = new Vector3(UnityEngine.Random.Range(-1f, 1f),0, UnityEngine.Random.Range(-1f, 1f)).normalized;
        
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(randomX,transform.position.y, randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 4f, whatIsGround) && !Physics.Raycast(walkPoint, transform.up, 10f, whatIsObstacle))
        {
            Debug.Log("found point");
            walkPointSet = true; 
        }
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
            bulletTransform.GetComponent<BulletLogic>().Setup(dir, _enemy, gameObject);
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    private void ResetAttack()
    {
        startAttack = false;
    }

    private bool CheckObstacleBetweenPlayers()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.Normalize(transform.position - player.position) * -sightRange,out hit))
        {
            Debug.DrawRay(transform.position, Vector3.Normalize(transform.position - player.position) * -hit.distance, Color.red);
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
            //Debug.Log("WAll in front");
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position,sightRange,whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position,attackRange,whatIsPlayer);      

        switch (_state)
        {
            case States.Patroling:
                Patroling();
                if (playerInSightRange)
                {
                    walkPointSet = false;
                    _state = States.Chasing;
                }
                break;
            case States.Chasing:
                Chasing();
                if (playerInAttackRange)
                    _state = States.Attacking;
                if (!playerInSightRange)
                    _state = States.Patroling;
                break;
            case States.Attacking:
                if (!CheckObstacleBetweenPlayers())
                {
                    _state = States.Chasing;
                    break;
                }
                Attacking();
                if (!playerInAttackRange)
                    _state = States.Chasing;
                break;
            default:
                break;
        }
    }
}
