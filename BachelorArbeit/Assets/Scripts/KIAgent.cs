using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KIAgent : MonoBehaviour, IIndigent
{
    public NavMeshAgent navAgent;

    [SerializeField] public Bedurfniss[ ] bedürfnisse;
    [SerializeField] private bool isWorkingOnNeed;
    public KIFaction faction;
    [SerializeField] private GameObject waiter;
    [SerializeField] private GameObject waiterPrefab;
      public Bedurfniss workingNeed;
    [SerializeField] KIAction currentAction;




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
        

        if ( waiter == null && currentAction==null ) {

            increaseNeeds();
            evaluateNeeds();
            currentAction = workingNeed.tryToSatisfy();
            if ( currentAction == null ) {
                    currentAction =  workingNeed.satify();
               // if ( currentAction != null ) currentAction.doAction( gameObject );
                //isWorkingOnNeed = false;
            }
           // else isWorkingOnNeed = true;
            waiter = Instantiate( waiterPrefab );

        }


        else if ( waiter == null && currentAction != null ) {
            increaseNeeds();
            isWorkingOnNeed = !currentAction.doAction( gameObject );
            if ( !isWorkingOnNeed ){
                if ( currentAction.satsifiedNeed ) {
                     currentAction.satsifiedNeed = false;
                    currentAction = workingNeed.satify();
                }
                else  currentAction = workingNeed.needHasNotBeenSatisfied();
               // if ( currentAction != null ) cur.doAction( gameObject );
                //currentAction = null;
            }
            waiter = Instantiate( waiterPrefab );

        }

    }

    public void changeMoney( int amount) {
        money += amount;
        if ( money < 0 ) money = 0;
    }

 

    public bool actionDefaultSatisfied() {
        satisfaction -= 1f;
        workingNeed.changeAskedCounter( false );// true sorgt für eine Verringerung des AskCounters im Bedürfniss
        if ( satisfaction > 10 ) satisfaction = 10;
        else if ( satisfaction < -10 ) satisfaction = -10;

        counterOfWaitingNeeds = 0;// !!Beobachten wegen: wird auf 0 gestetzt in jedem fall bei needs ohne station
        if ( counterOfWaitingNeeds < 0 ) counterOfWaitingNeeds = 0;


        return true;
    }

  

    public void failedToSatisfy() {
        satisfaction += 1f;
        if ( satisfaction > 10 ) satisfaction = 10;
        else if ( satisfaction < -10 ) satisfaction = -10;
        workingNeed.changeAskedCounter( true );// true sorgt für eine Erhöhung des AskCounters im Bedürfniss


        counterOfWaitingNeeds++;
        if ( counterOfWaitingNeeds > bedürfnisse.Length - 1 ) { counterOfWaitingNeeds = 0; }

       // KIAction action = workingNeed.needHasNotBeenSatisfied( gameObject );
       // if ( action != null ) action.doAction( gameObject );

       waitingForFreeNeedPoint = false;

    }

   

   

    public  void evaluateNeeds()
    {

        //noch sehr einfach (ermittelt das höchste bedürfniss.)
       

            Array.Sort( bedürfnisse );
            Array.Reverse( bedürfnisse );
            //highestNeedValue = b.Currentvalue;
            workingNeed = bedürfnisse[ counterOfWaitingNeeds];
        if ( workingNeed.currentvalue < 0 ) workingNeed = GetComponent<Freetime>();
            mostAskedNeed = setMostRequestedNeed();


        
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
            Bedurfniss work = GetComponent<Work>();
            work.increaseCurrentValue( 1 );
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
