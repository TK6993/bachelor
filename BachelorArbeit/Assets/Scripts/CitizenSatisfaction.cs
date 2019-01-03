using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSatisfaction : Bedurfniss {

    [SerializeField]
    private KIFaction faction;


    // signatur müsste nochmal überarbeittet werden mit den Paramether , diese werden hier nicht gebraucht.
    public override bool needHasNotBeenSatisfied( NeedManager needM, GameObject agent )
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        faction = gameObject.GetComponent<KIFaction>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool satisfy()
    {
        float tempValue = 0.0f;
        foreach ( KIAgent agent in faction.agentMembers )
        {
            tempValue += agent.satisfaction;
        }
        currentvalue = (int) (tempValue / faction.agentMembers.Length);
        if ( currentvalue < 0 )
        {
            return true;
        }
        else {
            needHasNotBeenSatisfied(faction,null );
            return false;
        }

    }
}
