using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class playerMovement : MonoBehaviour
{
    private GameObject levelMusic;
    private AudioSource levelSource;
    private GameObject groundTouchLevelInfo2;

    private GameObject level1;
    private GameObject level2;

    private Rigidbody rb;

    int randomMusic;

    public bool groundTouch = false;
    private float veloDiff = 0f;
    private float playerYSpeed = 0f;
    private float addPlayerYSpeed = 0f;
    private int moveSpeed = 500;
    public Animator animator;

    private GameObject mainCamera;
    public AudioSource effectSource;
    private AudioClip winning;


    public AudioClip fail;

    //public int Fail 
    //{ 
    //    get => fail;
    //    private set;
    //}
    //

    public byte level;
    public int spaceClickedCounter = 0;


    private GameObject spaceClickedCounterObject;
    private Text text_wynik;

    private Color lerpedColor = Color.white;

    private float GameEndTime = 0;
    private float TimeOnPause = 0;
    private float SummaryTimeOnPause = 0;

    private bool StuckTimer_Switch = true;
    private float StuckTimer = 0;
    private GameObject Stuck_Button;
    private Button Stuck_component;
    private GameObject Stuck_Button_cancel;
    private Button Stuck_component_cancel;

    void Awake()
    {
        level = 1;

        level1 = GameObject.Find("Level_1");
        level2 = GameObject.Find("Level_2");

        Stuck_Button = GameObject.Find("Stuck_Button");
        Stuck_component = Stuck_Button.GetComponent<Button>();
        Stuck_component.onClick.AddListener(Stuck);
        Stuck_Button_cancel = GameObject.Find("Cancel_stuck_button");
        Stuck_component_cancel = Stuck_Button_cancel.GetComponent<Button>();
        Stuck_component_cancel.onClick.AddListener(CancelStuck);
        Stuck_Button.SetActive(false);
        Stuck_Button_cancel.SetActive(false);


        levelMusic = GameObject.Find("LevelMusic");
        AudioSource[] levelSources = levelMusic.GetComponents<AudioSource>();
        levelSource = levelSources[1];

        mainCamera = GameObject.Find("Main Camera");
        AudioSource[] effectSources = mainCamera.GetComponents<AudioSource>();
        effectSource = effectSources[1];
        winning = effectSources[1].clip;
        fail = effectSources[2].clip;

        spaceClickedCounterObject = GameObject.Find("spaceClickedCounter");
        text_wynik = spaceClickedCounterObject.GetComponent<Text>();
        spaceClickedCounterObject.SetActive(false);

        if (level == 1)
        {
            groundTouchLevelInfo2 = GameObject.Find("Ground_touch2");
            groundTouchLevelInfo2.SetActive(false);
            level2.SetActive(false);
        }
        if (level == 2)
        {
            groundTouchLevelInfo2 = GameObject.Find("Ground_touch2");
            groundTouchLevelInfo2.SetActive(true);
        }


        rb = GetComponent<Rigidbody>();
        GameObject.Find("Instructions").GetComponent<Image>().enabled = true;
        rb.freezeRotation = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        randomMusic = rb.GetComponent<Floor_touch_count>().randomMusic;

        //------------------Stats-Color-Changer----------------

        if (spaceClickedCounterObject.active)
        {
            text_wynik.color = Color.Lerp(Color.green, Color.black, Mathf.PingPong(Time.time, 1));
        }


        //-----------------ANTI-FALL------------------------
        playerYSpeed = rb.velocity.y;
        if (rb.velocity.y <= -10)
        {
            veloDiff = ((playerYSpeed + 10) / 100);
            addPlayerYSpeed = 750 + 750 * (-veloDiff);
            rb.AddForce(0, addPlayerYSpeed * Time.deltaTime, 0);
        }

        //-----------------GAME-BUG RESET------------------------
        if(level == 1)
        {
            if (rb.transform.position.x <= -17f || rb.transform.position.x >= 15f || rb.transform.position.y <= -1f)
            {
                rb.transform.position = new Vector3(0, 0, 0);
            }
        }
        else if (level == 2)
        {
            if (rb.transform.position.x <= 25f || rb.transform.position.x >= 55f || rb.transform.position.y <= -5f)
            {
                rb.transform.position = new Vector3(40, 0, 0);
            }

        }
        if (rb.velocity.magnitude < 0.15f && groundTouch == false)
        {
            if(StuckTimer_Switch)
            {
                StuckTimer = Time.fixedTime;
                StuckTimer_Switch = false;
            }
            if(Time.fixedTime > StuckTimer + 3f && !Stuck_Button.active)
            {
                Stuck_Button.SetActive(true);
                Stuck_Button_cancel.SetActive(true);
                Cursor.visible = true;
            }
        }
        //-----------------GAME PAUSE------------------------
        if (Input.GetKeyDown(KeyCode.Escape))

            //pause
            if (Time.timeScale == 1)
            {
                TimeOnPause = Time.fixedTime;
                Cursor.visible = true;
                GameObject.Find("Instructions").GetComponent<Image>().enabled = true;
                GameObject.Find("Game_Paused").GetComponent<Image>().enabled = true;
                GameObject.Find("Player").GetComponent<showFPS>().enabled = false;
                Time.timeScale = 0;
                AudioSource[] levelSources = levelMusic.GetComponents<AudioSource>();
                levelSource = levelSources[1];
                levelSources[randomMusic].Pause();
            }
            //unpause
            else
            {
                TimeOnPause = Time.fixedTime - TimeOnPause;
                SummaryTimeOnPause += TimeOnPause;
                Cursor.visible = false;
                GameObject.Find("Instructions").GetComponent<Image>().enabled = false;
                GameObject.Find("Game_Paused").GetComponent<Image>().enabled = false;
                GameObject.Find("Player").GetComponent<showFPS>().enabled = true;
                Time.timeScale = 1;
                AudioSource[] levelSources = levelMusic.GetComponents<AudioSource>();
                levelSource = levelSources[1];
                levelSources[randomMusic].UnPause();
            }

        //-----------------LEFT MOVEMENT------------------------
        if (Input.GetKey("a") && rb.velocity.magnitude <= 10 && groundTouch)
        {
            animator.SetFloat("Horizontal", -0.5f);
            rb.AddForce(-moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.acceleration.x < 0 && groundTouch)
        {
            animator.SetFloat("Horizontal", -0.5f);
            rb.AddForce(-moveSpeed * Time.deltaTime, 0, 0);
        }
        //----------------RIGHT MOVEMENT-------------------------
        if (Input.GetKey("d") && rb.velocity.magnitude <= 10 && groundTouch)
        {
            animator.SetFloat("Horizontal", 0.5f);
            rb.AddForce(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.acceleration.x > 0 && groundTouch)
        {
            animator.SetFloat("Horizontal", 0.5f);
            rb.AddForce(moveSpeed * Time.deltaTime, 0, 0);
        }

        //-----------------JUMP SCRIPT------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) && groundTouch)
        {
            Jump();
        }
    }

    public void Jump()
    {
        spaceClickedCounter++;
        if (groundTouch)
        {
            if(level == 1)
            {
                rb.AddForce(0, 500, 0);
                groundTouch = false;
            }
            if(level == 2)
            {
                rb.AddForce(0, 750, 0);
                groundTouch = false;
            }
        }
    }
    //-----------------Collision detections------------------------
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Floor" || collision.gameObject.name == "Obstacle" || collision.gameObject.name == "Floor_lvl2" || collision.gameObject.name == "Obstacle_lvl2")
        {
            groundTouch = true;
        }
        if (collision.gameObject.name == "Obstacle_not_touchable (1)" && groundTouch == false)
        {
            rb.AddForce(Random.Range(100, 250), Random.Range(50, 150), 0);
        }
        if (collision.gameObject.name == "Obstacle_not_touchable (2)" && groundTouch == false)
        {
            rb.AddForce(-Random.Range(100, 250), Random.Range(50, 150), 0);
        }
        if ((collision.gameObject.name == "Wall_1" || collision.gameObject.name == "Wall_1_lvl2") && groundTouch == false)
        {
            rb.AddForce(200, 250, 0);
        }
        if ((collision.gameObject.name == "Wall_2" || collision.gameObject.name == "Wall_2_lvl2") && groundTouch == false)
        {
            rb.AddForce(-200, 250, 0);
        }
        if (collision.gameObject.name == "CoinToLevel2" && level == 1)
        {
            GameObject.Find("Player").GetComponent<Floor_touch_count>().showHighscore = 0;
            groundTouchLevelInfo2.SetActive(true);
            groundTouch = false;
            rb.transform.position = new Vector3(40, 0, 0);
            level = 2;
            level2.SetActive(true);
            level1.SetActive(false);
        }
        if (collision.gameObject.name == "Winning_coin" && level == 2)
        {
            GameEndTime = Time.fixedTime;
            ShowStatsAfterGame();

            levelMusic = GameObject.Find("LevelMusic");
            AudioSource[] levelSources = levelMusic.GetComponents<AudioSource>();
            levelSource = levelSources[1];
            int randomMusic = rb.GetComponent<Floor_touch_count>().randomMusic;
            if(randomMusic != -1)
            {
                levelSources[randomMusic].Stop();
            }
            rb.AddForce(0, -1000, 0);
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(15f);
        SceneManager.UnloadScene("game_screen");
        SceneManager.LoadScene("start_screen", LoadSceneMode.Single);
    }

    void Stuck()
    {
        if (level == 1 && rb.transform.position.x >= 0)
        {
            rb.AddForce(Random.Range(100, 250), Random.Range(50, 150), 0);
        }
        if (level == 1 && rb.transform.position.x < 0)
        {
            rb.AddForce(-Random.Range(100, 250), Random.Range(50, 150), 0);
        }
        if (level == 2 && rb.transform.position.x >= 40)
        {
            rb.AddForce(Random.Range(100, 250), Random.Range(50, 150), 0);
        }
        if (level == 2 && rb.transform.position.x < 40)
        {
            rb.AddForce(-Random.Range(100, 250), Random.Range(50, 150), 0);
        }
        Cursor.visible = false;
        StuckTimer_Switch = true;
        Stuck_Button.SetActive(false);
        Stuck_Button_cancel.SetActive(false);
    }
    void CancelStuck()
    {
        Cursor.visible = false;
        StuckTimer_Switch = true;
        Stuck_Button.SetActive(false);
        Stuck_Button_cancel.SetActive(false);
    }

    void ShowStatsAfterGame()
    {
        int jumpperdeath = 0;
        float jumpperdeathrounded = 0;
        int jumpCounter = 0;
        string wynik;
        

        jumpCounter = rb.GetComponent<playerMovement>().spaceClickedCounter;
        jumpperdeath = rb.GetComponent<Floor_touch_count>().showPoints;
        jumpperdeath += rb.GetComponent<Floor_touch_count>().showPoints2;
        jumpperdeathrounded = (float)jumpCounter / (float)jumpperdeath;

        if(GameEndTime > 60)
        {
            GameEndTime = (GameEndTime - SummaryTimeOnPause) / 60;
            wynik = "You jumped: " + jumpCounter + " times!! This is " + jumpperdeathrounded + " jump per death. You did it just in " + (int)GameEndTime + " minutes";
        }
        else
        {
            wynik = "You jumped: " + jumpCounter + " times!! This is " + jumpperdeathrounded + " jump per death. You did it just in " + (int)GameEndTime + " seconds";
        }


        text_wynik.text = wynik;

        effectSource.PlayOneShot(winning);
        GameObject.Find("YOU_WON").GetComponent<Image>().enabled = true;
        spaceClickedCounterObject.SetActive(true);  
    }

}
