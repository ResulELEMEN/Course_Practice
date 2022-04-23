using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    Toplan,
    Devriye,
    Takip,
    Saldiri
}

public class AiTestMove : MonoBehaviour
{

    [SerializeField]
    private Transform[] targets; // devriye noktalarý
    private NavMeshAgent agent;

    private int targetIndex;

    private Transform player;

    // Toplan
    private bool toplanma;
    public Vector3 toplanmaNoktasi;

    // Durum
    public AgentState currentState; // mevcut durum

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = AgentState.Devriye;
    }

    // Update is called once per frame
    void Update()
    {
        // Devriye
        if (currentState == AgentState.Devriye)
        {
            Debug.Log("Devriye geziyorum.....");

            if (agent.remainingDistance < 1f)  // vardý
            {
                targetIndex++; // yeni hedef indeksi
                targetIndex = targetIndex % targets.Length;  // round robin
                agent.SetDestination(targets[targetIndex].position);
            }
        }

        // Toplan
        if (currentState == AgentState.Toplan)
        {

            agent.SetDestination(toplanmaNoktasi);

            // bu iþ bittikten sonra
            currentState = AgentState.Devriye;
        }

        // Chase  - Takip
        if (currentState == AgentState.Takip)
        {
            Debug.Log("Takip ediyorum...");

            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) < 5f)
            {
                currentState = AgentState.Saldiri;
            }
            else
            {
                currentState = AgentState.Devriye;
            }
        }

        // Attack - Saldýrý
        if (currentState == AgentState.Saldiri)
        {
            Debug.Log("Saldýrýyorum...");
            if (Vector3.Distance(transform.position, player.position) > 3f)
            {
                currentState = AgentState.Takip;
            }
            agent.SetDestination(player.position);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // 
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Gördüm...");
            currentState = AgentState.Takip;
        }
    }
}
