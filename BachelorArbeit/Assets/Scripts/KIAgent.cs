using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KIAgent : MonoBehaviour, IIndigent
{
    public NavMeshAgent navAgent;

    public KIFaction faction;
    public Bedurfniss[ ] bedürfnisse;
    public KIAction currentAction;
    public Bedurfniss workingNeed;
    public Bedurfniss mostAskedNeed;
    public int money = 0;
    public bool waitingForFreeNeedPoint = false;
    public float satisfaction = 0;


    [SerializeField]private int counterOfWaitingNeeds = 0;
    [SerializeField]private GameObject waiterPrefab;
    [SerializeField]private int salery;

    [HideInInspector]public List<GameObject> currentCollisions = new List<GameObject>();


    private GameObject waiter;
    private bool isWorkingOnNeed;
    private bool gameIsPaused = false;


    public int Salery
    {
        get
        {
            return (int) (salery * (1 + (0.5f * faction.Techlevel)));
        }

        set
        {
            salery = value;
        }
    }




    // Use this for initialization
    void Start()
    {
        
        
     }

    // Update is called once per frame
    void Update()
    {
        if ( gameIsPaused ) return;

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

 

    public  void evaluateNeeds()
    {

        //noch sehr einfach (ermittelt das höchste bedürfniss.)
       

        Array.Sort( bedürfnisse );
        Array.Reverse( bedürfnisse );
        satisfaction = calculateSatisfaction();
        workingNeed = bedürfnisse[ counterOfWaitingNeeds];
        if ( workingNeed.currentvalue < 0 ) {
         workingNeed = GetComponent<Freetime>();
        }

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

    public int pay( string v )
    {
        int payamount;
        switch ( v ) {
            case "food" :
                payamount = faction.foodPrice ;
                 break;
            case "taxes" :
                payamount =(int)( salery * ( faction.taxesPercent / 100.0f ));
                break;
            default:
                payamount = 0;
                break;
        }
        if ( payamount > money )
        {
            Bedurfniss pros  = GetComponent<Prosperity>();
            pros.increaseCurrentValue( 10 );
            return -1;
        }
        else {
            money -= payamount;
            return payamount;
        }
    }

    Bedurfniss setMostRequestedNeed() {
        Bedurfniss tempNeed = null;
        int highestAskCounter = 1;
        foreach ( Bedurfniss need in bedürfnisse )      
        {
            if ( need.name == "factionLoyalty" ) continue;
            if ( need.askForCounter > highestAskCounter ) {
                tempNeed = need;
                highestAskCounter = need.askForCounter;
            }
        }
        return tempNeed;
    }


    int calculateSatisfaction() {

        float tempSatisfaction = 0;
        float devineCounter = 0;

        foreach ( Bedurfniss need in bedürfnisse )
        {
            tempSatisfaction += need.currentvalue * need.priority;
            devineCounter += need.priority;
        }

        return (int) (tempSatisfaction / devineCounter);

    }

    public void changeWaitingCounter( bool changeDirection )
    {
        if ( changeDirection ) counterOfWaitingNeeds++;
        else counterOfWaitingNeeds = 0;
        if ( counterOfWaitingNeeds > bedürfnisse.Length - 1 ) { counterOfWaitingNeeds = 0; }
    }

    public void setWaitingForFreeNeedPoint( bool value ) {
        waitingForFreeNeedPoint = value;
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
         if ( col.gameObject.layer == 8 ) {
        col.GetComponent<NeedStation>().removeFromStation(gameObject);
        currentCollisions.Remove( col.gameObject );
          }
              
    }

    public void pauseGame( bool value )
    {
        gameIsPaused = value;
        GetComponent<NavMeshAgent>().isStopped = value;
    }
}
