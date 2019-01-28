using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work : Bedurfniss {

    KIAgent agent = null;
   [SerializeField] public NeedStation workplace = null;



    // Use this for initialization
    public override void Start()
    {
        base.Start();
        name = "work";
    }



    public override KIAction tryToSatisfy(  )
    {
        if(agent == null) actor.GetComponent<KIAgent>();
         agent = actor.GetComponent<KIAgent>();
        return actor.GetComponent<GoToA>(); ;
     



    }

    public override KIAction needHasNotBeenSatisfied( )
    {
        failedToSatisfy();
        increaseCurrentValue( 60 );
        return null;
    }

    public override KIAction satify()
    {
       // agent.changeMoney( agent.Salery );
        actionDefaultSatisfied();
       decreaseCurrentValue( 60 );
        return null;
    }

    public void setworkplace( WorkPlace workP ) {
        this.workplace = workP;
    }
}
