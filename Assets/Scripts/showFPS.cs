using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class showFPS : MonoBehaviour
{
    public int avgFrameRate;
    private GameObject fps;
    private Text newFPS;

    private float timeToGo = 0;
    private float current = 0;
    private float currentTimeAgo = 0;
    private float timeTimeAgo = 0;

    void Awake()
    {
        fps = GameObject.Find("FPS");
        newFPS = fps.GetComponent<Text>();
    }
    void Update()
    {
       //currentTimeAgo = Time.frameCount;
       //timeTimeAgo = Time.time;
        if (Time.fixedTime > timeToGo)
        {
            //current -= currentTimeAgo;
            //timeTimeAgo -= Time.time;
            //current = Time.frameCount;
            current = Time.frameCount / Time.time;
            avgFrameRate = (int)current;
            newFPS.text = "FPS:" + avgFrameRate.ToString();
            timeToGo = Time.fixedTime + 1f;
            Debug.Log(avgFrameRate);
        }
    }
}