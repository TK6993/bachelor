﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToA : KIAction
{
    Vector3 destiantion = new Vector3(0,0,0);
    NavMeshAgent navAgent = null;
    KIAgent agent = null;
    GameObject destinationObject = null;

    public override bool doAction( GameObject actor )
    {
        if ( destiantion.sqrMagnitude <= 0.0f ) {
                navAgent = actor.GetComponent<NavMeshAgent>();
                agent = actor.GetComponent<KIAgent>();
            if ( destinationObject == null )
            {
                GameObject needStationObject = agent.faction.getNearestPointofSatisfaction( agent.workingNeed, actor );
                if ( agent.waitingForFreeNeedPoint )
                {
                    satsifiedNeed = false;
                    return true;
                }
                destiantion = needStationObject.transform.position;
            }
            else destiantion = destinationObject.transform.position;
            actor.GetComponent<NavMeshAgent>().SetDestination( destiantion );
            return false;
        }
        if ( navAgent.remainingDistance > 0.5f ) return false;
        else
        {
            satsifiedNeed = true;
            destiantion = new Vector3( 0, 0, 0 );
            destinationObject = null;
            navAgent = null;
            agent = null;
            return true;
        }

    }

    public void setDestinationObject(GameObject destinationO) {
       
        destinationObject = destinationO;

    }


}
