using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour
{
    GameObject optionSettingPanel;

    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) // 메인 게임 씬
        {
            GameObject.Find("OptionButton").GetComponent<Button>().onClick.AddListener(() => OptionButtonClick());
            optionSettingPanel = GameObject.Find("OptionSettingBox").transform.GetChild(0).gameObject;
        }

        else if(SceneManager.GetActiveScene().buildIndex == 0) // 초기 화면
        {
            GameObject.Find("OptionButton").GetComponent<Button>().onClick.AddListener(() => OptionButtonClick());
            optionSettingPanel = GameObject.Find("Option").transform.GetChild(0).gameObject;
            GameObject.Find("GameStartButton").GetComponent<Button>().onClick.AddListener(() => GameStartButtonClick());
        }
    }

    void OptionButtonClick()
    {
        if (!optionSettingPanel.activeSelf)
        {
            optionSettingPanel.SetActive(true);
            GameObject.Find("ExitButton").GetComponent<Button>().onClick.AddListener(() => OptionButtonClick());
        }
        else
        {
            optionSettingPanel.SetActive(false);
        }
    }
    
    void GameStartButtonClick()
    {
        SceneManager.LoadScene(1);
    }
}
