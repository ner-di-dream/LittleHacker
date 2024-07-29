using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class MapCreate : MonoBehaviour
{
    [SerializeField]
    List<GameObject> renderObj = new List<GameObject>();
    public GameObject renderTile;

    private float mapX;
    private float mapY;

    public Player player;
    public GameManager gameManager;
    private Vector2 renderPos; // 렌더링 할 좌표로 이용
    private GameObject mapBox; // map 오브젝트 내에 요소를 담기위한 박스
    private string stageInfo;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SN"))
        {
            GameManager.currentScenario = PlayerPrefs.GetInt("SN");
            GameManager.currentStage = PlayerPrefs.GetInt("ST");
            stageInfo = "SN_" + GameManager.currentScenario.ToString() + "_ST_" + GameManager.currentStage.ToString();
            Initialize(stageInfo);
        }
        else
        {
            stageInfo = "SN_" + GameManager.currentScenario.ToString() + "_ST_" + GameManager.currentStage.ToString();
            Initialize(stageInfo);
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.currentStage++;
            stageInfo = "SN_" + GameManager.currentScenario.ToString() + "_ST_" + GameManager.currentStage.ToString();

            Initialize(stageInfo);
            player.Initialized();
        }
        if (Input.GetKeyDown(KeyCode.N) && GameManager.currentStage > 1)
        {
            GameManager.currentStage--;
            stageInfo = "SN_" + GameManager.currentScenario.ToString() + "_ST_" + GameManager.currentStage.ToString();

            Initialize(stageInfo);
            player.Initialized();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            stageInfo = "SN_" + GameManager.currentScenario.ToString() + "_ST_" + GameManager.currentStage.ToString();

            Initialize(stageInfo);
            player.Initialized();
        }
    }

    // Json파일 가져오기
    public void Initialize(string stageJsonData)
    {
        TextAsset jsonData = Resources.Load<TextAsset>("MapDatasJSON/" + stageJsonData); // Resources -> MapDatasJson내에 이름으로 접근

        if (jsonData == null)
        {
            Debug.LogError("Failed to load map data!");
            return;
        }

        MapData mapData = JsonConvert.DeserializeObject<MapData>(jsonData.text); // mapData 불러오기
        if (mapData == null)
        {
            Debug.LogError("Failed to parse map data!");
            return;
        }

        RenderMap(mapData);
    }

    private void RenderMap(MapData mapData)
    {
        mapBox = GameObject.Find("Maps"); // 오브젝트를 담을 Maps선언

        // Maps 초기화 (모든 오브젝트 삭제)
        foreach (Transform child in mapBox.transform)
        {
            Destroy(child.gameObject);
        }

        // 맵의 x, y크기 가져오기
        mapX = mapData.xLength;
        mapY = mapData.yLength;

        // 맵크기에 맞게 카메라 거리 설정
        AdjustCameraSize(mapX, mapY);

        // 모든 오브젝트를 통합하여 렌더링
        foreach (TileData tile in mapData.Tiles)
        {
            if (tile.type == TileType.Player)
            {
                renderPos = new Vector2(GameManager.gridSize * (-mapX / 2 + tile.x + 0.5f), GameManager.gridSize * (mapY / 2 - tile.y));
                GameObject clone = Instantiate(renderObj[2], new Vector3(renderPos.x, renderPos.y, 0), Quaternion.identity, mapBox.transform);
                clone.tag = "Player";
                clone.layer = 6; // Player
            }
            else
            {
                renderPos = new Vector2(GameManager.gridSize * (-mapX / 2 + tile.x + 0.5f), GameManager.gridSize * (mapY / 2 - tile.y));
                GameObject clone = Instantiate(renderTile, new Vector3(renderPos.x, renderPos.y, 0), Quaternion.identity, mapBox.transform);
                
                if(tile.type == TileType.Gate) 
                { 
                    clone.AddComponent<GateScript>();
                }
                if (tile.type == TileType.AllOper)
                {
                    clone.AddComponent<AllOperatorScript>();
                }

                clone.GetComponent<ObjectData>().tileData = tile;
            }
        }

        // (미사용) 렌더링 기준점 초기화
        // renderPos = new Vector2(-GameManager.gridSize * (mapX / 2), GameManager.gridSize * (mapY / 2));

        // 플레이어 대화 시작
        GameManager.talkStart = true;
    }

    // 맵의 해상도를 맞추는 임시함수
    private void AdjustCameraSize(float mapWidth, float mapHeight)
    {
        Camera mainCamera = Camera.main;
        Camera uiCamera = GameObject.FindWithTag("UiCamera").GetComponent<Camera>();

        // 카메라의 Size를 맵의 최대 길이에 맞게 설정
        mainCamera.orthographicSize = Mathf.Max(mapWidth, mapHeight) + 2;
        uiCamera.orthographicSize = Mathf.Max(mapWidth, mapHeight) + 2;
    }

    // 해당 함수는 사용하지 않는 듯? 일단 남김
    void MapSuvCreate(string[] splitText)
    {
        GameObject tmpObj;
        string[] suvTmpText = new string[3];
        switch (int.Parse(splitText[0].ToString()))
        {
            case 2:
                Instantiate(renderObj[int.Parse(splitText[0].ToString())], renderPos, Quaternion.identity);
                break;
            case 3:
                tmpObj = Instantiate(renderObj[int.Parse(splitText[0].ToString())], renderPos, Quaternion.identity, mapBox.transform);
                tmpObj.GetComponent<ObjectData>().tileData.value = int.Parse(splitText[1].ToString());
                tmpObj.transform.GetChild(0).GetComponent<TMP_Text>().text = splitText[1];
                break;

            case 8:
                tmpObj = Instantiate(renderObj[int.Parse(splitText[0].ToString())], renderPos, Quaternion.identity, mapBox.transform);
                tmpObj.GetComponent<ObjectData>().tileData.value = int.Parse(splitText[1].ToString());
                tmpObj.transform.GetChild(0).GetComponent<TMP_Text>().text = splitText[1];
                break;

            case 9:
                tmpObj = Instantiate(renderObj[int.Parse(splitText[0].ToString())], renderPos, Quaternion.identity, mapBox.transform);
                if (splitText.Length > 1)
                {
                    suvTmpText.CopyTo(splitText, 1);
                    MapSuvCreate(suvTmpText);
                }
                break;

            default:
                Instantiate(renderObj[int.Parse(splitText[0].ToString())], renderPos, Quaternion.identity, mapBox.transform);
                break;
        }
    }
}
