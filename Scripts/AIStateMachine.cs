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
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer, whatIsBullet, whatIsObstacle, whatIsHealingPack;

    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField]private float walkPointRange;

    [SerializeField]private float timeBetweenAttack;

    [SerializeField] private float sightRange, attackRange, healingpackrange;
    private bool playerInSightRange, playerInAttackRange,healingpackInSightRange;

    public PlayerLogic playerLogic;
    private bool startAttack = false;
    [SerializeField] private Transform pfBullet;
    private GameObject _enemy;
    private bool aiChaseDownPlayer = false;
    private GameObject aimassist;

    private void Awake()
    {
        _state = States.Patroling;
        player = GameObject.Find("Player").transform;
        _enemy = GameObject.Find("Player");
        aimassist = GameObject.Find("AimAssist");
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
            Debug.Log("found patroling point");
            walkPointSet = true; 
        }
    }

    private void Chasing()
    {
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        Vector3 cleanedvelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 predictedposition = PredictPoint(transform.position, 65f, player.position, cleanedvelocity);
        agent.SetDestination(predictedposition);
        transform.LookAt(predictedposition);
        if (predictedposition != new Vector3(123, 456, 789))
        {
            aimassist.transform.position = predictedposition;
            transform.LookAt(aimassist.transform.position);
            if (!startAttack)
            {
                startAttack = true;
                Vector3 dir = new Vector3(1f, 0f, 0f);
                Transform bulletTransform = Instantiate(pfBullet, gameObject.transform.position + gameObject.transform.forward * 1.5f, Quaternion.identity);
                bulletTransform.GetComponent<BulletLogic>().Setup(dir, _enemy, gameObject);
                Invoke(nameof(ResetAttack), timeBetweenAttack);
            }
        }
    }

    private Vector3 PredictPoint(Vector3 PC, float SC, Vector3 PR, Vector3 VR)
    {
        Vector3 D = PC - PR;

        //! Scale of distance vector
        float d = D.magnitude;

        //! Speed of target scale of VR
        float SR = VR.magnitude;

        //% Quadratic EQUATION members = (ax)^2 + bx + c = 0

        float a = Mathf.Pow(SC, 2) - Mathf.Pow(SR, 2);

        float b = 2 * Vector3.Dot(D, VR);

        float c = -Vector3.Dot(D, D);

        if ((Mathf.Pow(b, 2) - (4 * (a * c))) < 0) //% The QUADRATIC FORMULA will not return a real number because sqrt(-value) is not a real number thus no interception
        {
            return new Vector3(123,456,789);//Player cannot be hit so this is returned as signal
        }
        //% Quadratic FORMULA = x = (  -b+sqrt( ((b)^2) * 4*a*c )  ) / 2a
        float t = (-(b) + Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * (a * c)))) / (2 * a);//% x = time to reach interception point which is = t
        //Debug.Log("t2: "+t);
        //% Calculate point of interception as vector from calculating distance between target and interception by t * VelocityVector
        return ((t * VR) + PR);
    }
  
    private void ResetAttack()
    {
        startAttack = false;
    }
    private void ResetChaseDown()
    {
        aiChaseDownPlayer = false;
    }

    private bool CheckPointingAtEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.Normalize(transform.position - player.position) * -sightRange, out hit))
        {
            Debug.DrawRay(transform.position, Vector3.Normalize(transform.position - player.position) * -hit.distance, Color.red);
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log(hit.collider.tag);
                return true;
            }
        }
        //Debug.Log("WAll in front");
        return false;
    }

    private bool CheckSightField()
    {
        Vector3 headingDir = (walkPoint - transform.position).normalized;
        Vector3 enemyDir = (player.position - transform.position).normalized;
        if (Vector3.Dot(headingDir, enemyDir) >= 0)         //enemy is in front or perpendicular to ai
        {
            //aiChaseDownPlayer = true;
            return true;
        }
        return false;
    }

   //check if enemy is in sight field => chase AND check if Ai is attacked by player => fight back 
    private void CheckSpecialCasesPatroling() 
    {
        float fixedBulletDetectionSphere = 10f;
        if (playerInSightRange)
        {
            if (Physics.CheckSphere(transform.position, fixedBulletDetectionSphere, whatIsBullet))  // player is shooting at ai
            {
                //Debug.Log("AI agressive and Chasing down player");
                //aiChaseDownPlayer = true;
                _state = States.Attacking;
            }
            if (CheckSightField())      //player can only be chased if visible to ai
            {
                walkPointSet = false;
                _state = States.Chasing;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position,sightRange,whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position,attackRange,whatIsPlayer);
        healingpackInSightRange = Physics.CheckSphere(transform.position,healingpackrange,whatIsHealingPack);
        switch (_state)
        {
            case States.Patroling:
                Patroling();
                CheckSpecialCasesPatroling();
                break;
            case States.Chasing:
                Chasing();
                if (playerInAttackRange && CheckPointingAtEnemy())  //to attack only if enemy is not behind wall
                {
                    //aiChaseDownPlayer = false;
                    _state = States.Attacking;
                    Debug.Log("Changed to attacking");
                }
                if (!playerInSightRange && !aiChaseDownPlayer)
                    _state = States.Patroling;
                break;
            case States.Attacking:
                Attacking();
                if (!playerInAttackRange || !CheckPointingAtEnemy())
                    _state = States.Chasing;
                break;
            default:
                break;
        }
    }
}
