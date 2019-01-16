using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work : Bedurfniss {

    KIAgent agent = null;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        name = "work";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override KIAction tryToSatisfy(  )
    {
        if(agent == null) actor.GetComponent<KIAgent>();
         agent = actor.GetComponent<KIAgent>();
        return actor.GetComponent<GoToA>(); ;
     



    }

    public override KIAction needHasNotBeenSatisfied( )
    {
        agent.failedToSatisfy();
        return null;
    }

    public override KIAction satify()
    {
        agent.changeMoney( agent.Salery );
        decreaseCurrentValue( 1 );
        agent.actionDefaultSatisfied();
        return null;
    }
}
