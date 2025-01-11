using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private BuildingManager buildingManager;

    [SerializeField] private TMP_Text clayText;
    [SerializeField] private TMP_Text mountainText;
    [SerializeField] private TMP_Text wheatText;
    [SerializeField] private TMP_Text ironText;
    [SerializeField] private TMP_Text woolText;
    [SerializeField] private TMP_Text woodText;

    private int randomNumber;
    public Dictionary<Vector2, Tile> tilesWithSettlementsAndAdjacents = new();
    List<Tile> adjacentTiles = new();

    void Awake()
    {
        Instance = this;
    }

    public void GenerateRandomNumber()
    {
        // Elegir aleatoriamente entre los dos rangos
        if (Random.value < 0.5f) // 50% de probabilidad
        {
            randomNumber = Random.Range(2, 7); // Rango 2-6 (el límite superior es exclusivo)
        }
        else
        {
            randomNumber = Random.Range(8, 13); // Rango 8-12
        }
        // ***********************************************************************************************************************
        // TODO: CAMBIAR LA FORMA DE LLAMAR Y GESTIONAR LOS RECURSOS, NO PUEDE HACERLO UNA FUNCION QUE GENERA UN NUMERO ALEATORIO
        UpdateResourceCount();
    }

    List<Tile> GetAdjacentsTiles()
    {
        foreach (var pos in playerManager.GetSettlementsPositions())
        {
            if (gridManager.tiles.ContainsKey(pos))
            {
                // Obtener las posiciones adyacentes (incluyendo la actual)
                Vector2[] directions = new Vector2[]
                {
                    new Vector2(0, 0),  // Incluir la posición actual
                    new Vector2(0, 1),  // Arriba
                    new Vector2(0, -1), // Abajo
                    new Vector2(1, 0),  // Derecha
                    new Vector2(-1, 0), // Izquierda
                    new Vector2(1, 1),  // Diagonal arriba-derecha
                    new Vector2(1, -1), // Diagonal abajo-derecha
                    new Vector2(-1, 1), // Diagonal arriba-izquierda
                    new Vector2(-1, -1) // Diagonal abajo-izquierda
                };

                foreach (Vector2 direction in directions)
                {
                    Vector2 adjacentKey = pos + direction;

                    // Verificar si la posición adyacente existe en el diccionario
                    if (gridManager.tiles.ContainsKey(adjacentKey))
                    {
                        adjacentTiles.Add(gridManager.tiles[adjacentKey]);
                    }
                }
            }
        }
        return adjacentTiles;
    }

    void UpdateResourceCount()
    {
        foreach (var entry in GetAdjacentsTiles())
        {
            if (randomNumber == entry.randomNumber)
            {
                string type = entry.GetType().Name;
                switch (type)
                {
                    case "ClayTile":
                        UpdateResourceCountClay();
                        break;

                    case "MountainTile":
                        UpdateResourceCountMountain();
                        break;

                    case "WheatTile":
                        UpdateResourceCountWheat();
                        break;

                    case "IronTile":
                        UpdateResourceCountIron();
                        break;

                    case "WoolTile":
                        UpdateResourceCountWool();
                        break;

                    case "WoodTile":
                        UpdateResourceCountWood();
                        break;

                    default:
                        Debug.Log("El tipo del tile no coincide con ninguno de los casos definidos.");
                        break;
                }
            }
        }
    }
    // Funciones de actualización
    void UpdateResourceCountClay()
    {
        int currentCount = int.Parse(clayText.text);
        clayText.text = (currentCount + 1).ToString();
    }

    void UpdateResourceCountMountain()
    {
        int currentCount = int.Parse(mountainText.text);
        mountainText.text = (currentCount + 1).ToString();
    }

    void UpdateResourceCountWheat()
    {
        int currentCount = int.Parse(wheatText.text);
        wheatText.text = (currentCount + 1).ToString();
    }

    void UpdateResourceCountIron()
    {
        int currentCount = int.Parse(ironText.text);
        ironText.text = (currentCount + 1).ToString();
    }

    void UpdateResourceCountWool()
    {
        int currentCount = int.Parse(woolText.text);
        woolText.text = (currentCount + 1).ToString();
    }

    void UpdateResourceCountWood()
    {
        int currentCount = int.Parse(woodText.text);
        woodText.text = (currentCount + 1).ToString();
    }
}
