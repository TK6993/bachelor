using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIFaction : NeedManager,IIndigent {

    public string fractionName;

    public KIAgent[ ] agentMembers;


    public Bedurfniss[ ] fractionNeeds;
    [SerializeField] private Bedurfniss workingNeed;
    [SerializeField] private int counterOfWaitingNeeds = 0;

    [SerializeField] private GameObject waiter;
    [SerializeField] private GameObject waiterPrefab;
    [SerializeField] private bool isWorkingOnNeed;





    // Use this for initialization
    public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update ()  {
        if ( waiter == null && !isWorkingOnNeed )
        {
            waiter = Instantiate( waiterPrefab );

            //increaseNeeds();
            //evaluateNeeds();
            //tryToSatisfyNeed( workingNeed );

        }


        else if ( waiter == null && isWorkingOnNeed )
        {
           /* waiter = Instantiate( waiterPrefab );
            increaseNeeds();
            isWorkingOnNeed = tryToSatisfyNeed( workingNeed );
            */

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
        throw new NotImplementedException();
    }
}
