using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationUiManager : MonoBehaviour
{

    [SerializeField] NeedStation activeStation;

    [SerializeField]
    GameObject uiAgentCounter;

    [SerializeField]
    Text needName;
    [SerializeField]
    Text factionName;


    [SerializeField]
    int maxVisiters;


    [SerializeField]
    bool stationUIisVisible = false;

    [SerializeField]
    //List<GameObject> uiNeeds = new List<GameObject>();
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {

        if ( stationUIisVisible ) updateView();
        //else  drawNeeds( activeKIAgent ); 
    }


    public void drawPanel( NeedStation station )
    {
        clearUIManager();
        activeStation = station;
        stationUIisVisible = true;

        maxVisiters = station.stationSize;

        int visitors = station.agentsOnThisStation.Count;
        Text counterText = uiAgentCounter.GetComponentInChildren<Text>();
        counterText.text = "" + visitors + "/" + maxVisiters;

        Healthbar bar = uiAgentCounter.GetComponentInChildren<Healthbar>();
        bar.maximumHealth = maxVisiters;
        bar.Start();
        bar.SetHealth( visitors );
          

        
       // drawAgentInfos();
        drawName();

    }

    void drawInfos()
    {
        //Healthbar bar = uiAgentCounter.GetComponentInChildren<Healthbar>();
        //bar.Start();
       // float barValue = ( -10 - activeKIAgent.satisfaction + 10 ) + 10;
    //    bar.SetHealth( barValue );


    }

    void drawName()
    {
        factionName.text = activeStation.ownerFaction.GetComponent<KIFaction>().fractionName;
        needName.text = activeStation.gameObject.tag;


    }

    void updateView()
    {


        int visitors = activeStation.agentsOnThisStation.Count;
        Text counterText = uiAgentCounter.GetComponentInChildren<Text>();
        counterText.text = "" + visitors + "/" + maxVisiters;

        Healthbar bar = uiAgentCounter.GetComponentInChildren<Healthbar>();
        bar.SetHealth( visitors );
    }


    public void clearUIManager()
    {

        stationUIisVisible = false;
        

    }

}
