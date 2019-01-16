using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConquerNeedStationA : KIAction {

    [SerializeField] Bedurfniss wantedNeed;
    [SerializeField] GameObject needStationToConquer;
    [SerializeField] GameObject responsibleAgent;
    //KIFaction faction;

    public override bool doAction( GameObject actor )
    {
        // Dieese Aktion wird von einer Fraktion an einen agent weiter gegeben deswegen hier die Unterscheidung , denn Fraktion und Agent gehen unterschiedlich mit der Aktion um
        this.actor = actor;// Kann sowohl eine Fraktion als auch ein Agent sein.
        KIFaction faction = actor.GetComponent<KIFaction>();
        if ( faction != null )
        {   
            if(responsibleAgent == null ) {
                needStationToConquer = faction.worldneedManager.getNearestPointofSatisfaction( wantedNeed, gameObject );
                actor.GetComponent<KIFaction>().taskForAgents = this;
            }
            if ( !satsifiedNeed ) return false;
            
        }
        else {
            KIAgent agent = actor.GetComponent<KIAgent>();
            return agentConqerNeedStationAgent( agent);



        }
        return true;
    }

    private bool agentConqerNeedStationAgent( KIAgent agent )
    {
        if ( needStationToConquer && (responsibleAgent == null || responsibleAgent == agent.gameObject) )
        {
            responsibleAgent = agent.gameObject;
            NeedStation needStation = needStationToConquer.GetComponent<NeedStation>();

         //   if ( !needStation.isInPossession ){
                    needStation.isInPossession = true;
                KIFaction faction = agent.faction;
                NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
                if ( faction.taskForAgents != null )
                {
                    satsifiedNeed = true;
                    faction.taskForAgents = null;
                    navAgent.SetDestination( needStationToConquer.transform.position );
                }
                if ( navAgent.remainingDistance <= 0.005f )
                {
                    faction.listOfAgentNeedStations[ wantedNeed.name ].Add( needStationToConquer );
                    needStationToConquer.GetComponent<NeedStation>().ownerFaction = faction.gameObject;
                    needStationToConquer.transform.GetChild( 0 ).gameObject.GetComponent<SpriteRenderer>().color = faction.factionColor;
                    needStationToConquer = null;
                   // wantedNeed = null;
                    responsibleAgent = null;

                    return true;
                }
                else return false;
                //agents an listofagentneed stations anpassen
            //}
            //else return true;

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
	
	// Update is called once per frame
	void Update () {
		
	}
}
