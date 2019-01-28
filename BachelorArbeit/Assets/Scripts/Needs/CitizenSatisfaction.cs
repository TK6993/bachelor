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
        
            if ( mostWantedNeed.needWithNeedStation )
            {
                UpgradeBuildingA upgradeAction = actor.GetComponent<UpgradeBuildingA>();
                if ( upgradeAction.hasEnoughtMoneyForUpgrade( mostWantedNeed ) ) return upgradeAction;



            
                    ConquerNeedStationA action = (ConquerNeedStationA) actor.GetComponent<ConquerNeedStationA>();

                    action.setWantedNeed( mostWantedNeed );
                    return action;
            }
            return null;

            // needM.logoutAgentfromStation( agent );
            //Destroy( gameObject );
        
       
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        name = "citzenSatisfaction";
    }



    public override KIAction tryToSatisfy()
    {
        if ( mostWantedNeed.needWithNeedStation )
        {
            if ( currentvalue < 10 || !isConquerNecessary( mostWantedNeed, faction ) )
            {
                mostWantedNeed = null;
                return GetComponent<PlaceTowerA>();
            }
            else{
                    UpgradeBuildingA upgradeAction = actor.GetComponent<UpgradeBuildingA>();
                    if ( upgradeAction.hasEnoughtMoneyForUpgrade( mostWantedNeed ) ) return upgradeAction;
                    ConquerNeedStationA conquerAction = (ConquerNeedStationA) actor.GetComponent<ConquerNeedStationA>();
                    conquerAction.setWantedNeed( mostWantedNeed );
                    return conquerAction;  
                }
        }
        return null;

    }

    public override KIAction satify()
    {
        actionDefaultSatisfied();
        return null;
    }
    public override void changeNeed()
    {
        base.changeNeed();

        faction = actor.GetComponent<KIFaction>();
        Dictionary<string, int> needsToSatisfy = new Dictionary<string, int>();
        float tempValue = 0.0f;
        int tempHighestValue = 0;
        Bedurfniss mostCitizenWantedNeed = null;
        faction.updateAgentMemberList( null );
        foreach ( KIAgent agent in faction.agentMembers )
        {
            //Zählt die Zufriedenheit aller Fraktionsmitglieder zusammen.
            Bedurfniss mostAskedNeed = agent.mostAskedNeed;
            tempValue += agent.satisfaction;

            //Ermittlen des am meisten gewünsten Bedürfniss der Fraktionsmitglieder
            if ( mostAskedNeed != null && !needsToSatisfy.ContainsKey( mostAskedNeed.name ) ) needsToSatisfy.Add( mostAskedNeed.name, 1 );
            else if ( mostAskedNeed != null && needsToSatisfy.ContainsKey( mostAskedNeed.name ) ) needsToSatisfy[ mostAskedNeed.name ]++;
            if ( mostAskedNeed != null )
            {
                if ( needsToSatisfy[ mostAskedNeed.name ] >= tempHighestValue )
                {
                    tempHighestValue = needsToSatisfy[ mostAskedNeed.name ];
                    mostCitizenWantedNeed = mostAskedNeed;
                }
            }

        }
        currentvalue = (int) ( tempValue / faction.agentMembers.Length );
        faction.mostWantedNeed = mostCitizenWantedNeed;
        mostWantedNeed = mostCitizenWantedNeed;
    }


    public bool isConquerNecessary( Bedurfniss need, KIFaction faction )
    {
        if ( !faction.listOfAgentNeedStations.ContainsKey( need.name ) ) Debug.Log( need.name );
        List<GameObject> stations = faction.listOfAgentNeedStations[ need.name ];
        int counterofWorkplaces = 0;
        foreach ( GameObject station in stations )
        {
            NeedStation needS = station.GetComponent<NeedStation>();
            counterofWorkplaces += needS.stationSize;
        }
        if ( counterofWorkplaces >= faction.agentMembers.Length ) return false;
        else return true;

    }

}
