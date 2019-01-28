using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freetime : Bedurfniss {



    public override KIAction needHasNotBeenSatisfied()
    {
        KIAgent agent = actor.GetComponent<KIAgent>();
       failedToSatisfy();
        return null;
    }

    public override KIAction satify()
    {
        KIAgent agent = actor.GetComponent<KIAgent>();
       actionDefaultSatisfied();
        //agent.counterOfWaitingNeeds = 0;
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
