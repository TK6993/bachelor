using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTaxesA : KIAction
{
    public override bool doAction( GameObject actor )
    {
        gameObject.GetComponent<KIFaction>().updateAgentMemberList(null);
        KIAgent[ ] factionAgents = gameObject.GetComponent<KIFaction>().agentMembers;
        foreach ( KIAgent agent in factionAgents )
        {
            agent.gameObject.GetComponent<FactionLoyalty>().setTaxesToPay( false );
        }
        satsifiedNeed = true;
        return true;

    }

}
