using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.tag != "tower" ) {
            if ( other.gameObject.layer == 9 ) {
                other.gameObject.GetComponent<Hunger>().increaseCurrentValue( 5 );

            }
          //  Destroy( gameObject );

        }
    }
}
