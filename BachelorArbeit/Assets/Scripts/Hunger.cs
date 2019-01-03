using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Bedurfniss {


	// Use this for initialization
	void Start () {
        name = "hunger";
        MinValue = -20;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool satisfy()
    {
        decreaseCurrentValue( 50 );
        // base.tryToSatisfy();
        return false;



    }

    public override bool needHasNotBeenSatisfied(NeedManager needM, GameObject agent)
    {
        if ( currentvalue > MaxValue - 5 ) {
            needM.logoutAgentfromStation( agent );
            Destroy( gameObject );
        }
        return false;
    }
}
