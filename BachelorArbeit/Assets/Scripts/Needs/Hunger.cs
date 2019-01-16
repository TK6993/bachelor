using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Bedurfniss {

    KIAgent agent = null;


    public override void Start(){
        base.Start();
        name = "hunger";

    }

 
	
	// Update is called once per frame
	void Update () {
		
	}

    public override KIAction tryToSatisfy()
    {
       if(agent == null) agent = actor.GetComponent<KIAgent>();
        if ( !agent.pay( "food" ) )return actor.GetComponent<EmptyActionA>();
       
        return actor.GetComponent<GoToA>(); 
        


    }

    public override KIAction needHasNotBeenSatisfied()
    {

        if ( currentvalue > MaxValue - 5 ) {
            KIAction action = actor.GetComponent<AgentDie>();
            return action;
            
           // needM.logoutAgentfromStation( agent );
            //Destroy( gameObject );
        }
        agent.failedToSatisfy();
        return null;
    }

    public override KIAction satify()
    {
        decreaseCurrentValue( 5 );
        agent.actionDefaultSatisfied();
        return null;
    }
}
