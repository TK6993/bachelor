using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KIAgent : MonoBehaviour
{
    public NavMeshAgent navAgent;   
    public Bedurfniss[ ] bedürfnisse;
    public bool isWorkingOnNeed;
    public NeedManager needManager;
    public GameObject waiter;
    public GameObject waiterPrefab;
    public Bedurfniss workingNeed;
    public bool waitingForFreeNeedPoint = false;

    List<GameObject> currentCollisions = new List<GameObject>();


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ( waiter == null && !isWorkingOnNeed) {
            waiter = Instantiate( waiterPrefab );

            increaseNeeds();
            evaluateNeeds();
        }

        else if ( isWorkingOnNeed ) {

            tryToSatisfyNeed( workingNeed );

        }

    }

    private void tryToSatisfyNeed( Bedurfniss workingNeed )
    {
        foreach ( GameObject coll in currentCollisions )
        {
            if ( coll.tag == workingNeed.name ) {

              isWorkingOnNeed =  workingNeed.satisfy();
            }
        }
    }

    private void evaluateNeeds()
    {

        //noch sehr einfach (nimmt erstes unbefreidigtes bedürfniss.)
        if ( !isWorkingOnNeed )
        {
            int highestNeedValue = -20;
            Bedurfniss highestNeed = null;
            foreach ( Bedurfniss b in bedürfnisse )
            {
                if ( b.Currentvalue >= highestNeedValue)
                {
                    highestNeedValue = b.Currentvalue;
                    highestNeed = b;

                }
            }
            if ( highestNeed != null )
            {
                FindWayTosatisfactionPoint( highestNeed );
                workingNeed = highestNeed;
                isWorkingOnNeed = !waitingForFreeNeedPoint;
            }
        }

    }

    private void FindWayTosatisfactionPoint( Bedurfniss b )
    {
        Vector3 pointForSatisfaction = needManager.getNearestPointofSatisfaction( b, gameObject);
        navAgent.SetDestination( pointForSatisfaction );
    }


    private void increaseNeeds() {
        foreach ( Bedurfniss need in bedürfnisse )
        {
            need.changeNeed();
        }


    }
   


    void OnTriggerEnter( Collider col )
    {
        currentCollisions.Add( col.gameObject );
    }

    void OnTriggerExit( Collider col )
    {
       currentCollisions.Remove( col.gameObject );
       if ( col.gameObject.layer == 8 ) {
            NeedStation needS = col.gameObject.GetComponent<NeedStation>();
            if ( needS.taken && needS.agentOnThisStation == gameObject) {
                    needS.taken = false;
                    needS.agentOnThisStation = null;
                }
        }
    }


}
