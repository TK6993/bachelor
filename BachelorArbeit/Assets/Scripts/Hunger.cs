using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Bedurfniss {

    GameObject waiterObject;

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
        decreaseCurrentValue( 40 );
        // base.tryToSatisfy();
        return false;



    }

    public override bool needHasNotBeenSatisfied()
    {
        if ( currentvalue > MaxValue - 5 ) {

            Destroy( gameObject );
        }
        return false;
    }
}
