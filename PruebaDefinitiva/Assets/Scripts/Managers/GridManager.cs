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

    // parte nueva
    [SerializeField] private GameObject gridVisualization;

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
        gridVisualization.SetActive(true);
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if ((x == 0 || x == width - 1) || (z == 0 || z == height - 1))
                {
                    // Al multiplicar por 2 los valores de (x,y) mantienes la escala
                    var spawnedWaterTile = Instantiate(waterTile, new Vector3(x*2, 0, z*2), Quaternion.identity);
                    spawnedWaterTile.transform.localScale = new Vector3(2f, 2f, 1f); // Modifica el tamaño de la casilla
                    spawnedWaterTile.transform.rotation = Quaternion.Euler(90, 0, 0); // Rota las casillas
                    spawnedWaterTile.Init(x, z);
                    tiles[new Vector2(x, z)] = spawnedWaterTile;
                }
                else if (x == width / 2 && z == height / 2) 
                {
                    var spawnedDesertTile = Instantiate(desertTile, new Vector3(x*2, 0, z *2), Quaternion.identity);
                    spawnedDesertTile.transform.localScale = new Vector3(2f, 2f, 1f);
                    spawnedDesertTile.transform.rotation = Quaternion.Euler(90, 0, 0);
                    spawnedDesertTile.Init(x, z);
                    tiles[new Vector2(x, z)] = spawnedDesertTile;
                }
                else
                {
                    var spawnedRandomTile = SpawnRandomTile(x*2, z*2);
                    spawnedRandomTile.transform.localScale = new Vector3(2f, 2f, 1f);
                    spawnedRandomTile.transform.rotation = Quaternion.Euler(90, 0, 0);
                    spawnedRandomTile.Init(x, z);
                    tiles[new Vector2(x, z)] = spawnedRandomTile;
                }
            }
        }

        // Con esto transformo la posicion de la camara para que quede centrada en el mapa que haga
        // en funcion de altura y anchura
        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -1);

        GameManager.Instance.UpdateGameState(GameManager.GameState.SpawnHeroes);
        Debug.Log("avanzo a SpawnHeroes: " + GameManager.Instance.State);
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if (tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }

        return null;
    }

    public Tile SpawnRandomTile(int x, int z)
    {
        int index = UnityEngine.Random.Range(0, 6);
        Tile spawnedRandomTile;
        switch (index)
        {
            case 0:
                return spawnedRandomTile = Instantiate(clayTile, new Vector3(x, 0, z), Quaternion.identity);
            case 1:
                return spawnedRandomTile = Instantiate(ironTile, new Vector3(x, 0, z), Quaternion.identity);
            case 2:
                return spawnedRandomTile = Instantiate(mountainTile, new Vector3(x, 0, z), Quaternion.identity);
            case 3:
                return spawnedRandomTile = Instantiate(wheatTile, new Vector3(x, 0, z), Quaternion.identity);
            case 4:
                return spawnedRandomTile = Instantiate(woodTile, new Vector3(x, 0, z), Quaternion.identity);
            case 5:
                return spawnedRandomTile = Instantiate(woolTile, new Vector3(x, 0, z), Quaternion.identity);
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Numero no valido, no se genero casilla");
        }
         
    }
}
