using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryFaction : Bedurfniss
{
    public override KIAction needHasNotBeenSatisfied()
    {
        failedToSatisfy();
        return null;
    }

    public override KIAction satify()
    {
        decreaseCurrentValue( 30 );
        actionDefaultSatisfied();
        return null;
    }

    public override KIAction tryToSatisfy()
    {
        if ( currentvalue > 10)
            return GetComponent<PlaceTowerA>();
        else return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        name = "militaer";
    }

}
