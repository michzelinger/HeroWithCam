using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public GameObject enemyCam;
    public GameObject waypointCam;

    public Text enemyCamStaus;
    public Text waypointCamStatus;

    // Variables for Enemy Cam
    EnemyBehavior enemy;
    HeroBehavior player;
    Vector3 playerPos;
    Vector3 enemyPos;

    // Start is called before the first frame update
    void Start()
    {
        waypointCam.SetActive(false);
        enemyCam.SetActive(false);
    }

    void LateUpdate()
    {
        if(enemyCam.activeInHierarchy && enemy != null && enemy.isChasing())
        {                  
            playerPos = player.gameObject.transform.position;
            enemyPos = enemy.gameObject.transform.position;
            enemyCam.GetComponent<Camera>().orthographicSize = (playerPos - enemyPos).magnitude;
            Vector3 newPos = Vector3.Lerp(enemyPos, playerPos, 0.5f);
            enemyCam.transform.position = new Vector3(newPos.x, newPos.y, -10f);
        }
        else
        {
            enemyCam.SetActive(false);
            enemyCamStaus.text = "Enemy Cam: Disabled";
        }
    }

    public void activateEnemyCam(EnemyBehavior enemy, HeroBehavior player)
    {
        enemyCamStaus.text = "Enemy Cam: Enabled";
        this.enemy = enemy;
        this.player = player;
        enemyCam.SetActive(true);
    }
    public void activateWaypointCam(float duration, Vector3 location)
    {
        if(!waypointCam.activeInHierarchy)
        {
            waypointCam.transform.position = new Vector3(location.x, location.y, transform.position.z);
            StartCoroutine(focusWaypointCam(duration));
        }
    }

    IEnumerator focusWaypointCam(float duration)
    {
        waypointCamStatus.text = "Waypoint Cam: Enabled";
        waypointCam.SetActive(true);
        yield return new WaitForSeconds(duration);
        waypointCam.SetActive(false);
        waypointCamStatus.text = "Waypoint Cam: Disabled";
    }
}