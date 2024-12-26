using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int mapSize;
    public Tilemap tilemap;
    public Tile[] tiles;
    int[,] map;

    private void Start()
    {
        mapSize = GameManager.Instance.size;
        Debug.Log(mapSize);
        map = new int[mapSize, mapSize];
        //TileGenerator(mapSize, map);
    }
    /*
    public void TileGenerator(int mapSize, int[,] map)
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if (x == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[6]);
                }
                else if (y == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[6]);
                }
                else if (x == mapSize - 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[6]);
                }
                else if (y == mapSize - 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[6]);
                }
                else if(x == mapSize/2 && y == mapSize/2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[5]);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[TilePainter(TileNumber(mapSize))]);
                }
            }
        }
    }*/

    public int TilePainter(int[] tilesTypes)
    {
        int index;

        do
        {
            index = Random.Range(0, 5);
        } while (tilesTypes[index] == 0);

        tilesTypes[index]--;
        return index;
    }

    public int[] TileNumber(int mapSize)
    {
        if (mapSize == 7)
        {
            return new int[] { 5, 5, 5, 5, 4 };
        }else if (mapSize == 9)
        {
            return new int[] { 10, 10, 10, 9, 9 };
        }
        else
        {
            return new int[] { 16, 16, 16, 16, 16 };
        }
    }
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
