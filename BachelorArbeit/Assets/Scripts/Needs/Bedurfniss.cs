using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bedurfniss : MonoBehaviour, IComparable {


  [SerializeField] private float increaseValue = 1;
    public bool needWithNeedStation = false;
    public int askForCounter = 0;
  public float currentvalue;
  private int maxValue= 50;
  private int minValue=-10;
  public string name;
    public GameObject actor;
    public float  priority =1;

    public virtual void Start()
    {
        actor = gameObject;

    }


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

    public float Currentvalue
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

  


    public virtual void changeNeed() {

        increaseCurrentValue( increaseValue );
    }

 


   public  float decreaseCurrentValue(int amount) {
        currentvalue -= amount;
        if ( currentvalue < minValue ) currentvalue = minValue;
        return currentvalue;

    }

   public float increaseCurrentValue( float amount ) {
        currentvalue += amount;
        if ( currentvalue > maxValue ) currentvalue = maxValue;
        return currentvalue;
    }

    abstract public KIAction needHasNotBeenSatisfied();

    abstract public KIAction tryToSatisfy( );

    abstract public KIAction satify();



    public int CompareTo( object obj )
    {
        Bedurfniss otherBedurfnis = obj as Bedurfniss;
        if ( otherBedurfnis != null ) { 
           // if ( this.currentvalue < 0 && otherBedurfnis.currentvalue < 0 ) return 0;
           // if ( this.currentvalue < 0 ) return 1;
            return this.currentvalue.CompareTo( otherBedurfnis.currentvalue );
    }
        else
            throw new ArgumentException( "Object is not a Need" );
    }

    public void  changeAskedCounter( bool changeDirection ) {
        if ( changeDirection ) askForCounter++;
        else askForCounter = 0;
        if ( askForCounter < 0 ) askForCounter = 0;

    }

    public void actionDefaultSatisfied()
    {
        IIndigent a = actor.GetComponent<IIndigent>();
        changeAskedCounter( false );
        a.changeWaitingCounter( false ); ;// !!Beobachten wegen: wird auf 0 gestetzt in jedem fall bei needs ohne station
        
    }

    public void failedToSatisfy()
    {
        IIndigent a = actor.GetComponent<IIndigent>();
        changeAskedCounter( true );// true sorgt für eine Erhöhung des AskCounters im Bedürfniss
        a.changeWaitingCounter( true );


        // KIAction action = workingNeed.needHasNotBeenSatisfied( gameObject );
        // if ( action != null ) action.doAction( gameObject );

        a.setWaitingForFreeNeedPoint(false);

    }

}
