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
    [SerializeField] private NeedManager needManager;
    [SerializeField] private GameObject waiter;
    [SerializeField] private GameObject waiterPrefab;
    [SerializeField] private Bedurfniss workingNeed;
    [SerializeField] private int counterOfWaitingNeeds = 0;
    public bool waitingForFreeNeedPoint = false;

    public List<GameObject> currentCollisions = new List<GameObject>();
    public float satisfaction = 0;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        if ( waiter == null && !isWorkingOnNeed ) {
            waiter = Instantiate( waiterPrefab );

            increaseNeeds();
            evaluateNeeds();
            tryToSatisfyNeed( workingNeed );

        }


        else if ( waiter == null && isWorkingOnNeed ) {
            waiter = Instantiate( waiterPrefab );
            increaseNeeds();
            isWorkingOnNeed = tryToSatisfyNeed( workingNeed );


        }

    }
    // TODO: 2 und 3 ... wichtigstes bedürfnis angehen wenn bestzet
    // is wworking on needs betrachten 
    public  bool tryToSatisfyNeed( Bedurfniss workingNeed )
    {
        foreach ( GameObject coll in currentCollisions )
        {
            if ( coll.tag == workingNeed.name ) {

                NeedStation needS = coll.gameObject.GetComponent<NeedStation>();

                satisfaction -= 0.1f;
                if ( satisfaction > 10 ) satisfaction = 10;
                else if ( satisfaction < -10 ) satisfaction = -10;

                return workingNeed.satisfy();
            }
        }
        return true;
    }

    public  void evaluateNeeds()
    {

        //noch sehr einfach (ermittelt das höchste bedürfniss.)
       

            Array.Sort( bedürfnisse );
            Array.Reverse( bedürfnisse );
            //highestNeedValue = b.Currentvalue;
            workingNeed = bedürfnisse[ counterOfWaitingNeeds];



            if ( workingNeed != null )
            {
                FindWayTosatisfactionPoint( workingNeed );
                // Wenn kein freier Platz für die befriedigung des Bedürfnisses gefunden wurde ist "isworkingOnNeed" falsch und
                // needHasNotBeenSatisfied() wird aufgerufen sowie der  counterOfWaitingNeeds wird erhöht das als nächtes das am 2 meisten gewollte bedrüfniss befriedigt wird.
                isWorkingOnNeed = !waitingForFreeNeedPoint;
                if ( waitingForFreeNeedPoint )
                {
                    satisfaction += 0.5f;
                    if ( satisfaction > 10 ) satisfaction = 10;
                    else if ( satisfaction < -10 ) satisfaction = -10;
                    waitingForFreeNeedPoint = false;
                    counterOfWaitingNeeds++;
                    if(counterOfWaitingNeeds > bedürfnisse.Length-1 ) { counterOfWaitingNeeds = 0; }
                     workingNeed.needHasNotBeenSatisfied(needManager,gameObject);
            }
                else {
                    counterOfWaitingNeeds--;
                    if ( counterOfWaitingNeeds < 0 ) counterOfWaitingNeeds = 0;
                }
            }
        

    }

    private void FindWayTosatisfactionPoint( Bedurfniss b )
    {
        Vector3 pointForSatisfaction = needManager.getNearestPointofSatisfaction( b, gameObject);
        navAgent.SetDestination( pointForSatisfaction );
    }


    public  void increaseNeeds() {
        foreach ( Bedurfniss need in bedürfnisse )
        {
            need.changeNeed();
        }


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
