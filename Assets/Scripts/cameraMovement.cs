using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private GameObject player;
    private GameObject camera;

    private byte level;

    void Awake()
    {
        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
        //camera = Camera.main;
    }

    void Update()
    {

        level = player.GetComponent<playerMovement>().level;
        if (level == 1)
        {
            if (player.transform.position.y <= 6.87f)
            {
                camera.transform.position = new Vector3(-0.48f, 6.88f, -10);
            }
            else
            {
                camera.transform.position = new Vector3(-0.48f, player.transform.position.y, -10);
            }
        }
        if(level == 2)
        {
            if (player.transform.position.y <= 6.87f)
            {
                camera.transform.position = new Vector3(39.85f, 6.88f, -10);
            }
            else if(player.transform.position.y >= 85f)
            {
                camera.transform.position = new Vector3(39.85f, 85f, -10);
            }
            else
            {
                camera.transform.position = new Vector3(39.85f, player.transform.position.y, -10);
            }
        }
        
    }
}
