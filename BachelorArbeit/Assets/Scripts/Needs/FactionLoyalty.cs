using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionLoyalty : Bedurfniss {



    public override KIAction needHasNotBeenSatisfied( GameObject agent )
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        name = "factionLoyalty";
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public override bool satisfy(GameObject actor)
    {
        KIFaction agentsFaction = actor.GetComponent<KIAgent>().faction;
        KIAction taskToDoForTheFaction = agentsFaction.taskForAgents;
        if ( taskToDoForTheFaction != null )
        {
            taskToDoForTheFaction.doAction( gameObject );
        }
        decreaseCurrentValue( 500 );
        return true;



    }
}
