using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;


public class startScreen : MonoBehaviour
{
    //---------------MUSIC-------------------------
    private GameObject soundEffects;
    private GameObject mainMusic;
    private GameObject levelMusic;

    private AudioSource effectsSource;
    private AudioSource mainmenuSource;
    private AudioSource levelSource;

    private AudioClip mouseClick;
    private AudioClip winning;
    private AudioClip rageQuit;
    private AudioClip welcomeToHell;

    private int randomMusic = -1;

    [SerializeField] 
    private AudioClip[] musicClips;

    //----------------------------------------
    private GameObject start_button;
    private Button start_component;
    private GameObject options_button;
    private Button options_component;
    private GameObject quit_button;
    private Button quit_component;
    private GameObject back_button;
    private Button back_component;

    private GameObject noOptions;
    private GameObject mainCamera;

    private float screenWidth; //= Screen.width;
    private float screenHeith; //= Screen.height;
    private Vector3 skalowalne;
    private float screenScalar = 0.5f;
    private float backheightScalar = 0.145f;
    private float backwidthScalar = 0.125f;

    private float rotateStart = 0f;
    private bool rosnace = true;
    private bool backgroudCheck = true;
    private float timeToRotate;
    private Color start_but_color;
    private float backColor = 0.5f;

    private int typOczekiwania = 0; // 1 - start gry, 2 - wyjscie

    void Awake()
    {
        AudioListener.volume = 0.15f;

        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        mainCamera = GameObject.Find("Main Camera");

        timeToRotate = Time.fixedTime;
        noOptions = GameObject.Find("no_options_yet");
        noOptions.GetComponent<Image>().enabled = false;

        // ---------------------------AUDIO-Sound_Effects----------------------
        soundEffects = GameObject.Find("SoundEffectObject");
        AudioSource[] effectSources = soundEffects.GetComponents<AudioSource>();
        effectsSource = effectSources[1];
        mouseClick = effectSources[0].clip;
        winning = effectSources[1].clip;
        rageQuit = effectSources[2].clip;
        welcomeToHell = effectSources[3].clip;

        //-------Audio_MAIN_MENU_MUSIC
        mainMusic = GameObject.Find("MainScreenMusic");
        AudioSource[] mainSources = mainMusic.GetComponents<AudioSource>();
        mainmenuSource = mainSources[1];
        
        //-------Audio_Level_music
        levelMusic = GameObject.Find("LevelMusic");
        AudioSource[] levelSources = levelMusic.GetComponents<AudioSource>();
        levelSource = levelSources[1];


        //----------------RandomMainMenuMusic-------------------
        randomMusic = Random.Range(0, mainSources.Length);
        mainSources[randomMusic].Play();

        // ---------------------------Przypisanie Przycisków----------------------
        start_button = GameObject.Find("Start_Button");
        start_component = start_button.GetComponent<Button>();
        start_component.onClick.AddListener(StartGame);

        options_button = GameObject.Find("Options_Button");
        options_component = options_button.GetComponent<Button>();
        options_component.onClick.AddListener(Options);

        quit_button = GameObject.Find("Quit_Button");
        quit_component = quit_button.GetComponent<Button>();
        quit_component.onClick.AddListener(Quit);

        back_button = GameObject.Find("Back_Button");
        back_component = back_button.GetComponent<Button>();
        back_component.onClick.AddListener(Back);
        back_button.SetActive(false);

        // ---------------------------Skalowanie UI Main Menu----------------------
        screenWidth = Screen.width;
        screenHeith = Screen.height;
        skalowalne = new Vector3(screenWidth * 0.4f / 200, screenHeith * 0.3f / 100, 1.0f);
        start_button.GetComponent<RectTransform>().localScale = skalowalne;
        options_button.GetComponent<RectTransform>().localScale = skalowalne;
        quit_button.GetComponent<RectTransform>().localScale = skalowalne;
        back_button.GetComponent<RectTransform>().localScale = skalowalne;
        noOptions.GetComponent<RectTransform>().localScale = skalowalne;

    }

    void MenuMusicChanger()
    {


        int temp;
        mainMusic = GameObject.Find("MainScreenMusic");
        AudioSource[] mainSources = mainMusic.GetComponents<AudioSource>();
        mainmenuSource = mainSources[1];

        if (randomMusic != -1)
        {
            mainSources[randomMusic].Stop();
        }
        //temp = Random.Range(0, musicClips.Length);
        temp = Random.Range(0, mainSources.Length);
        while (temp == randomMusic)
        {
            temp = Random.Range(0, mainSources.Length);
            //temp = Random.Range(0, musicClips.Length);
        }
        randomMusic = temp;
        //mainSources[0].clip = musicClips[randomMusic]
        mainSources[randomMusic].Play();
    }

    void Update()
    {
        //screenWidth = Screen.width;
        //screenHeith = Screen.height;
        //skalowalne = new Vector3(screenWidth * 0.1f / 200, screenHeith * 0.1f / 100, 1.0f);
        //start_button.GetComponent<RectTransform>().localScale = skalowalne;
        //options_button.GetComponent<RectTransform>().localScale = skalowalne;
        //quit_button.GetComponent<RectTransform>().localScale = skalowalne;
        //back_button.GetComponent<RectTransform>().localScale = skalowalne;
        //noOptions.GetComponent<RectTransform>().localScale = skalowalne;
        //Debug.Log("Szerokość okna: " + screenWidth + "wyliczona szerokość obrazu: " + skalowalne);



        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            MenuMusicChanger();
        }
    }

    void FixedUpdate()
    {

        if (Time.fixedTime > timeToRotate)
        {
            if (backColor < 1f && backgroudCheck)
            {
                mainCamera.GetComponent<Camera>().backgroundColor = new Color(0.2f, backColor / 3, backColor / 2);
                backColor += 0.001f;
            }
            else
            {
                backgroudCheck = false;
            }
            if (!backgroudCheck)
            {
                mainCamera.GetComponent<Camera>().backgroundColor = new Color(backColor / 3, 0.2f, backColor / 2);
                backColor -= 0.001f;
                if (backColor <= 0.5f)
                {
                    backgroudCheck = true;
                }
            }

            if (rotateStart < 5f && rosnace)
            {
                start_button.GetComponent<RectTransform>().rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotateStart);
                rotateStart = rotateStart + 0.25f;
                if (rotateStart >= 5)
                {
                    rosnace = false;
                    StartColorChanger();
                }
            }
            if (!rosnace)
            {
                start_button.GetComponent<RectTransform>().rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotateStart);
                rotateStart = rotateStart - 0.25f;
                if (rotateStart <= -5f)
                {
                    rosnace = true;
                    StartColorChanger();
                }
            }
            timeToRotate = timeToRotate + 0.01f;
        }
    }

    void StartColorChanger()
    {
        start_but_color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 255);
        start_button.GetComponent<Image>().color = start_but_color;
    }

    void StartGame()
    {
        //dzwiek klika przeniesiony do waiter()
        start_button.SetActive(false);
        options_button.SetActive(false);
        quit_button.SetActive(false);
        back_button.SetActive(false);
        typOczekiwania = 1;
        StartCoroutine(waiter());

    }

    void Options()
    {
        effectsSource.PlayOneShot(mouseClick);
        start_button.SetActive(false);
        options_button.SetActive(false);
        quit_button.SetActive(false);
        back_button.SetActive(true);
        noOptions.GetComponent<Image>().enabled = true;
    }

    void Back()
    {
        effectsSource.PlayOneShot(mouseClick);
        start_button.SetActive(true);
        options_button.SetActive(true);
        quit_button.SetActive(true);
        back_button.SetActive(false);
        noOptions.GetComponent<Image>().enabled = false;
    }

    void Quit()
    {
        typOczekiwania = 2;
        StartCoroutine(waiter());
    }


    IEnumerator waiter()
    {
        // 1 - start gry, 2 - wyjscie
        if (typOczekiwania == 1)
        {
            int rand = Random.Range(1, 5);
            if (rand == 1)
            {
                levelSource.PlayOneShot(welcomeToHell);
                yield return new WaitForSecondsRealtime(2f);
                SceneManager.UnloadScene("start_screen");
                SceneManager.LoadScene("game_screen", LoadSceneMode.Single);
            }
            else
            {
                effectsSource.PlayOneShot(mouseClick);
                yield return new WaitForSecondsRealtime(0.5f);
                SceneManager.UnloadScene("start_screen");
                SceneManager.LoadScene("game_screen", LoadSceneMode.Single);
            }

        }
        else if (typOczekiwania == 2)
        {
            //RE-RE-RE-RAGE QUUUIT!!!
            int rand = Random.Range(1, 5);
            if (rand == 1)
            {
                mainmenuSource.PlayOneShot(rageQuit);
                yield return new WaitForSecondsRealtime(2.5f);
                Application.Quit();
            }
            else
            {
                effectsSource.PlayOneShot(mouseClick);
                yield return new WaitForSecondsRealtime(0.5f);
                Application.Quit();
            }
        }

    }
}
