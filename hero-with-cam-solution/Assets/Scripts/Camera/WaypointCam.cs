using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCam : MonoBehaviour
{
    public Vector3 target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        //tempObject = GameObject.FindGameObjectWithTag("WayPoints");
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Debug.Log("goes here");
        // for(int i = 0; i < tempObject.mWayPoints.Length; i++)
        // {
        //     Debug.Log("for loop");
        //     if(tempObject.isWaypointShaking(i) == true)
        //     {
        //         Debug.Log("go here");
        //         target = tempObject.transform;
        //         gameObject.SetActive(true);
        //         break;
        //     }
        // }
        
       // Vector3 desiredPosition = target.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
       // transform.position = desiredPosition;

    }

    public void setTarget(Vector3 t)
    {
        target = t;
        transform.position = new Vector3(t.x, t.y, transform.position.z);
    }
}
