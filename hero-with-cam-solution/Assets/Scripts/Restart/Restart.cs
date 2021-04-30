using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Restart : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("ClassExample");
    }
    /*void Start()
    {
        GameObject.Find ("Restart").transform.localScale = new Vector3(0, 0, 0);
    }*/
    
}
