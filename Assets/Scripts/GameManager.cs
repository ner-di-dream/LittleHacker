using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 지금 진행하고 있는 스테이지 및 시나리오 갱신
    static public int currentScenario = 1;
    static public int currentStage = 1;
    static public int maxScenario = 3;
    static public int maxStaage = 15;
    static public int playerTurn = 0;
    static public float gridSize = 1;
    static public bool isClear = false;

    public GameObject Canvas; // 플레이어 화면보다 아래에 랜더링 되는 UI
    public GameObject UpperCanvas; // 플레이어 화면보다 위에 랜더링 되는 UI
    public GameObject UiCamera;
    public TMP_Text talkTextBox;

    public List<Dictionary<string, object>> deClear_Text;
    public List<Dictionary<string, object>> clear_Text;
    static public bool talkStart = false;
    public float textPrintSpeed = 0.1f;
    private List<string> currentTexts = new List<string>();
    private int currentTextCount = 0;
    private bool isPrint = false;
    private bool talkAnchor = false;

    public Player player;
    public MapCreate mapCreate;

    private void Awake()
    {
        currentScenario = 1;
        currentStage = 1;
        SpawnInitialized();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        mapCreate = transform.GetComponent<MapCreate>();
        isClear = false;

        deClear_Text = CSVReader.Read("SN_" + currentScenario.ToString());
        clear_Text   = CSVReader.Read("SN_" + currentScenario.ToString() + "_Clear");
    }

    private void Update()
    {
        if (talkStart)
        {
            if (!talkAnchor)
            {
                talkAnchor = true;
                player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.Initialized();
                TalkStart();
            }
            TexControl();
        }

        if (!talkStart && isClear) StageClear();
    }

    void SpawnInitialized()
    {
        Canvas canvas = Instantiate(Canvas).GetComponent<Canvas>();
        Instantiate(UpperCanvas).GetComponent<Canvas>().worldCamera = Camera.main;
        canvas.worldCamera = Instantiate(UiCamera).GetComponent<Camera>();
        talkTextBox = GameObject.Find("TalkText").GetComponent<TMP_Text>();
    }

    // 각각 스크립트 Initialized 함수를 모아서 실행시킬예정
    public void StageClear()
    {
        isClear = false;
        currentStage++;
        PlayerPrefs.SetInt("" + currentScenario.ToString() + "-" + currentStage.ToString(), 1);
        mapCreate.Initialize("SN_" + currentScenario.ToString() + "_ST_" + currentStage.ToString());
    }

    public void TalkStart()
    {
        if (!isClear)
        {
            for (int count = 0; count < deClear_Text.Count; count++)
            {
                if (deClear_Text[count]["Stage"].ToString() == currentStage.ToString())
                {
                    currentTexts.Add(deClear_Text[count]["Talk"].ToString());
                }
            }
        }
        else
        {
            for (int count = 0; count < clear_Text.Count; count++)
            {
                if (clear_Text[count]["Stage"].ToString() == currentStage.ToString())
                {
                    currentTexts.Add(clear_Text[count]["Talk"].ToString());
                }
            }
        }
        textPrintSpeed = 0.1f;
        if (currentTexts.Count != 0) StartCoroutine(printText());
        else
        {
            talkStart = false;
            talkAnchor = false;
        }
    }

    void TexControl()
    {
        if(player.playerTouch == Player.ETouchState.End)
        {
            if (isPrint)
            {
                textPrintSpeed = 0;
            }

            else
            {
                textPrintSpeed = 0.1f;
                ++currentTextCount;
                if (currentTextCount == currentTexts.Count)
                {
                    talkAnchor = false;
                    currentTexts.Clear();
                    currentTextCount = 0;
                    talkTextBox.text = "";
                    talkStart = false;
                }

                else
                {
                    StartCoroutine(printText());
                }
            }
        }
    }

    IEnumerator printText()
    {
        Debug.Log("start : " + currentTextCount.ToString());
        isPrint = true;
        talkTextBox.text = "";
        int count = 0;
        string text = currentTexts[currentTextCount].ToString();

        while(count != text.Length)
        {
            if(count < text.Length)
            {
                talkTextBox.text += text[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(textPrintSpeed);
        }

        isPrint = false;
    }
}
