using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NeedManager : MonoBehaviour {

    public GameObject[ ] hungerPoints;
    public GameObject[ ] workPoints;
    public GameObject[ ] freetTimePoints;
    public GameObject[ ] techPoints;
    public Dictionary<string, GameObject[]> agentNeeds = new Dictionary<string,GameObject[]>();

    // Use this for initialization
   public  virtual void Start () {
        agentNeeds.Add("hunger",hungerPoints);
        agentNeeds.Add( "work", workPoints );
        agentNeeds.Add( "freetime", freetTimePoints);
        agentNeeds.Add( "technologie", techPoints );



    }


    public GameObject[ ] arrayResize( GameObject[ ] arrayToResize, int resizeAmount )
    {
            GameObject[ ] temp = new GameObject[ arrayToResize.Length + resizeAmount ];
        if ( resizeAmount >= 0 )
        {
            arrayToResize.CopyTo( temp, 0 );
            return temp;
        }
        else {
            for (int g=0; g < arrayToResize.Length; g++ ) {
                if ( arrayToResize[ g ] != null )
                {
                    temp[ g ] = arrayToResize[ g ];
                }
            }
            return temp;
        }
    }



    public virtual GameObject getNearestPointofSatisfaction( Bedurfniss b, GameObject actor )
    {
         //Suche nach nächten freien Punkt zur Erfüllung des Bedürfnisses
         GameObject nearestPlace = null;
       
    
         GameObject[ ] needSatisfactionPlaces = agentNeeds[b.name]; // Erstellen eines Arrays das wir uns aus dem Dictionry needs holen duch den na,en des bedürfnisses als Key 

         NavMeshAgent navAgent = actor.GetComponent<NavMeshAgent>();       
         float pathlength = float.MaxValue;
         float newPathLenght = 0;
         NeedStation needS = null;

        foreach ( GameObject p in needSatisfactionPlaces ) { // Gehen über alle für das Bedürfniss relevanten Stellen

            // Herausfinden ob die Stelle schon besetzt ist
            needS = p.GetComponent<NeedStation>();

            if ( needS.ownerFaction != null )// überprüfen ob diese Station bereits der Fraction gehört
            {
                if ( needS.ownerFaction.GetComponent<KIFaction>() == actor.GetComponent<KIFaction>() )
                {
                    continue;
                }
            }

           NavMeshPath path = new NavMeshPath();
           navAgent.CalculatePath( p.transform.position, path ); // Pfand zum Ziel berechnen
          Vector3[] corners = path.corners;

                // Länge des Pfades berechnen
                for ( int c = 0; c < corners.Length; c++ ){
                    if(c != corners.Length-1)
                    newPathLenght += Vector3.Distance( corners[ c ], corners[ c + 1 ] );
                }
           //     newPathLenght = CalculatePathCost(path);

                if( needS.ownerFaction != null ) newPathLenght = newPathLenght; // erhöht die kosten um eine Station einzunehmen.
                if ( newPathLenght < pathlength ) {
                    pathlength = newPathLenght;
                    nearestPlace = p;
                }
                newPathLenght = 0;
        }

        return nearestPlace;


    }


    float CalculatePathCost( NavMeshPath path )
    {
        if ( path.corners.Length < 2 ) return 0;

        float cost = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition( path.corners[ 0 ], out hit, 0.1f, NavMesh.AllAreas );
        Vector3 rayStart = path.corners[ 0 ];
        int mask = hit.mask;
        int index = IndexFromMask( mask );

        for ( int i = 1; i < path.corners.Length; ++i )
        {

            while ( true )
            {
                NavMesh.Raycast( rayStart, path.corners[ i ], out hit, mask );

                cost += NavMesh.GetAreaCost( index ) * hit.distance;

                if ( hit.mask != 0 ) mask = hit.mask;

                index = IndexFromMask( mask );
                rayStart = hit.position;

                if ( hit.mask == 0 )
                { //hit boundary; move startPoint of ray a bit closer to endpoint
                    rayStart += ( path.corners[ i ] - rayStart ).normalized * 0.01f;
                }

                if ( !hit.hit ) break;
            }
        }

        return cost;
    }

    int IndexFromMask( int mask )
    {
        for ( int i = 0; i < 32; ++i )
        {
            if ( ( 1 << i ) == mask )
                return i;
        }
        return -1;
    }


    public virtual void logoutAgentfromStation( GameObject agent ) {

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
