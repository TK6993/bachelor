using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConquerNeedStationA : KIAction {

    [SerializeField] Bedurfniss wantedNeed;
    [SerializeField] GameObject needStationToConquer;
    [SerializeField] GameObject responsibleAgent;
    private bool publishedTask = false;
    KIFaction agentsFaction;
    NeedStation needStation;
    NavMeshAgent navAgent;
    //KIFaction faction;

    public override bool doAction( GameObject actor )
    {
        // Dieese Aktion wird von einer Fraktion an einen agent weiter gegeben deswegen hier die Unterscheidung , denn Fraktion und Agent gehen unterschiedlich mit der Aktion um
        this.actor = actor;// Kann sowohl eine Fraktion als auch ein Agent sein.
        KIFaction faction = actor.GetComponent<KIFaction>();
        if ( faction != null )
        {   
            if ( wantedNeed == null ) return true;
            if(!publishedTask && !satsifiedNeed) {
                needStationToConquer = faction.worldneedManager.getNearestPointofSatisfaction( wantedNeed, gameObject );
                if ( needStationToConquer == null ) { // wenn es keine need station mehr auf der map gibt
                    satsifiedNeed = true;
                    wantedNeed = null;
                    return true;
                   }
                needStationToConquer.GetComponent<NeedStation>().isInPossession = true;

                actor.GetComponent<KIFaction>().taskForAgents = this;
                publishedTask = true;
                return false;
            }
           else { return false; }
        }

        else {
            KIAgent agent = actor.GetComponent<KIAgent>();
            return agentConqerNeedStationAgent( agent);



        }

    }

    private bool agentConqerNeedStationAgent( KIAgent agent )
    {
        if ( responsibleAgent == null )
        {
            agentsFaction = agent.faction;
            agentsFaction.taskForAgents = null;
            responsibleAgent = agent.gameObject;
            needStation = needStationToConquer.GetComponent<NeedStation>();
            navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
            navAgent.SetDestination( needStationToConquer.transform.position );
            return false;
        }
       else if ( responsibleAgent == agent.gameObject ) {

            if ( navAgent.remainingDistance <= 0.005f ) {
                satsifiedNeed = true;
                agentsFaction.listOfAgentNeedStations[ wantedNeed.name ].Add( needStationToConquer );
                needStationToConquer.GetComponent<NeedStation>().ownerFaction = agentsFaction.gameObject;
                needStationToConquer.transform.GetChild( 0 ).gameObject.GetComponent<SpriteRenderer>().color = agentsFaction.factionColor;
                needStationToConquer = null;
                wantedNeed = null;
                responsibleAgent = null;
                agentsFaction = null;
                needStation = null;
                navAgent = null;
                publishedTask = false;

                return true;
            }
            else return false;
        }
        else return true;
    }









    public override void setActor( GameObject actor )
    {
        this.actor = actor;
    }

    public void setWantedNeed(Bedurfniss need) {
        this.wantedNeed = need;
    }

    // Use this for initialization
    void Start () {
        name = "conquerNeedStation";

    }
	

}
