using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyActionA : KIAction
{
    public override bool doAction( GameObject actor )
    {
        satsifiedNeed = false;
        return true;
    }

   
}
