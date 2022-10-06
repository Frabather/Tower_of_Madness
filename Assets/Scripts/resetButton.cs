using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class resetButton : MonoBehaviour
{
    private GameObject reset_button;
    private GameObject backtomain_button;
    private Button reset_component;
    private Button backtomain_component;

    private GameObject mainCamera;
    private AudioSource source;
    private AudioClip mouseClick;
    public int typOczekiwania = 0; // 1 - restart, 2 - do menu

    void Awake()
    {
        Time.timeScale = 1;

        mainCamera = GameObject.Find("Main Camera");
        AudioSource[] audioSources = mainCamera.GetComponents<AudioSource>();
        source = audioSources[0];
        mouseClick = audioSources[0].clip;

        reset_button = GameObject.Find("Reset_Button");
        backtomain_button = GameObject.Find("Exit_to_main_menu");

        reset_component = reset_button.GetComponent<Button>();
        backtomain_component = backtomain_button.GetComponent<Button>();

        reset_component.onClick.AddListener(Reset);
        backtomain_component.onClick.AddListener(BackToMain);

        reset_button.SetActive(false);
        backtomain_button.SetActive(false);
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            reset_button.SetActive(true);
            backtomain_button.SetActive(true);
        }
        else
        {
            reset_button.SetActive(false);
            backtomain_button.SetActive(false);
        }

    }

    void Reset()
    {
        source.PlayOneShot(mouseClick);
        typOczekiwania = 1;
        StartCoroutine(waiter());
        
    }

    void BackToMain()
    {
        source.PlayOneShot(mouseClick);
        typOczekiwania = 2;
        reset_button.SetActive(false);
        backtomain_button.SetActive(false);
        StartCoroutine(waiter());
        
    }


    IEnumerator waiter()
    {
        // 1 - restart, 2 - do menu
        if (typOczekiwania == 1)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            Application.LoadLevel(Application.loadedLevel);
            Time.timeScale = 1;
        }
        else if (typOczekiwania == 2)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            SceneManager.UnloadScene("game_screen");
            SceneManager.LoadScene("start_screen", LoadSceneMode.Single);
        }

    }

}
