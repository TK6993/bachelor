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
    [SerializeField]
    private KIAction currentAction;
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


   /* public bool actionDefaultSatisfied()
    {
     

        counterOfWaitingNeeds = 0;// !!Beobachten wegen: wird auf 0 gestetzt in jedem fall bei needs ohne station
        if ( counterOfWaitingNeeds < 0 ) counterOfWaitingNeeds = 0;


        return true;
    }

  



    public void failedToSatisfy()
    {
      

        counterOfWaitingNeeds++;
        if ( counterOfWaitingNeeds > bedüfnisse.Length - 1 ) { counterOfWaitingNeeds = 0; }

        // KIAction action = workingNeed.needHasNotBeenSatisfied( gameObject );
        // if ( action != null ) action.doAction( gameObject );

    

    }*/

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

    public void pauseGame( bool value )
    {
        gameIsPaused = value;
         GetComponent<NavMeshAgent>().isStopped = value;
    }
}
