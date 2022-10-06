using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Floor_touch_count : MonoBehaviour
{
    private GameObject player;

    private GameObject levelMusic;
    private AudioSource levelSource;
    public int randomMusic = -1;

    private byte level;

    private GameObject Score;
    private Text newScore;
    private GameObject Score2;
    private Text newScore2;

    private GameObject Highscore;
    private Text newHighscore;

    public int showPoints = 0;
    public int showPoints2 = 0;
    public int showHighscore = 0;

    private playerMovement movement;

    private Transform playerTransform;

    private float groundPitchChanger = 1f;

    void Awake()
    {
        //Transform playerTranform = player.transform;


        player = GameObject.Find("Player");
        movement = player.GetComponent<playerMovement>();
        playerTransform = player.transform;


        Score = GameObject.Find("Ground_touch");
        newScore = Score.GetComponent<Text>();
        Score2 = GameObject.Find("Ground_touch2");
        newScore2 = Score2.GetComponent<Text>();

        Highscore = GameObject.Find("Highscore");
        newHighscore = Highscore.GetComponent<Text>();
    }

    void Update()
    {
        level = movement.level;
        StringBuilder sb = new StringBuilder("Highscore: ");

        //-------------------------------HIGHSCORE---------------------
        if (movement.groundTouch == true)
        {
            if ((int)playerTransform.position.y > showHighscore)
            {
                showHighscore = (int)playerTransform.position.y;
                //Math.floor();
            }
        }
        sb.Append(showHighscore);
        sb.Append("m");
        newHighscore.text = sb.ToString();
        sb.Clear();

        //newHighscore.text = "Highscore: " + showHighscore + "m";
        //-------------------------------Ground_Touch---------------------
        if (level == 1)
        {
            sb.Append("Floor touches(lvl1): ");
            sb.Append(showPoints);

            if (showPoints == 1)
            {
                sb.Append(" time");
                //newScore.text = "Floor touches (lvl1): " + showPoints + " time";
            }
            else
            {
                sb.Append(" times");
                //newScore.text = "Floor touches (lvl1): " + showPoints + " times";
            }
            newScore.text = sb.ToString();
        }
        else if(level == 2)
        {
            sb.Append("Floor touches(lvl2): ");
            sb.Append(showPoints2);

            if (showPoints2 == 1)
            {
                sb.Append(" time");
                //newScore2.text = "Floor touches (lvl2): " + showPoints2 + " time";
            }
            else
            {
                sb.Append(" times");
                //newScore2.text = "Floor touches (lvl2): " + showPoints2 + " times";
            }
            newScore2.text = sb.ToString();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Color color;
        AudioClip fail;

        fail = player.GetComponent<playerMovement>().fail;

        if (collision.gameObject.name == "Floor")
        {
            LevelMusicChanger();
            showPoints++;
            if (showPoints <= 150)
            {
                newScore.color = new Color(1, (float)showPoints / 150, 0, 1);
            }
            if (showPoints % 50 == 0)
            {
                player.GetComponent<playerMovement>().effectSource.PlayOneShot(fail);
            }
        }
        if (collision.gameObject.name == "Floor_lvl2")
        {
            LevelMusicChanger();
            showPoints2++;
            if (showPoints <= 150)
            {
                newScore.color = new Color(1, (float)showPoints2 / 150, 0, 1);
            }
            if (showPoints2 % 50 == 0)
            {
                player.GetComponent<playerMovement>().effectSource.PlayOneShot(fail);
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("XD");
        }


    }

    void LevelMusicChanger()
    {
        int temp, chanceToSwitch;
        levelMusic = GameObject.Find("LevelMusic");
        AudioSource[] levelSources = levelMusic.GetComponents<AudioSource>();
        levelSource = levelSources[1];

        chanceToSwitch = Random.Range(0, 5);
        if (randomMusic != -1)
        {
            if (chanceToSwitch == 0)
            {
                if (randomMusic != -1)
                {
                    levelSources[randomMusic].Stop();
                }
                temp = Random.Range(0, levelSources.Length);
                while (temp == randomMusic)
                {
                    temp = Random.Range(0, levelSources.Length);
                }
                randomMusic = temp;
                levelSources[randomMusic].Play();
                AudioListener.volume = 0.15f;
            }
        }
        else
        {
            temp = Random.Range(0, levelSources.Length);
            while (temp == randomMusic)
            {
                temp = Random.Range(0, levelSources.Length);
            }
            randomMusic = temp;
            levelSources[randomMusic].Play();
            AudioListener.volume = 0.15f;
        }  
    }
}

//int x = y > 0 ? 1 : 0;
//
//if(y > 0)
//{
//    x = 1;
//}
//else
//{
//    x = 0;
//}