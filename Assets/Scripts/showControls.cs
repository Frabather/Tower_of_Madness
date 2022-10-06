using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class showControls : MonoBehaviour
{
    private GameObject MoveKeys;
    float timeLeft = 5.0f;
    private GameObject player;
    private byte level;

    void Start()
    {
        player = GameObject.Find("Player");
        level = player.GetComponent<playerMovement>().level;
    }

    void Update()
    {
        if (level == 1)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                GameObject.Find("Instructions").GetComponent<Image>().enabled = false;
                GameObject.Find("Player").GetComponent<showControls>().enabled = false;
            }
        }
        else
        {
            GameObject.Find("Instructions").GetComponent<Image>().enabled = false;
            GameObject.Find("Player").GetComponent<showControls>().enabled = false;
        }
    }
}
