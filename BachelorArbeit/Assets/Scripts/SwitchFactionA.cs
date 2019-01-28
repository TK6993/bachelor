using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFactionA : KIAction
{
    private int patienceCounter;
    private int patienceCounterLimit = 10;

    public override bool doAction( GameObject actor )
    {
        /* patienceCounter++;
         if ( patienceCounter <= patienceCounterLimit ) return true;
         KIAgent agent = actor.GetComponent<KIAgent>();
         */
        return true;

    }

}
