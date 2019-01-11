using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work : Bedurfniss {

    // Use this for initialization
    void Start()
    {
        name = "work";
        needWithNeedStation = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool satisfy( GameObject actor )
    {
        decreaseCurrentValue( 600 );
        //base.tryToSatisfy();
        return true;



    }

    public override KIAction needHasNotBeenSatisfied(  GameObject Agent )
    {
        return null;
    }
}
