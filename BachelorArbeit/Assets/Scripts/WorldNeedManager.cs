using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNeedManager : NeedManager {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public override Vector3 getNearestPointofSatisfaction( Bedurfniss b, GameObject agent )
    {
        GameObject nearestPlace = null;

        GameObject[ ] needSatisfactionPlaces = agentNeeds[ b.name ]; // Erstellen eines Arrays das wir uns aus dem Dictionry needs holen duch den na,en des bedürfnisses als Key 

        float pathlength = float.MaxValue;
        float newPathLenght = 0;
        NeedStation needS = null;

        foreach ( GameObject p in needSatisfactionPlaces )
        { // Gehen über alle für das Bedürfniss relevanten Stellen

            // Herausfinden ob die Stelle schon besetzt ist
            needS = p.GetComponent<NeedStation>();
            if ( needS.isInPossession)
            {
                continue;
            }

            newPathLenght = (agent.transform.position - needS.transform.position).magnitude;
            
            // Länge des Pfades berechnen
          
            if ( newPathLenght < pathlength )
            {
                pathlength = newPathLenght;
                nearestPlace = p;
            }
            newPathLenght = 0;
        }

        return nearestPlace.transform.position;
    }
}
