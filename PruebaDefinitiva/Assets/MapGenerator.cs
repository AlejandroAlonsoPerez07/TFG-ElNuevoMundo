using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int mapSize = 7;
    public Tilemap tilemap;
    public Tile[] tiles;
    int[,] map;
    int[] tilesTypes = new int[] { 5, 5, 5, 5, 4 };


    private void Start()
    {
        map = new int[7,7];
        TileGenerator(7, map);
    }

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
                else if (x == 6)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[6]);
                }
                else if (y == 6)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[6]);
                }
                else if(x == 3 && y == 3)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[5]);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[TilePainter(tilesTypes)]);
                }
            }
        }
    }

    public int TilePainter(int[] tilesTypes)
    {
        int index;

        do
        {
            index = Random.Range(0, tilesTypes.Length);
        } while (tilesTypes[index] == 0);

        tilesTypes[index]--;
        return index;
    }
}
