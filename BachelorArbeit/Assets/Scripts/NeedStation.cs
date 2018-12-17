using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedStation : MonoBehaviour
{

    public bool justforOne = false;
    public bool taken = false;
    public GameObject agentOnThisStation = null;

    // Use this for initialization
    void Start()
    {
        if ( !justforOne ) taken = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
  
}
