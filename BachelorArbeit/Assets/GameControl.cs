using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{

    private GameObject[] factions;

    // Start is called before the first frame update
    void Start()
    {
       factions = GameObject.FindGameObjectsWithTag( "faction" ); 
    }



    public void pressedPause() {
        pauseGame( true );

    }

    public void pressedPlay() {
        pauseGame( false );

    }

    private void pauseGame(bool value) {
        foreach ( GameObject faction in factions ) {
            KIFaction factionScript = faction.GetComponent<KIFaction>();
            foreach ( KIAgent agent in factionScript.agentMembers ){
                agent.pauseGame( value );
            }
            factionScript.pauseGame( value );

        }

    }
}
