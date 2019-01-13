using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Bedurfniss {


	// Use this for initialization
	void Start () {
        name = "hunger";
        needWithNeedStation = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool satisfy( GameObject actor)
    {
       KIAgent agent = actor.GetComponent<KIAgent>();
        if ( !agent.pay( "food" ) )return false;
        decreaseCurrentValue( 3000 );
        // base.tryToSatisfy();
        return true;
        


    }

    public override KIAction needHasNotBeenSatisfied(GameObject agent)
    {

        if ( currentvalue > MaxValue - 5 ) {
            KIAction action = agent.GetComponent<AgentDie>();
            return action;
            
           // needM.logoutAgentfromStation( agent );
            //Destroy( gameObject );
        }
        return null;
    }
}
