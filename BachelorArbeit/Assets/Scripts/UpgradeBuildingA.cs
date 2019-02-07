using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuildingA : KIAction
{
    public override bool doAction( GameObject actor )
    {
        this.actor = actor;
        KIFaction faction = actor.GetComponent<KIFaction>();
        if ( faction.mostWantedNeed == null )
            return true;// nur kurzer fix
        string needName = faction.mostWantedNeed.name;
        List<GameObject> factionsNeedStationsOfSearchedNeed = faction.listOfAgentNeedStations[ needName ];
        NeedStation stationToUpgrade = null;
        satsifiedNeed = true;
        foreach ( GameObject station in factionsNeedStationsOfSearchedNeed )
        {
            NeedStation needS = station.GetComponent<NeedStation>();

            if ( needS.level >= station.GetComponentInChildren<LevelUpdater>().levels.Length - 1 )   continue;

            int payAmount = faction.pay(needS.UpgradeCosts);
            if ( payAmount < 0 ) {
                continue;
            }

            stationToUpgrade = needS;
        }
        if ( stationToUpgrade != null )
        {
            stationToUpgrade.stationSize += 2;
            stationToUpgrade.level++;
            stationToUpgrade.gameObject.GetComponentInChildren<LevelUpdater>().upgradeGraphic();
        }
        return true;

    }

    public bool hasEnoughtMoneyForUpgrade( Bedurfniss needToUpgrade ) {
        if ( needToUpgrade == null ) return false;
        KIFaction faction = gameObject.GetComponent<KIFaction>();
        if ( !faction.listOfAgentNeedStations.ContainsKey( needToUpgrade.name ) ) Debug.Log( needToUpgrade.name );
        List<GameObject> factionsNeedStationsOfSearchedNeed = faction.listOfAgentNeedStations[ needToUpgrade.name ];
        foreach ( GameObject station in factionsNeedStationsOfSearchedNeed ) {

                NeedStation needS = station.GetComponent<NeedStation>();
                if ( needS.UpgradeCosts > faction.money ) continue;
                return true;
            }
        return false;

        }
}
