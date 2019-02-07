using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedStation : MonoBehaviour
{

    public bool hasCapacity = false;
    public bool isFull = false;
    public int stationSize = 1;
    public List<GameObject> agentsOnThisStation = new List<GameObject>();
    public bool isInPossession =false;
    public GameObject ownerFaction;
    public int level = 1;
    public GameObject waiterPrefab;
    public int lifeCounterLimit = 120;

    [SerializeField] private List<GameObject> ListOfVisitors;
    [SerializeField] int upgradeCosts= 70;

    [SerializeField] private int counterIsInUse = 0;
    private GameObject waiter;



    public int UpgradeCosts
    {
        get
        {
            int kosten = upgradeCosts * level *level * level;
            return (int) (kosten - ((kosten) * ( 0.05f * ownerFaction.GetComponent<KIFaction>().Techlevel )));

        }

        set
        {
            upgradeCosts = value;
        }
    }

    // Use this for initialization
    public virtual void Start()
    {
        if ( !hasCapacity ) stationSize = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if ( ownerFaction != null ) {
            if ( ownerFaction.GetComponent<KIFaction>().GameIsPaused ) return;
                if(waiter == null ){
                    counterIsInUse++;
                if ( counterIsInUse >= lifeCounterLimit ) loseOwnerFaction( ownerFaction );
                    waiter = Instantiate( waiterPrefab );
                }

        }
    }

    public virtual void loseOwnerFaction( GameObject ownerFaction )
    {

        agentsOnThisStation.Clear();
        isFull = false;
        isInPossession = false;
        List<GameObject> needListOfStationOfThisKind = ownerFaction.GetComponent<KIFaction>().listOfAgentNeedStations[ gameObject.tag ];
        needListOfStationOfThisKind.Remove( gameObject );
        gameObject.transform.GetChild( 0 ).gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
        this.ownerFaction = null;
    }


    public virtual bool resgisterOnStation( GameObject agent) {
        if ( agentsOnThisStation.Count < stationSize && !agentsOnThisStation.Contains( agent ) )
        {
            agentsOnThisStation.Add( agent );
            // agent.GetComponent<KIAgent>().currentCollisions.Add( gameObject );
            if ( agentsOnThisStation.Count >= stationSize ) isFull = true;
            if ( ListOfVisitors.Contains( agent ) ) {
                agent.GetComponent<KIAgent>().currentCollisions.Add( gameObject );

            }
            return true;
        }
        else if ( agentsOnThisStation.Count >= stationSize )
        {
            isFull = true;
            return false;
        }
        else return false;

    }

    public virtual bool removeFromStation(GameObject agent) {
        if ( agentsOnThisStation.Contains( agent ) )
        {
            isFull = false;
            agentsOnThisStation.Remove( agent );
           // agent.GetComponent<KIAgent>().currentCollisions.Remove( gameObject );
            return true;
        }
        else { return false; }
        }

    public bool isItFull() {

        if ( agentsOnThisStation.Count >= stationSize )
        {
            isFull = true;
            return true;
        }
        else
        {
            isFull = false;
            return false;
        }
    }


     void OnTriggerEnter( Collider other )
    {
        ListOfVisitors.Add( other.gameObject );
        counterIsInUse = 0;
    }

     void OnTriggerExit( Collider other )
    {
        ListOfVisitors.Remove( other.gameObject );
    }

   


}
