using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSatisfaction : Bedurfniss {

    [SerializeField]
    private KIFaction faction;
    [SerializeField]
    private Bedurfniss mostWantedNeed;



    // signatur müsste nochmal überarbeittet werden mit den Paramether , diese werden hier nicht gebraucht.
    public override KIAction needHasNotBeenSatisfied(  GameObject agent )
    {
        if ( currentvalue > MaxValue - 5 )
        {
            ConquerNeedStationA action  = (ConquerNeedStationA) agent.GetComponent<KIAgent>().agentActionsbyName[ "conquerNeedStation" ];
            action.setNeedStationKind( mostWantedNeed.name );
            return action;

            // needM.logoutAgentfromStation( agent );
            //Destroy( gameObject );
        }
        return null;
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
        Dictionary<Bedurfniss, int> needsToSatisfy = new Dictionary<Bedurfniss,int>();
        float tempValue = 0.0f;
        int tempHighestValue = 0;
        Bedurfniss mostCitizenWantedNeed= null;
        foreach ( KIAgent agent in faction.agentMembers )
        {
            Bedurfniss mostAskedNeed = agent.mostAskedNeed;
            tempValue += agent.satisfaction;
            if ( mostAskedNeed != null && !needsToSatisfy.ContainsKey( mostAskedNeed ) ) needsToSatisfy.Add( mostAskedNeed, 1 );
            else if ( mostAskedNeed != null && needsToSatisfy.ContainsKey( mostAskedNeed ) ) needsToSatisfy[ mostAskedNeed ]++;
            if ( mostAskedNeed != null )
            {
                if ( needsToSatisfy[ mostAskedNeed ] > tempHighestValue )
                {
                    tempHighestValue = needsToSatisfy[ mostAskedNeed ];
                    mostCitizenWantedNeed = mostAskedNeed;
                }
            }

        }
        currentvalue = (int) (tempValue / faction.agentMembers.Length);
        if ( currentvalue < 0 )
        {
            return true;
        }
        else {
            mostWantedNeed = mostCitizenWantedNeed;
            return false;
        }

    }
}
