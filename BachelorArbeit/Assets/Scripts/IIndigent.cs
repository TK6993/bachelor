using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IIndigent

{
    bool tryToSatisfyNeed( Bedurfniss workingNeed );
    void evaluateNeeds();
    void increaseNeeds();
}
