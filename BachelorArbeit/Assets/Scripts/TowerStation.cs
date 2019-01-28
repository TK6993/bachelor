using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStation : MonoBehaviour
{
    public PlaceTowerA creatingAction;
    private bool isBuild =false;
    [SerializeField] Collider shootRadius;
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private GameObject placeholderBullet;


    void OnTriggerEnter( Collider other )
    {
        if ( !isBuild )
        {
            if ( other.gameObject.layer == 8 || other.gameObject.layer == 10 )
            {
                creatingAction.findAnotherPlaceForTowerAtStation( gameObject );
                Destroy( gameObject );

            }
        }
        else {
            if ( other.gameObject.layer == 9 ) {
                KIAgent agent = other.gameObject.GetComponent<KIAgent>();
                shootAt( agent );
            }


        }
    }

    private void OnTriggerStay( Collider other )
    {
        if ( isBuild )
        {
         
            if ( other.gameObject.layer == 9 )
            {
                KIAgent agent = other.gameObject.GetComponent<KIAgent>();
                shootAt( agent );
            }


        }
    }

    private void shootAt( KIAgent agent )
    {
        if ( agent.faction != creatingAction.faction ) {
            Vector3 vec = placeholderBullet.transform.position;
             GameObject bullet = Instantiate( bulletprefab, vec, Quaternion.identity );
            Vector3 shootDir = Vector3.Normalize( agent.transform.position - vec );
            shootDir.y = 0;
            bullet.GetComponent<Rigidbody>().AddForce( shootDir * 700);
        }
    }

    internal void setTowerActive()
    {
        isBuild = true;
        shootRadius.enabled = true;
    }
}
