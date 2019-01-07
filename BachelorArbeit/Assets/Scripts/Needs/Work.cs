using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work : Bedurfniss {

    // Use this for initialization
    void Start()
    {
        name = "work";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool satisfy()
    {
        decreaseCurrentValue( 25 );
        //base.tryToSatisfy();
        return false;



    }

    public override KIAction needHasNotBeenSatisfied(  GameObject Agent )
    {
        throw new NotImplementedException();
    }
}
