using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class MakeJsonMapData : MonoBehaviour
{   
    private string csvDirectoryPath;     // CSV ���ϵ��� �ִ� ���� ���
    private string jsonDirectoryPath;    // JSON ���ϵ��� ������ ���� ���

    void Start()
    {
        csvDirectoryPath = "Assets/Resources/MapDatasCSV";
        jsonDirectoryPath = "Assets/Resources/MapDatasJSON";

        // InitializeJsonData(); // �����ϴ� Json ���ϵ� ����� (�ʿ��ϸ� �ּ� ó��)

        // ���丮���� ��� CSV ���� ����� ��������
        string[] csvFiles = Directory.GetFiles(csvDirectoryPath, "*.csv");

        foreach (string csvFile in csvFiles)
        {
            // Debug.Log("csvFile: " + csvFile);

            // ������ ��ü ��ο��� ���� �̸��� ���� (Ȯ���� ����)
            string fileName = csvFile.Split('\\')[1].Split('.')[0];

            // Debug.Log("CSV File Name: " + fileName);

            // JSON ������ ������ ��� ����
            string jsonFilePath = jsonDirectoryPath + "/" + fileName + ".json";

            // Debug.Log("JSON File Name: " + jsonFilePath);

            // SaveMapDataToJson(fileName, jsonFilePath); // Json ���� ����
        }
    }

    // JSON ���ϵ��� ����� ������ ��� ������ �����
    void InitializeJsonData()
    {
        
        if (Directory.Exists(jsonDirectoryPath))
        {
            string[] files = Directory.GetFiles(jsonDirectoryPath);
            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log("DoneFileDelete : " + file); // ���°� ������ Ȯ���ϴ� �ڵ�
            }
        }
    }
    
    void SaveMapDataToJson(string csvFilePath, string jsonFilePath)
    {
        // CSV ���� �б�
        List<Dictionary<string, object>> csvData = CSVReader.Read(csvFilePath);

        // JSON���� ��ȯ�� ��ü ����
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

                if (value == "1")  // ��
                {
                    mapData.Tiles.Add(new TileData(TileType.Wall, 0, colIndex, rowIndex));
                }
                else if (value == "0")  // ���
                {
                    mapData.Tiles.Add(new TileData(TileType.Background, 0, colIndex, rowIndex));
                }
                else if (value.StartsWith("3_"))  // ����
                {
                    mapData.Tiles.Add(new TileData(TileType.Number, Convert.ToInt32(value.Split('_')[1]), colIndex, rowIndex));
                }
                else if (value.StartsWith("4_") || value.StartsWith("5_") || value.StartsWith("6_") || value.StartsWith("7_"))  // ������
                {
                    string oper = value.Split("_")[1];

                    if(oper.Equals("x"))
                    {
                        oper = "*";
                    }

                    mapData.Tiles.Add(new TileData(TileType.Operator, oper, colIndex, rowIndex));
                }
                else if (value == "2")  // �÷��̾� ��ġ
                {
                    mapData.Tiles.Add(new TileData(TileType.Player, colIndex, rowIndex));
                }
                else if(value.StartsWith("8_"))   // �� ��ġ
                {
                    mapData.Tiles.Add(new TileData(TileType.Door, Convert.ToInt32(value.Split('_')[1]), colIndex, rowIndex));
                }
                else if(value.StartsWith("9_"))     // ������ ��ġ
                {
                    mapData.Tiles.Add(new TileData(TileType.Box, colIndex, rowIndex));
                }
                else if (value == "T")     // Ʈ���� ��ġ
                {
                    mapData.Tiles.Add(new TileData(TileType.Trap, colIndex, rowIndex));
                }
                else if(value.StartsWith("G_"))     // ����Ʈ�� ��ġ
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

        // Json���� Vector2�� ������� ������ ���ܼ� ������ �����������
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    
        // JSON ���Ϸ� ����
        string json = JsonConvert.SerializeObject(mapData, Formatting.Indented, settings);

        /* string json = JsonUtility.ToJson(mapData, true); �̰ž�������� ���� ����ȭ �ȵȴ��ؼ� �Ⱦ������Դϴ�
         * json������ ���ٷ� �� ��µǾ ���̻ڱ��ؿ�
         */

        File.WriteAllText(jsonFilePath, json);

        Debug.Log("DoneFileWrite : " + jsonFilePath); // ���°� ������ Ȯ���ϴ� �ڵ�
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