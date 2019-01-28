using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prosperity : Bedurfniss
{

    KIAgent agent; 

    public override KIAction needHasNotBeenSatisfied()
    {
        failedToSatisfy();
        return null;
    }

    public override KIAction satify()
    {
        agent.changeMoney( agent.Salery );
        decreaseCurrentValue( 30 );
        actionDefaultSatisfied();
        return null;
    }

    public override void Start()
    {
        base.Start();
        name = "prosperity";

    }

    public override KIAction tryToSatisfy()
    {
        agent = actor.GetComponent<KIAgent>();
        Work work = GetComponent<Work>();
        if ( work != null ) {
            NeedStation workplace = work.workplace;
            if ( work.workplace ) {
                GoToA goTo = gameObject.GetComponent<GoToA>();
                goTo.setDestinationObject( workplace.gameObject );
                return goTo;
            }

        }
        return GetComponent<EmptyActionA>();
    }

  


}
