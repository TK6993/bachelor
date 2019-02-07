using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConquerNeedStationA : KIAction {

    [SerializeField] Bedurfniss wantedNeed;
    [SerializeField] GameObject needStationToConquer;
    [SerializeField] KIAgent responsibleAgent;
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
            if ( wantedNeed == null )// Kommt zum tragen wenn die station eingenommen wurde und damit die Aktion abgeschlossen wird.
                    return true;
            if(!publishedTask && !satsifiedNeed) {// Erstmaliges auführen der doAction Methode um einen Conquer Auftrag für Agents bereit zu stellen.
                needStationToConquer = faction.worldneedManager.getNearestPointofSatisfaction( wantedNeed, gameObject );
                if ( needStationToConquer == null ) { // wenn es keine need station mehr auf der map gibt
                    return resetAction();
                   }
                //needStationToConquer.GetComponent<NeedStation>().isInPossession = true;

                actor.GetComponent<KIFaction>().taskForAgents = this;
                publishedTask = true;
                return false;
            }
           else {
                if ( responsibleAgent == null && agentsFaction != null )
                    return resetAction();// Falls der Agent auf dem Weg stirbt lässt hier auch die Faction von der Action ab.
                return false;
            }
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
            responsibleAgent = agent;
            agentsFaction = responsibleAgent.faction;
            agentsFaction.taskForAgents = null;
            needStation = needStationToConquer.GetComponent<NeedStation>();
            navAgent = responsibleAgent.gameObject.GetComponent<NavMeshAgent>();
            navAgent.SetDestination( needStationToConquer.transform.position );
            return false;
        }
       else if ( responsibleAgent == agent ) {

            if ( navAgent.remainingDistance <= 0.005f ) {
                satsifiedNeed = true;
                //NeedStation needS = needStationToConquer.GetComponent<NeedStation>();
                agentsFaction.listOfAgentNeedStations[ wantedNeed.name ].Add( needStationToConquer );
                if ( needStation.ownerFaction != null )// Falls die Station besetztist wird sie hier von der alten Fraktion befreit und die Alte Fraktion merkt das es eine umkämpfe Station ist
                {
                    needStation.ownerFaction.GetComponent<MilitaryFaction>().increaseCurrentValue( 30 );
                    needStation.ownerFaction.GetComponent<KIFaction>().increaseEndangeredStation( needStation );
                    needStation.loseOwnerFaction( needStation.ownerFaction );
                    
                }


                needStation.ownerFaction = agentsFaction.gameObject;
                needStationToConquer.transform.GetChild( 0 ).gameObject.GetComponent<SpriteRenderer>().color = agentsFaction.factionColor;
                return resetAction();;
            }
            else return false;
        }
        else return true;
    }


    private bool resetAction() {

        needStationToConquer = null;
        wantedNeed = null;
        responsibleAgent = null;
        agentsFaction = null;
        needStation = null;
        navAgent = null;
        publishedTask = false;

        return true;
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
