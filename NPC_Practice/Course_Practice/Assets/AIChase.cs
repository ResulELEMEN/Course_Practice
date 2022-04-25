using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChase : MonoBehaviour
{
    public enum AISTATE { PATROL = 0, CHASE = 1, ATTACK = 2 };
    private NavMeshAgent ThisAgent = null;
    private Transform Player = null;

    public AISTATE CurrentState
    {
        get { return _CurrentState; }
        set
        {
            StopAllCoroutines();
            _CurrentState = value;

            switch (CurrentState)
            {
                case AISTATE.PATROL:
                    StartCoroutine(StatePatrol());
                    break;

                case AISTATE.CHASE:
                    StartCoroutine(StateChase());
                    break;

                case AISTATE.ATTACK:
                    StartCoroutine(StateAttack());
                    break;
            }
        }
    }

    [SerializeField]
    private AISTATE _CurrentState = AISTATE.PATROL;

    private void Awake()
    {
        ThisAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        CurrentState = AISTATE.PATROL;
    }

    public IEnumerator StateChase()
    {
        float AttackDistance = 2f;

        while (CurrentState == AISTATE.CHASE)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < AttackDistance)
            {
                CurrentState = AISTATE.ATTACK;
                yield break;
            }


            ThisAgent.SetDestination(Player.transform.position);
            yield return null;
        }
    }

    public IEnumerator StateAttack()
    {
        float AttackDistance = 2f;

        while (CurrentState == AISTATE.ATTACK)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) > AttackDistance)
            {
                CurrentState = AISTATE.CHASE;
                yield break;
            }

            print("Attack!");
            ThisAgent.SetDestination(Player.transform.position);
            yield return null;
        }
    }

    public IEnumerator StatePatrol()
    {
        GameObject[] Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        GameObject CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
        float TargetDistance = 2f;

        while (CurrentState == AISTATE.PATROL)
        {
            ThisAgent.SetDestination(CurrentWaypoint.transform.position);

            if (Vector3.Distance(transform.position, CurrentWaypoint.transform.position) < TargetDistance)
            {
                CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CurrentState = AISTATE.CHASE;
    }
}












//using System.Collections;
//using UnityEngine;
//using UnityEngine.AI;

//public class AIChase : MonoBehaviour
//{
//    public enum AISTATE { 
//        PATROL = 0, 
//        CHASE = 1, 
//        ATTACK = 2 
//    }; // devriye takip saldýrý

//    private NavMeshAgent ThisAgent;
//    private Transform Player;
//    public AISTATE CurrentState = AISTATE.PATROL;

//    private void Awake()
//    {
//        ThisAgent = GetComponent<NavMeshAgent>();
//        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
//    }

//    private void Start()
//    {
//        //StartCoroutine(StatePatrol());
//        ChangeState(AISTATE.PATROL);
//    }

//    void Update()
//    {

//    }

//    private void ChangeState(AISTATE newState)
//    {
//        StopAllCoroutines();
//        CurrentState = newState;

//        switch (CurrentState)
//        {
//            case AISTATE.PATROL:
//                StartCoroutine(StatePatrol());
//                break;

//            case AISTATE.CHASE:
//                StartCoroutine(StateChase());
//                break;

//            case AISTATE.ATTACK:
//                StartCoroutine(StateAttack());
//                break;
//        }
//    }

//    private IEnumerator StateAttack()
//    {
//        float AttackDistance = 2f;

//        while (CurrentState == AISTATE.ATTACK)
//        {
//            if (Vector3.Distance(transform.position, Player.transform.position) > AttackDistance)
//            {
//                ChangeState(AISTATE.CHASE);
//                yield break;
//            }
//            print("Attack");
//            ThisAgent.SetDestination(Player.transform.position);
//            yield return null;
//        }
//    }

//    private IEnumerator StateChase()
//    {
//        float AttackDistance = 2f;

//        while (CurrentState == AISTATE.CHASE)
//        {
//            if (Vector3.Distance(transform.position, Player.transform.position) < AttackDistance)
//            {
//                ChangeState(AISTATE.ATTACK);
//                yield break;
//            }

//            ThisAgent.SetDestination(Player.transform.position);
//            yield return null;
//        }
//    }

//    public IEnumerator StatePatrol()
//    {
//        GameObject[] Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
//        GameObject CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
//        float TargetDistance = 2f;

//        while (CurrentState == AISTATE.PATROL)
//        {
//            ThisAgent.SetDestination(CurrentWaypoint.transform.position);

//            if (Vector3.Distance(transform.position, CurrentWaypoint.transform.position) < TargetDistance)
//            {
//                CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
//            }

//            yield return null;
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        ChangeState(AISTATE.CHASE);
//    }

//}