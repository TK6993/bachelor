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
    [SerializeField] private List<GameObject> ListOfVisitors;

    // Use this for initialization
    void Start()
    {
        if ( !hasCapacity ) stationSize = 1000;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool resgisterOnStation( GameObject agent) {
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

    public bool removeFromStation(GameObject agent) {
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
    }

     void OnTriggerExit( Collider other )
    {
        ListOfVisitors.Remove( other.gameObject );
    }

   


}
