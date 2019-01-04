﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NeedManager : MonoBehaviour {

    public GameObject[ ] hungerPoints;
    public GameObject[ ] workPoints;
    public GameObject[ ] freetTimePoints;
    public Dictionary<string, GameObject[]> agentNeeds = new Dictionary<string,GameObject[]>();

    // Use this for initialization
   public  virtual void Start () {
        agentNeeds.Add("hunger",hungerPoints);
        agentNeeds.Add( "work", workPoints );
        agentNeeds.Add( "freetime", freetTimePoints);


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual Vector3 getNearestPointofSatisfaction( Bedurfniss b, GameObject agent )
    {
         //Suche nach nächten freien Punkt zur Erfüllung des Bedürfnisses
         GameObject nearestPlace = null;
    
         GameObject[ ] needSatisfactionPlaces = agentNeeds[b.name]; // Erstellen eines Arrays das wir uns aus dem Dictionry needs holen duch den na,en des bedürfnisses als Key 

         NavMeshAgent navAgent = agent.GetComponent<KIAgent>().navAgent;       
         float pathlength = float.MaxValue;
         float newPathLenght = 0;
         NeedStation needS = null;

         foreach ( GameObject p in needSatisfactionPlaces ) { // Gehen über alle für das Bedürfniss relevanten Stellen

                // Herausfinden ob die Stelle schon besetzt ist
                needS = p.GetComponent<NeedStation>();
                if ( needS.isItFull()) {
                    continue;
                }

                NavMeshPath path = new NavMeshPath();
                navAgent.CalculatePath( p.transform.position, path ); // Pfand zum Ziel berechnen
                Vector3[] corners = path.corners;

                // Länge des Pfades berechnen
                for ( int c = 0; c < corners.Length; c++ ){
                    if(c != corners.Length-1)
                    newPathLenght += Vector3.Distance( corners[ c ], corners[ c + 1 ] );
                }
                if ( newPathLenght < pathlength ) {
                    pathlength = newPathLenght;
                    nearestPlace = p;
                }
                newPathLenght = 0;
        }

        
        if ( nearestPlace == null ){ // Warten wenn kein freier Punkt gefunden wurde 
            agent.GetComponent<KIAgent>().waitingForFreeNeedPoint = true;
            return freetTimePoints[0].transform.position;
        }

        agent.GetComponent<KIAgent>().waitingForFreeNeedPoint = false;
        needS = nearestPlace.GetComponent<NeedStation>(); // Variable wird hier neu verwendet
        // Wenn die Stelle nur für begrenzte ist wird Sie hier beesetzt.
         needS.resgisterOnStation( agent );
       

        return nearestPlace.transform.position;


    }

    public void logoutAgentfromStation( GameObject agent ) {

        GameObject[ ][] values = new GameObject[ agentNeeds.Count ][];
        agentNeeds.Values.CopyTo( values, 0 );



        for ( int needList = 0; needList < values.Length; needList++ )
        {
            
            foreach ( GameObject satisfactionPoint in values[needList]) {
                NeedStation needS = satisfactionPoint.GetComponent<NeedStation>();
                needS.removeFromStation( agent );

            }

        }

    }
}
