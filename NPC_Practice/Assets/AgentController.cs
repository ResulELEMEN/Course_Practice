using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] agents;
    [SerializeField]
    private Transform gatherPoint;

    private AgentState agentOrder;
    bool tus;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // input degerlendirme
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tus = !tus;
        }
        //
        agentOrder = tus ? AgentState.Toplan : AgentState.Devriye ;
        ToplanmaNormal(agentOrder);
    }

    private void ToplanmaNormal(AgentState state)
    {
        foreach (var agent in agents)
        {
            agent.GetComponent<AiTestMove>().currentState = state;
            agent.GetComponent<AiTestMove>().toplanmaNoktasi = gatherPoint.position;
        }
    }

    //private void Normal()
    //{
    //    foreach (var agent in agents)
    //    {
    //        agent.GetComponent<AiTestMove>().currentState = AgentState.Devriye;
    //    }
    //}
}
