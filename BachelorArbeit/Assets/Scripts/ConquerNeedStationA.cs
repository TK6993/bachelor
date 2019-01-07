using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConquerNeedStationA : KIAction {

    string NeedStationKind;
    NeedStation needStation;
    NeedStation needStationToConqer;
    KIFaction faction;

    public override bool doAction( GameObject actor )
    {
        this.actor = actor;
        KIFaction faction = actor.GetComponent<KIFaction>();
        if ( faction != null )
        {
            fractionConqerNeedStation();


        }
        else {
            KIAgent agent = actor.GetComponent<KIAgent>();
            agentConqerNeedStationAgent( agent);



        }
        return true;
    }

    private void agentConqerNeedStationAgent( KIAgent agent) {
        NavMeshAgent navAgent = agent.gameObject.GetComponent<NavMeshAgent>();
        navAgent.SetDestination( needStationToConqer.gameObject.transform.position);
        while ( navAgent.remainingDistance > 5.0f ) {
            Debug.Log("conquering");

        }
        GameObject[] needStationsInFaction = faction.agentNeeds[ NeedStationKind ];
        Array.Resize( ref needStationsInFaction, needStationsInFaction.Length + 1 );
        needStationsInFaction[ needStationsInFaction.Length - 1 ] = needStationToConqer.gameObject;
        needStationToConqer.isInPossession = true;


    }

    private void fractionConqerNeedStation()
    {

        faction = actor.GetComponent<KIFaction>();

        GameObject[ ] stations = faction.getAllNeedStationsFromWorldNeedManagerOfKind( NeedStationKind );
        float closestMagnitude = float.MaxValue;
        GameObject closestStation = null;
        foreach ( GameObject needStation in stations )
        {
            NeedStation station = needStation.GetComponent<NeedStation>();
            if ( station.isInPossession ) continue;
            float magnitude = ( actor.transform.position - needStation.transform.position ).sqrMagnitude;
            if ( magnitude < closestMagnitude )
            {
                closestMagnitude = magnitude;
                closestStation = needStation;
            }

        }
        if ( closestStation != null ) needStationToConqer = closestStation.GetComponent<NeedStation>();
    }



    public override void setActor( GameObject actor )
    {
        this.actor = actor;
    }

    public void setNeedStationKind(string needStationKind) {
        this.NeedStationKind = needStationKind;
    }

    // Use this for initialization
    void Start () {
        name = "conquerNeedStation";

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
