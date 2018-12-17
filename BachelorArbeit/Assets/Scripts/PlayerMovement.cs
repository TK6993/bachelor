using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{

    public Camera cam;
    public NavMeshAgent agent;
    public Transform[ ] goals;
    public int counter = 0;
    private Waiter waiter;

    // Use this for initialization
    void Start()
    {
        agent.SetDestination( goals[ counter ].position );
        waiter = gameObject.GetComponent<Waiter>();

    }

    // Update is called once per frame
    void Update()
    {

        if ( agent.remainingDistance <= 0.5f) {

            if ( true )
            {
             counter++;
             if ( counter >= goals.Length ) counter = 0;   
             agent.SetDestination( goals[ counter ].position );
            }
        }
        




        //mouseClickControle
        //if ( Input.GetMouseButtonDown( 0 ) )
        //{
        //    Ray ray = cam.ScreenPointToRay( Input.mousePosition );
        //    RaycastHit hit;
        //    if ( Physics.Raycast( ray, out hit ) )
        //    {

        //        agent.SetDestination( hit.point );
        //        agent.isStopped
        //    }

        //}



    }
}
