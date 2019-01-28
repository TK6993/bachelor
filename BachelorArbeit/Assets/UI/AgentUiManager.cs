using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentUiManager : MonoBehaviour
{

    [SerializeField] KIAgent activeKIAgent;
    [SerializeField] Bedurfniss[ ] activeNeeds;

    [SerializeField] GameObject uiSatisfaction;

    [SerializeField]Text agentName;
    [SerializeField]Text factionName;
    [SerializeField] Text moneyAmount;

    [SerializeField] bool agentUIisVisible= false;

    [SerializeField] public GameObject needBarPrefab;
    public Dictionary<string, GameObject> uiNeeds = new Dictionary<string, GameObject>();
    //List<GameObject> uiNeeds = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if ( agentUIisVisible ) updateNeedView();
        //else  drawNeeds( activeKIAgent ); 
    }


    public void drawNeeds(KIAgent agent) {
        clearAgentUIManager();
        activeKIAgent = agent;
        agentUIisVisible = true;
        activeNeeds = agent.bedürfnisse;

        foreach ( Bedurfniss need in activeNeeds )
        {
            GameObject uiNeed = Instantiate<GameObject>( needBarPrefab );
            uiNeed.transform.parent = gameObject.transform;
            Healthbar bar = uiNeed.GetComponentInChildren<Healthbar>();
            Text needName = uiNeed.GetComponentInChildren<Text>();
            needName.text = need.name; 
            bar.Start();
            uiNeeds.Add( need.name, uiNeed );
            float barValue = (need.MinValue - need.currentvalue + need.MaxValue)+10;
            bar.SetHealth(barValue);

        }
        drawAgentInfos();
        drawName();

    }

    void drawAgentInfos()
    {
        Healthbar bar = uiSatisfaction.GetComponentInChildren<Healthbar>();
        //bar.Start();
        float barValue = ( -10 - activeKIAgent.satisfaction + 50 ) + 10;
        bar.SetHealth( barValue );
        moneyAmount.text = "" + activeKIAgent.money;


    }

    void drawName()
    {
        agentName.text = activeKIAgent.name;
        factionName.text = activeKIAgent.faction.fractionName;


    }

    void updateNeedView() {

        for ( int i = 0; i< activeNeeds.Length; i++ )
        {
            
            float barValue = (activeNeeds[i].MinValue - activeNeeds[ i ].currentvalue + activeNeeds[ i ].MaxValue)+10;
            uiNeeds[activeNeeds[i].name].GetComponentInChildren<Healthbar>().SetHealth( barValue );


        }
        drawAgentInfos();
    }


    public void clearAgentUIManager(){

            agentUIisVisible = false;
            activeNeeds = null;
        activeKIAgent = null;
        foreach ( GameObject uiNeed in uiNeeds.Values )
        {
            Destroy( uiNeed );
        }
        uiNeeds = new Dictionary<string, GameObject>();

    }


}
