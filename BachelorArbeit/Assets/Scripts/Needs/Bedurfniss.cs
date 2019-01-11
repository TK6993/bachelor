using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bedurfniss : MonoBehaviour, IComparable {


  [SerializeField] private int increaseValue = 1;
    public bool needWithNeedStation = false;
    public int askForCounter = 0;
  public int currentvalue;
  private int maxValue= 500;
  private int minValue=-200;
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

    abstract public KIAction needHasNotBeenSatisfied( GameObject actor );

    abstract public bool satisfy( GameObject actor );
   


    public int CompareTo( object obj )
    {
        Bedurfniss otherBedurfnis = obj as Bedurfniss;
        if ( otherBedurfnis != null )
            return this.currentvalue.CompareTo( otherBedurfnis.currentvalue );
        else
            throw new ArgumentException( "Object is not a Temperature" );
    }

    public void  changeAskedCounter( bool changeDirection ) {
        askForCounter += ( -1 + (2* Convert.ToInt32( changeDirection )) );
        if ( askForCounter < 0 ) askForCounter = 0;

    }
}
