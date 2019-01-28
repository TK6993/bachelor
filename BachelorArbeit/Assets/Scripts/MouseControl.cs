using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public Vector2 camLimitZ;
    public Vector2 camLimitX;
    public Vector2 zoomLimit;

    public float mouseSpeed = 20.0f;
    public float zoomSpeed = 100.0f;
    public float touchingBorder = 20.0f;

    private float zoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float zoom = GetComponent<Camera>().orthographicSize;
        float scroll = Input.GetAxis( "Mouse ScrollWheel" );
        zoom -= scroll * zoomSpeed * Time.deltaTime;

        Vector3 camPosition = transform.position;
        if ( Input.mousePosition.y >= Screen.height - touchingBorder ) {
            camPosition.z += mouseSpeed * Time.deltaTime;
        }


        if ( Input.mousePosition.y <=  touchingBorder )
        {
            camPosition.z -= mouseSpeed * Time.deltaTime;
        }

        if ( Input.mousePosition.x >= Screen.width - touchingBorder )
        {
            camPosition.x += mouseSpeed * Time.deltaTime;
        }

        if ( Input.mousePosition.x <=  touchingBorder )
        {
            camPosition.x -= mouseSpeed * Time.deltaTime;
        }

        camPosition.x = Mathf.Clamp( camPosition.x, camLimitX.x, camLimitX.y );
        camPosition.z = Mathf.Clamp( camPosition.z, camLimitZ.x, camLimitZ.y );
        zoom = Mathf.Clamp( zoom, zoomLimit.x, zoomLimit.y );




        GetComponent<Camera>().orthographicSize = zoom;
        transform.position = camPosition;
    }
}
