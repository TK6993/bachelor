using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KIAgent : MonoBehaviour, IIndigent
{
    public NavMeshAgent navAgent;

    [SerializeField] private Bedurfniss[ ] bedürfnisse;
    [SerializeField] private bool isWorkingOnNeed;
    public KIFaction faction;
    [SerializeField] private GameObject waiter;
    [SerializeField] private GameObject waiterPrefab;
    [SerializeField] private Bedurfniss workingNeed;

    

    public Bedurfniss mostAskedNeed;
    [SerializeField] private int counterOfWaitingNeeds = 0;
    public KIAction[] agentActions;

    [SerializeField] private int money = 0;

    public Dictionary<string, KIAction> agentActionsbyName = new Dictionary<string, KIAction>();

    public bool waitingForFreeNeedPoint = false;

    public List<GameObject> currentCollisions = new List<GameObject>();
    public float satisfaction = 0;
    [SerializeField]private int salery;

    public int Salery
    {
        get
        {
            return salery;
        }

        set
        {
            salery = value;
        }
    }




    // Use this for initialization
    void Start()
    {
        foreach ( KIAction action in agentActions )
        {
            agentActionsbyName.Add( action.name, action );
        }
        
     }

    // Update is called once per frame
    void Update()
    {
        

        if ( waiter == null && !isWorkingOnNeed ) {

            increaseNeeds();
            evaluateNeeds();
            isWorkingOnNeed = processNeedWithoutStation();
           if(isWorkingOnNeed) isWorkingOnNeed = processNotReachabelNeed();
           // tryToSatisfyNeed( workingNeed );
            waiter = Instantiate( waiterPrefab );

        }


        else if ( waiter == null && isWorkingOnNeed ) {
            increaseNeeds();
            isWorkingOnNeed= !tryToSatisfyNeed(workingNeed);
            //isWorkingOnNeed = !tryToSatisfyNeed( workingNeed );
           // waiter = Instantiate( waiterPrefab );


        }

    }

    public void changeMoney( int amount) {
        money += amount;
        if ( money < 0 ) money = 0;
    }

    bool processNeedWithoutStation() {
        if ( !workingNeed.needWithNeedStation )
        {
            workingNeed.satisfy( gameObject );

            satisfaction -= 1f;
            workingNeed.changeAskedCounter( false );// true sorgt für eine Verringerung des AskCounters im Bedürfniss
            if ( satisfaction > 10 ) satisfaction = 10;
            else if ( satisfaction < -10 ) satisfaction = -10;

            counterOfWaitingNeeds = 0;// !!Beobachten wegen: wird auf 0 gestetzt in jedem fall bei needs ohne station
            if ( counterOfWaitingNeeds < 0 ) counterOfWaitingNeeds = 0;
            return false;
        }
        else return true;

    }

    bool processNotReachabelNeed() {

        if ( waitingForFreeNeedPoint ) // ist in jedem fall false wenn das workingneed keine NeedStation erfordert
        {
            failedToSatisfy();
            return false;
        }
        else return true; 

    }

    private void failedToSatisfy() {
        satisfaction += 0.5f;
        if ( satisfaction > 10 ) satisfaction = 10;
        else if ( satisfaction < -10 ) satisfaction = -10;
        workingNeed.changeAskedCounter( true );// true sorgt für eine Erhöhung des AskCounters im Bedürfniss


        counterOfWaitingNeeds++;
        if ( counterOfWaitingNeeds > bedürfnisse.Length - 1 ) { counterOfWaitingNeeds = 0; }

        KIAction action = workingNeed.needHasNotBeenSatisfied( gameObject );
        if ( action != null ) action.doAction( gameObject );

       waitingForFreeNeedPoint = false;

    }

    public bool tryToSatisfyNeed( Bedurfniss workingNeed ) {
        counterOfWaitingNeeds = 0;// !!Beobachten wegen: wird auf 0 gestetzt in jedem fall bei needs ohne station
        if ( counterOfWaitingNeeds < 0 ) counterOfWaitingNeeds = 0;
        foreach ( GameObject coll in currentCollisions )
        {
            if ( coll.tag == workingNeed.name )
            {

                NeedStation needS = coll.gameObject.GetComponent<NeedStation>();

                if ( workingNeed.satisfy( gameObject ) ) {
                    satisfaction -= 1f;
                    workingNeed.changeAskedCounter( false );// true sorgt für eine Verringerung des AskCounters im Bedürfniss
                    if ( satisfaction > 10 ) satisfaction = 10;
                    else if ( satisfaction < -10 ) satisfaction = -10;
                }
                else {
                    failedToSatisfy();
                }


                return true; 
                
            }
        }
        return false;

    }

    public  void evaluateNeeds()
    {

        //noch sehr einfach (ermittelt das höchste bedürfniss.)
       

            Array.Sort( bedürfnisse );
            Array.Reverse( bedürfnisse );
            //highestNeedValue = b.Currentvalue;
            workingNeed = bedürfnisse[ counterOfWaitingNeeds];
            mostAskedNeed = setMostRequestedNeed();


        if ( workingNeed != null )
            {
            if ( workingNeed.needWithNeedStation ) FindWayTosatisfactionPoint( workingNeed );
            else waitingForFreeNeedPoint = false;
                // Wenn kein freier Platz für die befriedigung des Bedürfnisses gefunden wurde ist "isworkingOnNeed" falsch und
                // needHasNotBeenSatisfied() wird aufgerufen sowie der  counterOfWaitingNeeds wird erhöht das als nächtes das am 2 meisten gewollte bedrüfniss befriedigt wird.
                //isWorkingOnNeed = !waitingForFreeNeedPoint;
   

            }
    }

    private void FindWayTosatisfactionPoint( Bedurfniss b )
    {
        Vector3 pointForSatisfaction = faction.getNearestPointofSatisfaction( b, gameObject).transform.position;
        navAgent.SetDestination( pointForSatisfaction );
    }


    public  void increaseNeeds() {
        foreach ( Bedurfniss need in bedürfnisse )
        {
            need.changeNeed();
        }


    }

    public bool pay( string v )
    {
        int payamount;
        switch ( v ) {
            case "food" :  payamount = faction.foodPrice ; break;
            default: payamount = 0; break;
        }
        if ( payamount > money )
        {
            return false;
        }
        else {
            money -= payamount;
            return true;
        }
    }

    Bedurfniss setMostRequestedNeed() {
        Bedurfniss tempNeed = null;
        int highestAskCounter = 0;
        foreach ( Bedurfniss need in bedürfnisse )      
        {
            if ( need.askForCounter > highestAskCounter ) {
                tempNeed = need;
                highestAskCounter = need.askForCounter;
            }
        }
        return tempNeed;
    }



    void OnTriggerEnter( Collider col )
    {
        if ( col.gameObject.layer == 8 )
        {
            NeedStation needS = col.GetComponent<NeedStation>();
            if ( needS.hasCapacity && !needS.agentsOnThisStation.Contains( gameObject ) ) return;
           // if(col.GetComponent<NeedStation>().resgisterOnStation( gameObject ))
           currentCollisions.Add( col.gameObject );

        }
    }
    void OnTriggerExit( Collider col )
    {
        // Agent gibt die Bedürfnissstation frei
        col.GetComponent<NeedStation>().removeFromStation(gameObject);
        currentCollisions.Remove( col.gameObject );
        /* if ( col.gameObject.layer == 8 ) {
              NeedStation needS = col.gameObject.GetComponent<NeedStation>();
              if ( needS.taken && needS.agentOnThisStation == gameObject) {
                      needS.taken = false;
                      needS.agentOnThisStation = null;
                  }
          }*/
    }

     

    


}
