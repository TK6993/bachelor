using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionUiManger : MonoBehaviour
{
    [SerializeField] KIFaction activeKIFaction;
    [SerializeField] Bedurfniss[ ] activeNeeds;

    [SerializeField] GameObject uiSatisfaction;

    [SerializeField]  Text factionName;
    [SerializeField]  Text moneyAmount;


    [SerializeField] bool uIisVisible = false;

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

        if ( uIisVisible ) updateView();
        //else  drawNeeds( activeKIAgent ); 
    }


    public void drawPanel( KIFaction faction )
    {
        clearUIManager();
        activeKIFaction = faction;
        uIisVisible = true;
        activeNeeds = faction.bedüfnisse;

        foreach ( Bedurfniss need in activeNeeds )
        {
            GameObject uiNeed = Instantiate<GameObject>( needBarPrefab );
            uiNeed.transform.parent = gameObject.transform;
            Healthbar bar = uiNeed.GetComponentInChildren<Healthbar>();
            Text needName = uiNeed.GetComponentInChildren<Text>();
            needName.text = need.name;
            bar.Start();
            uiNeeds.Add( need.name, uiNeed );
            float barValue = ( need.MinValue - need.currentvalue + need.MaxValue ) + 10;
            bar.SetHealth( barValue );

        }
        drawInfos();
        drawName();

    }

    void drawInfos()
    {
        Healthbar bar = uiSatisfaction.GetComponentInChildren<Healthbar>();
        //bar.Start();
        float barValue = ( -10 - activeKIFaction.gameObject.GetComponent<CitizenSatisfaction>().currentvalue + 50 ) + 10;
        bar.SetHealth( barValue );
        moneyAmount.text = "" + activeKIFaction.money;

    }

    void drawName()
    {
        factionName.text = activeKIFaction.fractionName;


    }

    void updateView()
    {

        for ( int i = 0; i < activeNeeds.Length; i++ )
        {

            float barValue = ( activeNeeds[ i ].MinValue - activeNeeds[ i ].currentvalue + activeNeeds[ i ].MaxValue ) + 10;
            uiNeeds[ activeNeeds[ i ].name ].GetComponentInChildren<Healthbar>().SetHealth( barValue );


        }
        drawInfos();
    }


    public void clearUIManager()
    {

        uIisVisible = false;
        activeNeeds = null;
        activeKIFaction = null;
        foreach ( GameObject uiNeed in uiNeeds.Values )
        {
            Destroy( uiNeed );
        }
        uiNeeds = new Dictionary<string, GameObject>();

    }

}
