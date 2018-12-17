using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour {

    // Das objekt wartet eine gewisse Zeit bis es timeisUp als true zurückgibt.

    float elapsedTime;
    public int unitSize = 1;
    public int unitsToWait = 1;
    bool startedTimer = false;
   public  bool timeisUp = true;

	// Use this for initialization
	void Start () {
        elapsedTime = 0;
        startTimer( 5 );
	}


    // Update is called once per frame
    void Update()
    {
       // Debug.Log( elapsedTime );
        if(startedTimer)elapsedTime += Time.deltaTime;
        if ( elapsedTime >= unitSize * unitsToWait ) {
            timeisUp = true;
        }
        if ( timeisUp ) resetWaiter();
    }


    public void startTimer(int waitUnits) {
        if ( !startedTimer ) {
            unitsToWait = waitUnits;
            timeisUp = false;
            startedTimer = true;
        }
      
    }

    void resetWaiter() {
        elapsedTime = 0;
        unitsToWait = 1;
        startedTimer = false;

        Destroy( gameObject );


    }
}
