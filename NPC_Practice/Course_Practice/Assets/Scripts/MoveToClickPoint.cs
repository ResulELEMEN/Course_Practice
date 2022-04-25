// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class MoveToClickPoint : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("TestTarget").transform; // caching
        //Debug.Log(agent.destination);
        
    }

    void Update()
    {
        //Debug.Log(agent.remainingDistance);

        //if (target != null && agent.remainingDistance < 2f)
        //{
        //    agent.destination = target.position;
        //}

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(
                Camera.main.ScreenPointToRay(Input.mousePosition),
                out hit, 
                1000))
            {
                //Debug.Log("Casting");
                agent.destination = hit.point;
            }
        }
    }
}