using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Bedurfniss {


	// Use this for initialization
	void Start () {
        name = "hunger";
        MinValue = -20;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool satisfy()
    {
        decreaseCurrentValue( 50 );
        // base.tryToSatisfy();
        return false;
        


    }

    public override KIAction needHasNotBeenSatisfied(NeedManager needM, GameObject agent)
    {

        if ( currentvalue > MaxValue - 5 ) {
            KIAction action = agent.GetComponent<KIAgent>().agentActionsbyName[ "agentDie" ];
            return action;
            
           // needM.logoutAgentfromStation( agent );
            //Destroy( gameObject );
        }
        return null;
    }
}
