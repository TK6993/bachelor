using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSatisfaction : Bedurfniss {

    [SerializeField]
    private KIFaction faction;
    [SerializeField]
    private Bedurfniss mostWantedNeed;



    public override KIAction needHasNotBeenSatisfied( )
    {
        if ( currentvalue > 0 )
        {
            ConquerNeedStationA action = (ConquerNeedStationA) actor.GetComponent<ConquerNeedStationA>();
            
            action.setWantedNeed( mostWantedNeed);
            return action;

            // needM.logoutAgentfromStation( agent );
            //Destroy( gameObject );
        }
        return null;
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        name = "citzenSatisfaction";
    }

    // Update is called once per frame
    void Update () {
		
	}

    public override KIAction tryToSatisfy()
    {
        KIFaction faction = actor.GetComponent<KIFaction>();
        Dictionary<Bedurfniss, int> needsToSatisfy = new Dictionary<Bedurfniss,int>();
        float tempValue = 0.0f;
        int tempHighestValue = 0;
        Bedurfniss mostCitizenWantedNeed= null;
        foreach ( KIAgent agent in faction.agentMembers )
        {
            //Zählt die Zufriedenheit aller Fraktionsmitglieder zusammen.
            Bedurfniss mostAskedNeed = agent.mostAskedNeed;
            tempValue += agent.satisfaction;

            //Ermittlen des am meisten gewünsten Bedürfniss der Fraktionsmitglieder
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
            mostWantedNeed = null;
            return null;
        }
        
       mostWantedNeed = mostCitizenWantedNeed;
       return GetComponent<EmptyActionA>();
        

    }

    public override KIAction satify()
    {
        return null;
    }
}
