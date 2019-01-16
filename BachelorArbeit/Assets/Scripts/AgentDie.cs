﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDie : KIAction {


    public override bool doAction( GameObject actor )
    {
        this.actor = actor;
        KIAgent agent = actor.GetComponent<KIAgent>();
        agent.faction.logoutAgentfromStation( actor );
        
        Destroy( gameObject );
        agent.faction.updateAgentMemberList();
        return true;
    }

    public override void setActor( GameObject actor )
    {
        this.actor = actor;
       

   
    }

    // Use this for initialization
    void Start () {
        name = "agentDie";
    }
	

}
