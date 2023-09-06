using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public string filePath = @"C:\Bai tap HB_Academy\GitHub\Everything-Here\StackMaker_HoangPM\Assets\_Game\File_Map\Map1.txt";

    public float tileSize = 1f;

    private char[,] tiles;

    public GameObject wallPrefab;
    public GameObject brickPrefab;
    public GameObject finishPrefab;
    public GameObject railPrefab;
    public GameObject startPrefab;

    void Start()
    {
        ReadTextFile();

        GenerateMap();
    }

    void ReadTextFile()
    {
        string[] lines = File.ReadAllLines(filePath);

        int rows = lines.Length;
        int cols = lines[0].Length;

        tiles = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            char[] chars = lines[i].ToCharArray();
            
            for (int j = 0; j < cols; j++)
            {
                tiles[i, j] = chars[j];
            }
        }
    }

    void GenerateMap()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                char tile = tiles[i, j];

                Vector3 position = new Vector3(j * tileSize, 0, -i * tileSize);

                GameObject prefab;

                switch (tile)
                {
                    case '0':
                        prefab = wallPrefab;
                        break;
                    case '1':
                        prefab = brickPrefab;
                        break;
                    case '2':
                        prefab = railPrefab;
                        break;
                    case '3':
                        prefab = startPrefab;
                        break;
                    case '4':
                        prefab = finishPrefab;
                        break;
                    default:
                        prefab = null;
                        break;
                }

                if (prefab != null)
                {
                    Instantiate(prefab, position, Quaternion.identity);
                }
            }
        }
    }
}
