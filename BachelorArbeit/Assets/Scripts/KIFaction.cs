using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KIFaction : NeedManager,IIndigent {

    public string fractionName;
    public KIAgent[ ] agentMembers;
    public KIAction[ ] factionsActions;
    public Bedurfniss[ ] fractionNeeds;


    public NeedManager worldneedManager;

    [SerializeField] private Bedurfniss workingNeed;
    [SerializeField] private bool isWorkingOnNeed;
    [SerializeField] private int counterOfWaitingNeeds = 0;

    public Dictionary<string, List<GameObject>> listOfAgentNeedStations = new Dictionary<string, List<GameObject>>();
    public KIAction taskForAgents;

    public Bedurfniss mostWantedNeed;


    [SerializeField] private GameObject waiter;
    [SerializeField] private GameObject waiterPrefab;
    public Color factionColor;
    public int foodPrice;





    // Use this for initialization
    public override void Start () {
        base.Start();
        
       foreach (  string s in agentNeeds.Keys )
        {
            listOfAgentNeedStations.Add(s,new List<GameObject>());
        }

    }

    public GameObject[] getAllNeedStationsFromWorldNeedManagerOfKind( string needStationKind )
    {
        return worldneedManager.agentNeeds[ needStationKind ];
    }
	
	// Update is called once per frame
	void Update ()  {
        if ( waiter == null && !isWorkingOnNeed )
        {
            waiter = Instantiate( waiterPrefab );

            increaseNeeds();
            evaluateNeeds();

        }


        else if ( waiter == null && isWorkingOnNeed )
        {
           /* waiter = Instantiate( waiterPrefab );
            */
            //increaseNeeds();
            isWorkingOnNeed = !tryToSatisfyNeed( workingNeed );

        }

    }

    public  void evaluateNeeds()
    {

        Array.Sort( fractionNeeds );
        Array.Reverse( fractionNeeds );
        workingNeed = fractionNeeds[ counterOfWaitingNeeds ];
        if ( workingNeed != null ) isWorkingOnNeed = true;
    }

    public  void increaseNeeds()
    {
        foreach ( Bedurfniss need in fractionNeeds )
        {
            need.changeNeed();
        }
    }

    public  bool tryToSatisfyNeed( Bedurfniss workingNeed )
    {
        if ( !workingNeed.satisfy(gameObject) ) {
            KIAction factionAction = workingNeed.needHasNotBeenSatisfied( gameObject );
            if(factionAction != null) factionAction.doAction(gameObject);


        }
        return true;
        
    }

    public override GameObject getNearestPointofSatisfaction( Bedurfniss b, GameObject agent )
    {
        //Suche nach nächten freien Punkt zur Erfüllung des Bedürfnisses
        GameObject nearestPlace = null;

        GameObject[ ] needSatisfactionPlaces = listOfAgentNeedStations[ b.name ].ToArray(); // Erstellen eines Arrays das wir uns aus dem Dictionry needs holen duch den na,en des bedürfnisses als Key 

        NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
        float pathlength = float.MaxValue;
        float newPathLenght = 0;
        NeedStation needS = null;

        foreach ( GameObject p in needSatisfactionPlaces )
        { // Gehen über alle für das Bedürfniss relevanten Stellen

            // Herausfinden ob die Stelle schon besetzt ist
            needS = p.GetComponent<NeedStation>();
           
                if ( needS.isItFull() )
                {
                    continue;
                }
            

            NavMeshPath path = new NavMeshPath();
            navAgent.CalculatePath( p.transform.position, path ); // Pfand zum Ziel berechnen
            Vector3[ ] corners = path.corners;

            // Länge des Pfades berechnen
            for ( int c = 0; c < corners.Length; c++ )
            {
                if ( c != corners.Length - 1 )
                    newPathLenght += Vector3.Distance( corners[ c ], corners[ c + 1 ] );
            }
            if ( newPathLenght < pathlength )
            {
                pathlength = newPathLenght;
                nearestPlace = p;
            }
            newPathLenght = 0;
        }


        if ( nearestPlace == null ){ // Warten wenn kein  Punkt gefunden wurde 
            agent.GetComponent<KIAgent>().waitingForFreeNeedPoint = true;
            return freetTimePoints[ 0 ];
           // return agent;
        }

        else {
            agent.GetComponent<KIAgent>().waitingForFreeNeedPoint = false;
            needS = nearestPlace.GetComponent<NeedStation>(); // Variable wird hier neu verwendet
                                                              // Wenn die Stelle nur für begrenzte ist wird Sie hier beesetzt.
            needS.resgisterOnStation( agent );
            return nearestPlace;
        }


    }


    public override void logoutAgentfromStation( GameObject agent )
    {

        List<GameObject>[ ] values = new List<GameObject>[ listOfAgentNeedStations.Count];
        listOfAgentNeedStations.Values.CopyTo( values, 0 );



        for ( int needList = 0; needList < values.Length; needList++ )
        {

            foreach ( GameObject satisfactionPoint in values[ needList ] )
            {
                NeedStation needS = satisfactionPoint.GetComponent<NeedStation>();
                needS.removeFromStation( agent );

            }

        }
    }

    public void updateAgentMemberList()
    {
        KIAgent[ ] tempMemberList = gameObject.GetComponent<KIFaction>().agentMembers;
        List<KIAgent> templist = new List<KIAgent>();
        foreach ( KIAgent agent in tempMemberList )
        {
            if ( agent != null ) templist.Add( agent );
        }
        tempMemberList = templist.ToArray();
        agentMembers = tempMemberList;
    }
}
