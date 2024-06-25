using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class MapCreate : MonoBehaviour
{
    [SerializeField]
    List<GameObject> renderObj = new List<GameObject>();

    private float mapX;
    private float mapY;

    public Player player;
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
        mapX = mapData.Walls[0].Count - 1;
        mapY = mapData.Walls.Count - 1;

        // 맵크기에 맞게 카메라 거리 설정
        AdjustCameraSize(mapX, mapY);

        renderPos = new Vector2(-GameManager.gridSize * (mapX / 2) + mapBox.transform.position.x, GameManager.gridSize * (mapY / 2) + mapBox.transform.position.y); // 

        for (int y = 0; y < mapData.Walls.Count; y++)
        {
            for (int x = 0; x < mapData.Walls[y].Count; x++)
            {
                GameObject prefab = mapData.Walls[y][x] == 1 ? renderObj[1] : renderObj[0];
                Instantiate(prefab, new Vector3(renderPos.x, renderPos.y, 0), Quaternion.identity, mapBox.transform);
                renderPos.x += GameManager.gridSize;
            }
            renderPos.x = -GameManager.gridSize * (mapX / 2);
            renderPos.y -= GameManager.gridSize;
        }

        renderPos = new Vector2(-GameManager.gridSize * (mapX / 2) + mapBox.transform.position.x, GameManager.gridSize * (mapY / 2) + mapBox.transform.position.y);

        for (int y = 0; y < mapData.Numbers.Count; y++)
        {
            for (int x = 0; x < mapData.Numbers[y].Count; x++)
            {
                if (!string.IsNullOrEmpty(mapData.Numbers[y][x]))
                {
                    GameObject numberObj = Instantiate(renderObj[3], new Vector3(renderPos.x, renderPos.y, -1), Quaternion.identity, mapBox.transform);
                    numberObj.GetComponent<ObjectData>().num = int.Parse(mapData.Numbers[y][x]);
                    numberObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mapData.Numbers[y][x];
                }
                if (!string.IsNullOrEmpty(mapData.Operators[y][x]))
                {
                    GameObject operatorObj;
                    switch (mapData.Operators[y][x]) {
                        case "+":
                            operatorObj = Instantiate(renderObj[4], new Vector3(renderPos.x, renderPos.y, -1), Quaternion.identity, mapBox.transform);
                            operatorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mapData.Operators[y][x];
                            operatorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = "+";
                            break;
                        case "-":
                            operatorObj = Instantiate(renderObj[5], new Vector3(renderPos.x, renderPos.y, -1), Quaternion.identity, mapBox.transform);
                            operatorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mapData.Operators[y][x];
                            operatorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = "-";
                            break;
                        case "x":
                            operatorObj = Instantiate(renderObj[6], new Vector3(renderPos.x, renderPos.y, -1), Quaternion.identity, mapBox.transform);
                            operatorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mapData.Operators[y][x];
                            operatorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = "x";
                            break;
                        case "/":
                            operatorObj = Instantiate(renderObj[7], new Vector3(renderPos.x, renderPos.y, -1), Quaternion.identity, mapBox.transform);
                            operatorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mapData.Operators[y][x];
                            operatorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = "/";
                            break;
                    }
                        
                    
                }
                renderPos.x += GameManager.gridSize;
            }
            renderPos.x = -GameManager.gridSize * (mapX / 2);
            renderPos.y -= GameManager.gridSize;
        }

        renderPos = new Vector2(-GameManager.gridSize * (mapX / 2) + mapBox.transform.position.x, GameManager.gridSize * (mapY / 2) + mapBox.transform.position.y);

        for (int y = 0; y < mapData.Boxes.Count; y++)
        {
            for (int x = 0; x < mapData.Boxes[y].Count; x++)
            {
                if(mapData.Boxes[y][x] == 1)
                {
                    GameObject prefab = renderObj[9];
                    Instantiate(prefab, new Vector3(renderPos.x, renderPos.y, -2), Quaternion.identity, mapBox.transform);
                }
                renderPos.x += GameManager.gridSize;
            }
            renderPos.x = -GameManager.gridSize * (mapX / 2);
            renderPos.y -= GameManager.gridSize;
        }

        renderPos = new Vector2(-GameManager.gridSize * (mapX / 2) + mapBox.transform.position.x, GameManager.gridSize * (mapY / 2) + mapBox.transform.position.y);

        Vector2 playerPosition = mapData.PlayerPosition;
        Instantiate(
            renderObj[2],
            new Vector3(renderPos.x + playerPosition.x * GameManager.gridSize, renderPos.y - playerPosition.y * GameManager.gridSize, -2),
            Quaternion.identity, mapBox.transform
        );

        renderPos = new Vector2(-GameManager.gridSize * (mapX / 2) + mapBox.transform.position.x, GameManager.gridSize * (mapY / 2) + mapBox.transform.position.y);

        Vector2 DoorPosition = mapData.DoorPosition;
        GameObject doorObj = Instantiate(
            renderObj[8],
            new Vector3(renderPos.x + DoorPosition.x * GameManager.gridSize, renderPos.y - DoorPosition.y * GameManager.gridSize, -2),
            Quaternion.identity, mapBox.transform
        );
        doorObj.GetComponent<ObjectData>().num = mapData.DoorValue;
        doorObj.transform.GetChild(0).GetComponent<TMP_Text>().text = mapData.DoorValue.ToString();

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
                tmpObj.GetComponent<ObjectData>().num = int.Parse(splitText[1].ToString());
                tmpObj.transform.GetChild(0).GetComponent<TMP_Text>().text = splitText[1];
                break;

            case 8:
                tmpObj = Instantiate(renderObj[int.Parse(splitText[0].ToString())], renderPos, Quaternion.identity, mapBox.transform);
                tmpObj.GetComponent<ObjectData>().doorNum = int.Parse(splitText[1].ToString());
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
