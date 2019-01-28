using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewAgentA : KIAction

{
    [SerializeField] private int callDoActionCounter= 0;
    [SerializeField] private int counterLimitToSpawnNewAgent = 10;
    [SerializeField] private GameObject agentPrefab;


    public override bool doAction( GameObject actor )
    {
        callDoActionCounter++;
        KIFaction faction = actor.GetComponent<KIFaction>();
        if ( callDoActionCounter >= counterLimitToSpawnNewAgent ) {
            callDoActionCounter = 0;
            GameObject newAgent = Instantiate( agentPrefab);
            KIAgent agent = newAgent.GetComponent<KIAgent>();
            agent.faction = faction;
            newAgent.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;
            faction.updateAgentMemberList( agent );

        }
        satsifiedNeed = true;
        return true;
    }
}
