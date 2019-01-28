using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologieFaction : Bedurfniss

{
    public override KIAction needHasNotBeenSatisfied()
    {
        decreaseCurrentValue( 50 );

        failedToSatisfy();
        return null;
    }

    public override KIAction satify()
    {
        decreaseCurrentValue( 50 );
       actionDefaultSatisfied();
        return null;

    }

    public override KIAction tryToSatisfy()
    {
        UpgradeBuildingA upgradeAction = actor.GetComponent<UpgradeBuildingA>();
        if ( upgradeAction.hasEnoughtMoneyForUpgrade( this ) )
            return upgradeAction;


        ConquerNeedStationA conquerAction = GetComponent<ConquerNeedStationA>();
        conquerAction.setWantedNeed( this );
        return conquerAction;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        name = "technologie";


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
