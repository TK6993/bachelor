using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bedurfniss : MonoBehaviour, IComparable {


  [SerializeField] private int increaseValue = 1;
  public int currentvalue;
  private int maxValue= 60;
  private int minValue=-10;
  public string name;

    public int MaxValue
    {
        get
        {
            return maxValue;
        }

        set
        {
            maxValue = value;
        }
    }


    public int MinValue
    {
        get
        {
            return minValue;
        }

        set
        {
            minValue = value;
        }
    }

    public int Currentvalue
    {
        get
        {
            return currentvalue;
        }

        set
        {
            currentvalue = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}


	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeNeed() {

        increaseCurrentValue( increaseValue );
    }

 


   public  int decreaseCurrentValue(int amount) {
        currentvalue -= amount;
        if ( currentvalue < minValue ) currentvalue = minValue;
        return currentvalue;

    }

   public int increaseCurrentValue( int amount ) {
        currentvalue += amount;
        if ( currentvalue > maxValue ) currentvalue = maxValue;
        return currentvalue;
    }

    abstract public bool needHasNotBeenSatisfied( NeedManager needM, GameObject agent );

    public virtual bool satisfy() {
        //work to do
       return false;
       
    }

    public int CompareTo( object obj )
    {
        Bedurfniss otherBedurfnis = obj as Bedurfniss;
        if ( otherBedurfnis != null )
            return this.currentvalue.CompareTo( otherBedurfnis.currentvalue );
        else
            throw new ArgumentException( "Object is not a Temperature" );
    }
}
