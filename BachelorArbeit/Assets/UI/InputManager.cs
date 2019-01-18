using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField]  Camera camera;
    [SerializeField] AgentUiManager agentUI;
    [SerializeField] GameObject uiAgentPanel;
    [SerializeField] StationUiManager stationUI;
    [SerializeField] GameObject uiStationPanel;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) )
        { // if left button pressed...
            Ray ray = camera.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit ) )
            {
                KIAgent agent = hit.transform.gameObject.GetComponent<KIAgent>();
                NeedStation station = hit.transform.gameObject.GetComponent<NeedStation>();
                if ( agent != null )
                {
                    uiAgentPanel.gameObject.SetActive( true );
                    agentUI.drawNeeds( agent );
                    //Debug.Log( hit.transform.gameObject.name );


                }
                else if ( station != null ) {
                    uiStationPanel.gameObject.SetActive( true );
                    stationUI.drawPanel( station );


                }
                else{
                    agentUI.clearAgentUIManager();
                    stationUI.clearUIManager();
                    uiAgentPanel.SetActive( false );
                    uiStationPanel.SetActive( false );


                }
                // the object identified by hit.transform was clicked
                // do whatever you want
            }
        }
    }
}
