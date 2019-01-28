using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IIndigent

{
    void evaluateNeeds();
    void increaseNeeds();
    void changeWaitingCounter(bool changeDirection);
    void setWaitingForFreeNeedPoint( bool value );
    void pauseGame( bool value );
}
