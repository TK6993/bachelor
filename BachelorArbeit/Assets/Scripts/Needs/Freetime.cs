using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freetime : Bedurfniss {



    public override KIAction needHasNotBeenSatisfied( GameObject agent )
    {
        throw new NotImplementedException();
    }


    public override bool satisfy()
    {
        decreaseCurrentValue( 0 );
        //base.tryToSatisfy();
        return false;



    }
    // Use this for initialization
    void Start () {
        name = "freetime";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
