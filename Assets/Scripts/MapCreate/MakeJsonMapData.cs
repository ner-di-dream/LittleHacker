using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class MakeJsonMapData : MonoBehaviour
{   
    private string csvDirectoryPath;     // CSV 파일들이 있는 폴더 경로
    private string jsonDirectoryPath;    // JSON 파일들을 저장할 폴더 경로

    void Start()
    {
        csvDirectoryPath = "Assets/Resources/MapDatasCSV";
        jsonDirectoryPath = "Assets/Resources/MapDatasJSON";

        // InitializeJsonData(); // 존재하는 Json 파일들 지우기 (필요하면 주석 처리)

        // 디렉토리에서 모든 CSV 파일 목록을 가져오기
        string[] csvFiles = Directory.GetFiles(csvDirectoryPath, "*.csv");

        foreach (string csvFile in csvFiles)
        {
            // Debug.Log("csvFile: " + csvFile);

            // 파일의 전체 경로에서 파일 이름만 추출 (확장자 제외)
            string fileName = csvFile.Split('\\')[1].Split('.')[0];

            // Debug.Log("CSV File Name: " + fileName);

            // JSON 파일의 완전한 경로 생성
            string jsonFilePath = jsonDirectoryPath + "/" + fileName + ".json";

            // Debug.Log("JSON File Name: " + jsonFilePath);

            // SaveMapDataToJson(fileName, jsonFilePath); // Json 파일 생성
        }
    }

    // JSON 파일들이 저장된 폴더의 모든 파일을 지우기
    void InitializeJsonData()
    {
        
        if (Directory.Exists(jsonDirectoryPath))
        {
            string[] files = Directory.GetFiles(jsonDirectoryPath);
            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log("DoneFileDelete : " + file); // 적는게 느려서 확인하는 코드
            }
        }
    }
    
    void SaveMapDataToJson(string csvFilePath, string jsonFilePath)
    {
        // CSV 파일 읽기
        List<Dictionary<string, object>> csvData = CSVReader.Read(csvFilePath);

        // JSON으로 변환할 객체 생성
        MapData mapData = new MapData();
        mapData.Tiles = new List<TileData>();

        mapData.yLength = csvData.Count;
        mapData.xLength = csvData[0].Count;

        int rowIndex = 0;
        foreach (var row in csvData)
        {
            int colIndex = 0;
            foreach (var col in row)
            {
                string value = col.Value.ToString();

                if (value == "1")  // 벽
                {
                    mapData.Tiles.Add(new TileData(TileType.Wall, 0, colIndex, rowIndex));
                }
                else if (value == "0")  // 배경
                {
                    mapData.Tiles.Add(new TileData(TileType.Background, 0, colIndex, rowIndex));
                }
                else if (value.StartsWith("3_"))  // 숫자
                {
                    mapData.Tiles.Add(new TileData(TileType.Number, Convert.ToInt32(value.Split('_')[1]), colIndex, rowIndex));
                }
                else if (value.StartsWith("4_") || value.StartsWith("5_") || value.StartsWith("6_") || value.StartsWith("7_"))  // 연산자
                {
                    string oper = value.Split("_")[1];

                    if(oper.Equals("x"))
                    {
                        oper = "*";
                    }

                    mapData.Tiles.Add(new TileData(TileType.Operator, oper, colIndex, rowIndex));
                }
                else if (value == "2")  // 플레이어 위치
                {
                    mapData.Tiles.Add(new TileData(TileType.Player, colIndex, rowIndex));
                }
                else if(value.StartsWith("8_"))   // 문 위치
                {
                    mapData.Tiles.Add(new TileData(TileType.Door, Convert.ToInt32(value.Split('_')[1]), colIndex, rowIndex));
                }
                else if(value.StartsWith("9_"))     // 상자의 위치
                {
                    mapData.Tiles.Add(new TileData(TileType.Box, colIndex, rowIndex));
                }
                else if (value == "T")     // 트랩의 위치
                {
                    mapData.Tiles.Add(new TileData(TileType.Trap, colIndex, rowIndex));
                }
                else if(value.StartsWith("G_"))     // 게이트의 위치
                {
                    mapData.Tiles.Add(new TileData(TileType.Gate, Convert.ToInt32(value.Split('_')[1]), colIndex, rowIndex));

                }
                else if(value.StartsWith("A_"))
                {
                    mapData.Tiles.Add(new TileData(TileType.AllOper, Convert.ToInt32(value.Split('_')[2]), value.Split('_')[1], colIndex, rowIndex));
                }
                colIndex++;
            }

            rowIndex++;
        }

        // Json에서 Vector2는 재귀참조 문제가 생겨서 루프를 무시해줘야함
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    
        // JSON 파일로 저장
        string json = JsonConvert.SerializeObject(mapData, Formatting.Indented, settings);

        /* string json = JsonUtility.ToJson(mapData, true); 이거쓰고싶은데 뭔가 직렬화 안된다해서 안쓰는중입니다
         * json파일이 한줄로 쭉 출력되어서 안이쁘긴해요
         */

        File.WriteAllText(jsonFilePath, json);

        Debug.Log("DoneFileWrite : " + jsonFilePath); // 적는게 느려서 확인하는 코드
    }
}

/*
[System.Serializable]
public class MapData
{
    public List<List<int>> Walls { get; set; }
    public List<List<string>> Numbers { get; set; }
    public List<List<string>> Operators { get; set; }
    public List<List<int>> Boxes { get; set; }
    public List<List<string>> AllOperators { get; set; }
    public List<List<int>> Traps { get; set; }
    public List<List<string>> Gates { get; set; }
    public Vector2 PlayerPosition;
    public Vector2 DoorPosition;
    public int DoorValue;
}
*/

[System.Serializable]
public class MapData
{
    public List<TileData> Tiles { get; set; }
    public int xLength { get; set; }
    public int yLength { get; set; }
}

public class TileData
{
    public TileType type;
    public int value;
    public string oper;
    public int x;
    public int y;

    public TileData()
    {

    }

    public TileData(TileType type, int x, int y)
    {
        this.type = type;
        this.x = x;
        this.y = y;
    }

    public TileData(TileType type, int value, int x, int y)
    {
        this.type = type;
        this.value = value;
        this.x = x;
        this.y = y;
    }

    public TileData(TileType type, string oper, int x, int y)
    {
        this.type = type;
        this.oper = oper;
        this.x = x;
        this.y = y;
    }

    public TileData(TileType type, int value, string oper, int x, int y)
    {
        this.type = type;
        this.value = value;
        this.oper = oper;
        this.x = x;
        this.y = y;
    }
}

public enum TileType
{
    Background = 0,
    Wall = 1,
    Player = 2,
    Number = 3,
    Operator = 4,
    Door = 8,
    Box = 9,
    Trap,
    Gate,
    AllOper,
}