using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 지금 진행하고 있는 스테이지 및 시나리오 갱신
    static public int currentScenario = 1;
    static public int currentStage = 1;
    static public int maxScenario = 3;
    static public int maxStaage = 15;
    static public int playerTurn = 0;
    static public float gridSize = 1;

    public GameObject Canvas; // 플레이어 화면보다 아래에 랜더링 되는 UI
    public GameObject UpperCanvas; // 플레이어 화면보다 위에 랜더링 되는 UI
    public GameObject UiCamera;

    public Player player;
    public MapCreate mapCreate;

    private void Awake()
    {
        SpawnInitialized();
        currentScenario = 1;
        currentStage = 1;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        mapCreate = transform.GetComponent<MapCreate>();
    }

    void SpawnInitialized()
    {
        Canvas canvas = Instantiate(Canvas).GetComponent<Canvas>();
        Instantiate(UpperCanvas).GetComponent<Canvas>().worldCamera = Camera.main;
        canvas.worldCamera = Instantiate(UiCamera).GetComponent<Camera>();
    }

    // 각각 스크립트 Initialized 함수를 모아서 실행시킬예정
    public void StageClear()
    {
        currentStage++;
        PlayerPrefs.SetInt("" + currentScenario.ToString() + "-" + currentStage.ToString(), 1);
        mapCreate.Initialize("SN_" + currentScenario.ToString() + "_ST_" + currentStage.ToString());
        player.Initialized();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}
