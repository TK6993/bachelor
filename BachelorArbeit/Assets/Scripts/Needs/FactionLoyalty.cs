using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionLoyalty : Bedurfniss {

    // Use this for initialization
    public override void Start(){
        base.Start();
        name = "factionLoyalty";
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public override KIAction needHasNotBeenSatisfied()
    {
        KIAgent agent = actor.GetComponent<KIAgent>();
        agent.failedToSatisfy();
        return null;
    }


    public override KIAction satify()
    {
        KIAgent agent = actor.GetComponent<KIAgent>();
        agent.actionDefaultSatisfied();
        decreaseCurrentValue( 50 );

        return null;
    }

    public override KIAction tryToSatisfy()
    {
        KIFaction agentsFaction = actor.GetComponent<KIAgent>().faction;
        KIAction taskToDoForTheFaction = agentsFaction.taskForAgents;
        if ( taskToDoForTheFaction != null )
        {
            return  taskToDoForTheFaction;
        }
        return null;


    }
}
