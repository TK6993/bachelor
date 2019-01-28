using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkPlace : NeedStation
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override bool resgisterOnStation( GameObject agent )
    {
        bool didRegister = base.resgisterOnStation( agent );
        if ( didRegister ) agent.GetComponent<Work>().setworkplace( this );
        return didRegister;
    }


    public override bool removeFromStation( GameObject agent )
    {
        if ( agent.GetComponent<KIAgent>().currentAction == agent.GetComponent<AgentDie>() ) return base.removeFromStation( agent );
        else return false;
    }

    public override void loseOwnerFaction( GameObject ownerFaction )
    {
        GameObject[ ] members = agentsOnThisStation.ToArray();
        foreach ( GameObject member in members )
        {
            member.GetComponent<Work>().workplace = null;
        }
        base.loseOwnerFaction( ownerFaction );
    }


}
