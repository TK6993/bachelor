using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTowerA : KIAction
{

    public GameObject towerPrefab;
    public KIFaction faction;
    public List<GameObject> stationsWherePlacementisnotPossible;

    private GameObject stationToTryToPlaceATower;
    private bool failedToPlace = false;
    private int placeDir = 1;
    private TowerStation tower;
    public float wasBulildCounter = 0;


    public override bool doAction( GameObject actor )
    {
        if ( failedToPlace ) return resetAction();
        if ( stationToTryToPlaceATower == null )
        {
            faction = actor.GetComponent<KIFaction>();
            getFarthestStation();
            if ( stationToTryToPlaceATower == null ) return true;
            InstaciateTower( placeDir );
        }
        if ( satsifiedNeed ) {
            tower.setTowerActive();
            return resetAction();
        } 
        if ( wasBulildCounter >= 1 ) satsifiedNeed = true;
        wasBulildCounter++;
        return false;

    }

    private bool resetAction()
    {
        stationToTryToPlaceATower = null;
        placeDir = 1;
        tower = null;
        wasBulildCounter = 0;
        return true;
    }



    private void getFarthestStation() {
        float distanceFromFactionCore = 0;
        GameObject farthestStation = null;
        foreach ( List<GameObject> stationList in faction.listOfAgentNeedStations.Values )
        {
            foreach ( GameObject station in stationList )
            {
                if ( stationsWherePlacementisnotPossible.Contains( station ) ) continue;
                float temp = Vector3.Distance( faction.gameObject.transform.position, station.transform.position );
                if ( temp >= distanceFromFactionCore )
                {
                    distanceFromFactionCore = temp;
                    farthestStation = station;
                }

            }
        }
        stationToTryToPlaceATower = farthestStation;
    }


    public void findAnotherPlaceForTowerAtStation(GameObject i)
    {
        GameObject o = i;
        // die placedirection geht von 1 bis 4 eine zahl für jede Richtung von der Station aus;
        wasBulildCounter = 0;
        switch ( placeDir )
        {
        case 4:
            stationsWherePlacementisnotPossible.Add( stationToTryToPlaceATower );
            stationToTryToPlaceATower = null;
            getFarthestStation();
            if ( stationToTryToPlaceATower == null ) failedToPlace = true;
            placeDir = 1;
            InstaciateTower( placeDir );
            break;
        default:
            placeDir++;
            InstaciateTower( placeDir );
            if ( placeDir > 3 ) placeDir = 4;
            break;
   
        }


    }

    private void InstaciateTower(int placeDirection ) {

        float xModifier = 0;
        float zModifier = 0;

        if ( placeDirection < 3 )  zModifier = 2f + ( ( placeDirection / 2 ) * -4f );
        else {
            placeDirection = placeDirection / 2;
            xModifier  = 2f + ( ( placeDirection / 2 ) * -4f );
        }
       
        Vector3 positionToBePlaced = new Vector3( stationToTryToPlaceATower.transform.position.x+xModifier, stationToTryToPlaceATower.transform.position.y, stationToTryToPlaceATower.transform.position.z+zModifier );
        GameObject towerObject = Instantiate( towerPrefab,positionToBePlaced, towerPrefab.transform.rotation );
        towerObject.transform.GetChild( 0 ).gameObject.GetComponent<SpriteRenderer>().color = faction.factionColor;

        tower = towerObject.GetComponent<TowerStation>();
        tower.creatingAction = this;



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
