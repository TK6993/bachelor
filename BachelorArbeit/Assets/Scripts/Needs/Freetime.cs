using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freetime : Bedurfniss {



    public override KIAction needHasNotBeenSatisfied()
    {
        KIAgent agent = actor.GetComponent<KIAgent>();
        agent.failedToSatisfy();
        return null;
    }

    public override KIAction satify()
    {
        KIAgent agent = actor.GetComponent<KIAgent>();
        decreaseCurrentValue( 25 );
        agent.actionDefaultSatisfied();
        return null;
    }

    public override KIAction tryToSatisfy()
    {
        return actor.GetComponent<GoToA>();
    }

   
    // Use this for initialization
    void Start () {
        base.Start();
        name = "freetime";
	}
	

}
