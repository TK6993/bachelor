using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionLoyalty : Bedurfniss {

   [SerializeField] public bool paidTaxes = false;

    // Use this for initialization
    public override void Start(){
        base.Start();
        name = "factionLoyalty";
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public override KIAction needHasNotBeenSatisfied()
    {
        KIAgent agent = actor.GetComponent<KIAgent>();
       failedToSatisfy();
        return null;
    }

    public void setTaxesToPay( bool v )
    {
        paidTaxes = v;
    }

    public override KIAction satify()
    {
        KIAgent agent = actor.GetComponent<KIAgent>();
        actionDefaultSatisfied();
        decreaseCurrentValue( 50 );

        return null;
    }

    public override KIAction tryToSatisfy()
    {
        KIFaction agentsFaction = actor.GetComponent<KIAgent>().faction;
        KIAction taskToDoForTheFaction = agentsFaction.taskForAgents;
        if ( taskToDoForTheFaction != null )
        {
            return  taskToDoForTheFaction;
        }
        if ( !paidTaxes ) {
            KIAgent agent = gameObject.GetComponent<KIAgent>();
            int paiedTaxesAmount = agent.pay("taxes");
            if ( paiedTaxesAmount < 0 ) return gameObject.GetComponent<EmptyActionA>();
            else
            {
                agent.faction.increaseMoney( paiedTaxesAmount );
                paidTaxes = true;
            }
        }
        return null;


    }
}
