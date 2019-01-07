using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionLoyalty : Bedurfniss {

    KIFaction agentsFaction;


    public override KIAction needHasNotBeenSatisfied( GameObject agent )
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        name = "factionLoyalty";
        agentsFaction = gameObject.GetComponent<KIAgent>().faction;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public override bool satisfy()
    {
        KIAction taskToDoForTheFaction = agentsFaction.taskForAgents;
        taskToDoForTheFaction.doAction( gameObject );
        decreaseCurrentValue( 50 );
        // base.tryToSatisfy();
        return false;



    }
}
