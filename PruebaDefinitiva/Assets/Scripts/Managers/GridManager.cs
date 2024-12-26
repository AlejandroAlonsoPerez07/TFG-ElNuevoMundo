using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int width, height;
    [SerializeField] private Tile clayTile, desertTile, ironTile,
        mountainTile, waterTile, wheatTile, woodTile, woolTile; // objeto Tile
    [SerializeField] private Transform cam; // camara

    private Dictionary<Vector2, Tile> tiles;
    
    void Awake()
    {
        Instance = this;
        GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
    }

    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        width = GameManager.Instance.size;
        height = GameManager.Instance.size;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if ((x == 0 || x == width - 1) || (y == 0 || y == height - 1))
                {
                    var spawnedWaterTile = Instantiate(waterTile, new Vector3(x, y), Quaternion.identity);
                    spawnedWaterTile.Init(x, y);
                    tiles[new Vector2(x, y)] = spawnedWaterTile;
                }
                else if (x == width / 2 && y == height / 2) 
                {
                    var spawnedDesertTile = Instantiate(desertTile, new Vector3(x, y), Quaternion.identity);
                    spawnedDesertTile.Init(x, y);
                    tiles[new Vector2(x, y)] = spawnedDesertTile;
                }
                else
                {
                    var spawnedRandomTile = SpawnRandomTile(x, y);
                    spawnedRandomTile.Init(x, y);
                    tiles[new Vector2(x, y)] = spawnedRandomTile;
                }

                /*
                var randomTile = Random.Range(0, 6) == 3 ? mountainTile : grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}"; // Aqui vemos como se crean todas las casillas en la jerarqu�a

                spawnedTile.Init(x, y);


                tiles[new Vector2(x, y)] = spawnedTile;
                */
            }
        }

        // Con esto transformo la posici�n de la c�mara para que quede centrada en el mapa que haga
        // en funci�n de altura y anchura
        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -1);

        GameManager.Instance.UpdateGameState(GameManager.GameState.SpawnHeroes);
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if (tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }

        return null;
    }

    public Tile SpawnRandomTile(int x, int y)
    {
        int index = UnityEngine.Random.Range(0, 6);
        Tile spawnedRandomTile;
        switch (index)
        {
            case 0:
                return spawnedRandomTile = Instantiate(clayTile, new Vector3(x, y), Quaternion.identity);
            case 1:
                return spawnedRandomTile = Instantiate(ironTile, new Vector3(x, y), Quaternion.identity);
            case 2:
                return spawnedRandomTile = Instantiate(mountainTile, new Vector3(x, y), Quaternion.identity);
            case 3:
                return spawnedRandomTile = Instantiate(wheatTile, new Vector3(x, y), Quaternion.identity);
            case 4:
                return spawnedRandomTile = Instantiate(woodTile, new Vector3(x, y), Quaternion.identity);
            case 5:
                return spawnedRandomTile = Instantiate(woolTile, new Vector3(x, y), Quaternion.identity);
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Numero no valido, no se genero casilla");
        }
         
    }
}
