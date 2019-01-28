using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taxes : Bedurfniss
{

    public override void Start()
    {
        base.Start();
        name = "taxes";
    }


    public override KIAction needHasNotBeenSatisfied(){
       failedToSatisfy();
        return null;
    }

    public override KIAction satify() {
        decreaseCurrentValue( 50 );
        actionDefaultSatisfied();
        return null;
    }

    public override KIAction tryToSatisfy()
    {
        return gameObject.GetComponent<CollectTaxesA>();
    }

}
