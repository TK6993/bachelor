using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freetime : Bedurfniss {



    public override KIAction needHasNotBeenSatisfied( GameObject agent )
    {
        return null;
    }


    public override bool satisfy(GameObject actor)
    {
        decreaseCurrentValue( 25 );
        //base.tryToSatisfy();
        return true;



    }
    // Use this for initialization
    void Start () {
        name = "freetime";
        needWithNeedStation = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
