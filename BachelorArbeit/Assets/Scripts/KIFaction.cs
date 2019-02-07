using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KIFaction : NeedManager,IIndigent {

    public string fractionName;
    public KIAgent[ ] agentMembers;
    public Bedurfniss[ ] bedüfnisse;
    public NeedManager worldneedManager;
    public KIAction taskForAgents;
    public Bedurfniss mostWantedNeed;
    public Color factionColor;
    public int foodPrice;
    public float taxesPercent = 10.0f;


    [SerializeField] private Bedurfniss workingNeed;
    [SerializeField] private bool isWorkingOnNeed;
    [SerializeField] private int counterOfWaitingNeeds = 0;
    [SerializeField] private GameObject waiterPrefab;
    [SerializeField] public int money;
    [SerializeField] int techlevel;

    [HideInInspector] public Dictionary<string, List<GameObject>> listOfAgentNeedStations = new Dictionary<string, List<GameObject>>();


    private GameObject waiter;
    [SerializeField] private KIAction currentAction;
    public Dictionary<NeedStation, int> endangeredNeedStations = new Dictionary<NeedStation, int>();
    private bool gameIsPaused;

    public int Techlevel
    {
        get
        {
            return calculateTechLevel();
        }

        set
        {
            techlevel = value;
        }
    }

    public bool GameIsPaused
    {
        get
        {
            return gameIsPaused;
        }

        set
        {
            gameIsPaused = value;
        }
    }

    private int calculateTechLevel()
    {
      List<GameObject> techStations = listOfAgentNeedStations["technologie"];
        int temp = 0;
        foreach ( GameObject station in techStations )
        {
            NeedStation needS = station.GetComponent<NeedStation>();
            temp += needS.level;
        }
        return temp;
    }

    public int pay( int payamount )
    {   
        if ( payamount > money )
        {
           // hier folgen wegen zuwenig Geld einbinden.
            return -1;
        }
        else
        {
            money -= payamount;
            return payamount;
        }
    }
    // Use this for initialization
    public override void Start () {
        base.Start();
        
       foreach (  string s in agentNeeds.Keys )
        {
            listOfAgentNeedStations.Add(s,new List<GameObject>());
        }
        listOfAgentNeedStations[ "freetime" ].Add( agentNeeds[ "freetime" ][ 0 ] );
    }

    public GameObject[] getAllNeedStationsFromWorldNeedManagerOfKind( string needStationKind )
    {
        return worldneedManager.agentNeeds[ needStationKind ];
    }

    public void increaseMoney( int paiedTaxesAmount )
    {
        money += paiedTaxesAmount;
    }

    // Update is called once per frame
    void Update ()  {

        if ( gameIsPaused ) return;

        if ( waiter == null && currentAction == null )
        {

            increaseNeeds();
            evaluateNeeds();
            currentAction = workingNeed.tryToSatisfy();
            if ( currentAction == null )
            {
                currentAction = workingNeed.satify();
                // if ( currentAction != null ) currentAction.doAction( gameObject );
                //isWorkingOnNeed = false;
            }
            // else isWorkingOnNeed = true;
            waiter = Instantiate( waiterPrefab );

        }


        else if ( waiter == null && currentAction != null )
        {
            increaseNeeds();
            isWorkingOnNeed = !currentAction.doAction( gameObject );
            if ( !isWorkingOnNeed )
            {
                if ( currentAction.satsifiedNeed )
                {
                    currentAction.satsifiedNeed = false;
                    currentAction = workingNeed.satify();
                }
                else currentAction = workingNeed.needHasNotBeenSatisfied();
                // if ( currentAction != null ) cur.doAction( gameObject );
                //currentAction = null;
            }
            waiter = Instantiate( waiterPrefab );

        }

    }

    public  void evaluateNeeds()
    {
        Array.Sort( bedüfnisse );
        Array.Reverse( bedüfnisse );
        workingNeed = bedüfnisse[ counterOfWaitingNeeds ];
        //if ( workingNeed != null ) isWorkingOnNeed = true;
    }


    public  void increaseNeeds()
    {
        foreach ( Bedurfniss need in bedüfnisse )
        {
            need.changeNeed();
        }
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
            /*
            Vector3[ ] corners = path.corners;
            

            // Länge des Pfades berechnen
            for ( int c = 0; c < corners.Length; c++ )
            {
                if ( c != corners.Length - 1 )
                    newPathLenght += Vector3.Distance( corners[ c ], corners[ c + 1 ] );
            }
        }*/

            newPathLenght = CalculatePathCost( path );

            if ( newPathLenght < pathlength )
            {
                pathlength = newPathLenght;
                if ( this.fractionName == "green"  && p.name == "Cube (17)") Debug.Log( pathlength );
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

    public void updateAgentMemberList(KIAgent agentToAdd)
    {
        KIAgent[ ] tempMemberList = gameObject.GetComponent<KIFaction>().agentMembers;
        List<KIAgent> templist = new List<KIAgent>();
        foreach ( KIAgent agent in tempMemberList )
        {
            if ( agent != null ) templist.Add( agent );
        }
        if ( agentToAdd != null ) templist.Add( agentToAdd );
        tempMemberList = templist.ToArray();
        agentMembers = tempMemberList;
    }

    public void changeWaitingCounter( bool changeDirection)
    {
        if ( changeDirection ) counterOfWaitingNeeds++;
        else counterOfWaitingNeeds = 0;
        if ( counterOfWaitingNeeds > bedüfnisse.Length - 1 ) { counterOfWaitingNeeds = 0; }
    }

    public void setWaitingForFreeNeedPoint( bool value )
    {
        //nothing TODO here;
    }

    public void increaseEndangeredStation(NeedStation needS ) {
        if ( endangeredNeedStations.ContainsKey( needS ) ) {
            endangeredNeedStations[ needS ]++;
        }
        else {
            endangeredNeedStations.Add( needS, 1 );
        }

    }

    public int getEndangeredCounter( NeedStation needS ) {

        if ( !endangeredNeedStations.ContainsKey( needS ) ) return 0;
        return endangeredNeedStations[ needS ];
    }



    public void pauseGame( bool value )
    {
        gameIsPaused = value;
         GetComponent<NavMeshAgent>().isStopped = value;
    }
}
