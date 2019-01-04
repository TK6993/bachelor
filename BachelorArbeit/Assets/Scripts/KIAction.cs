using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KIAction : MonoBehaviour {




    public GameObject actor;
    public string name;

    public abstract bool doAction( GameObject actor );
    public abstract void setActor( GameObject actor );
}
