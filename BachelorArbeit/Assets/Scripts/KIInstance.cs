using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KIInstance : MonoBehaviour {

    public Bedurfniss[ ] bedürfnisse;
    public bool isWorkingOnNeed;
    public NeedManager needManager;
    public GameObject waiter;
    public GameObject waiterPrefab;
    public Bedurfniss workingNeed;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract bool tryToSatisfyNeed( Bedurfniss workingNeed );
    public abstract void evaluateNeeds();
    public abstract void increaseNeeds();

}
