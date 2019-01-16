using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KIAction : MonoBehaviour {




    public GameObject actor;
    public string name;
    public bool satsifiedNeed = false;

    public abstract bool doAction( GameObject actor );
    public virtual void setActor( GameObject actor ) {
        this.actor = actor;


    }
}
