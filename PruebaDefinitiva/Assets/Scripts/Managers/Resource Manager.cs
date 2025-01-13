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
    [SerializeField] private TMP_Text displayDiceRoll;
    [SerializeField] private TMP_Text clayObtainedText;
    [SerializeField] private TMP_Text mountainObtainedText;
    [SerializeField] private TMP_Text wheatObtainedText;
    [SerializeField] private TMP_Text ironObtainedText;
    [SerializeField] private TMP_Text woolObtainedText;
    [SerializeField] private TMP_Text woodObtainedText;

    private int randomNumber;
    public Dictionary<Vector2, Tile> tilesWithSettlementsAndAdjacents = new();
    List<Tile> adjacentTiles = new();

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(GameManager.Instance.State == GameManager.GameState.BuildingPhase)
        {
            CleanAllLists();
            return;
        }
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
        displayDiceRoll.text = randomNumber.ToString();
        UpdateResourceCount();
        GameManager.Instance.UpdateGameState(GameManager.GameState.BuildingPhase);
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
        int clayCount = 0;
        int ironCount = 0;
        int stoneCount = 0;
        int wheatCount = 0;
        int woodCount = 0;
        int woolCount = 0;

        foreach (var entry in GetAdjacentsTiles())
        {
            if (randomNumber == entry.randomNumber)
            {
                string type = entry.GetType().Name;
                switch (type)
                {
                    case "ClayTile":
                        clayCount++;
                        UpdateResourceCountClay(clayCount);
                    break;

                    case "MountainTile":
                        stoneCount++;
                        UpdateResourceCountMountain(stoneCount);
                    break;

                    case "WheatTile":
                        wheatCount++;
                        UpdateResourceCountWheat(wheatCount);
                    break;

                    case "IronTile":
                        ironCount++;
                        UpdateResourceCountIron(ironCount);
                    break;

                    case "WoolTile":
                        woolCount++;
                        UpdateResourceCountWool(woolCount);
                    break;

                    case "WoodTile":
                        woodCount++;
                        UpdateResourceCountWood(woodCount);
                    break;

                    default:
                        Debug.Log("El tipo del tile no coincide con ninguno de los casos definidos.");
                    break;
                }
            }
        }
    }

    void CleanAllLists()
    {
        tilesWithSettlementsAndAdjacents.Clear();
        adjacentTiles.Clear();
    }
    // Funciones de actualización
    void UpdateResourceCountClay(int count)
    {
        int currentCount = int.Parse(clayText.text);
        clayText.text = (currentCount + 1).ToString();
        clayObtainedText.text = "+ " + count.ToString();
    }

    void UpdateResourceCountMountain(int count)
    {
        int currentCount = int.Parse(mountainText.text);
        mountainText.text = (currentCount + 1).ToString();
        mountainObtainedText.text = "+ " + count.ToString();
    }

    void UpdateResourceCountWheat(int count)
    {
        int currentCount = int.Parse(wheatText.text);
        wheatText.text = (currentCount + 1).ToString();
        wheatObtainedText.text = "+ " + count.ToString();
    }

    void UpdateResourceCountIron(int count)
    {
        int currentCount = int.Parse(ironText.text);
        ironText.text = (currentCount + 1).ToString();
        ironObtainedText.text = "+ " + count.ToString();
    }

    void UpdateResourceCountWool(int count)
    {
        int currentCount = int.Parse(woolText.text);
        woolText.text = (currentCount + 1).ToString();
        woolObtainedText.text = "+ " + count.ToString();
    }

    void UpdateResourceCountWood(int count)
    {
        int currentCount = int.Parse(woodText.text);
        woodText.text = (currentCount + 1).ToString();
        woodObtainedText.text = "+ " + count.ToString();
    }
}
