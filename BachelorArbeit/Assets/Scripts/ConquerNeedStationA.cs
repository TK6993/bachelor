using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConquerNeedStationA : KIAction {

    Bedurfniss wantedNeed;
    GameObject needStationToConquer;
    //KIFaction faction;

    public override bool doAction( GameObject actor )
    {
        // Dieese Aktion wird von einer Fraktion an einen agent weiter gegeben deswegen hier die Unterscheidung , denn Fraktion und Agnet gehen unterschiedlich mit der Aktion um
        this.actor = actor;// Kann sowohl eine Fraktion als auch ein Agnet sein.
        KIFaction faction = actor.GetComponent<KIFaction>();
        if ( faction != null )
        {
            needStationToConquer = faction.worldneedManager.getNearestPointofSatisfaction( wantedNeed, gameObject );
            actor.GetComponent<KIFaction>().taskForAgents = this;
        }
        else {
            KIAgent agent = actor.GetComponent<KIAgent>();
            agentConqerNeedStationAgent( agent);



        }
        return true;
    }

    private void agentConqerNeedStationAgent( KIAgent agent) {

            KIFaction faction = agent.faction;
        if ( needStationToConquer )
        {
            NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
            navAgent.SetDestination( needStationToConquer.transform.position );

            //agents an listofagentneed stations anpassen
            if ( !needStationToConquer.GetComponent<NeedStation>().isInPossession )
            {
            faction.listOfAgentNeedStations[ wantedNeed.name ].Add( needStationToConquer );
                needStationToConquer.GetComponent<NeedStation>().isInPossession = true;
                needStationToConquer.GetComponent<NeedStation>().ownerFaction = faction.gameObject;
                needStationToConquer.transform.GetChild( 0 ).gameObject.GetComponent<SpriteRenderer>().color = faction.factionColor;
            }
        }
        faction.taskForAgents = null;


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
