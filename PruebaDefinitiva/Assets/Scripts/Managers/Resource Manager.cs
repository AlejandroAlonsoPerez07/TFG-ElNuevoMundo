using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    private int randomNumber;
    public Dictionary<Vector2, Tile> tilesWithSettlementsAndAdjacents = new Dictionary<Vector2, Tile>();
    List<Tile> adjacentTiles = new List<Tile>();

    [SerializeField] private GridManager gridManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private BuildingManager buildingManager;

    [SerializeField] public TMP_Text clayText;
    [SerializeField] public TMP_Text mountainText;
    [SerializeField] public TMP_Text wheatText;
    [SerializeField] public TMP_Text ironText;
    [SerializeField] public TMP_Text woolText;
    [SerializeField] public TMP_Text woodText;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

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
        Debug.Log("El numero aleatorio es: " + randomNumber);
        //Vector2 prueba = new Vector2(1,1);
        //Debug.Log("Diccionario: " + gridManager.tiles.ContainsKey(prueba));
        foreach (var entry in gridManager.tiles)
        {
            Vector2 key = entry.Key;
            Tile tile = entry.Value;
            if(playerManager.GetSettlementsPositions().Contains(key))
            {
                Debug.Log($"Clave: {key}, Tile: {tile.name}");
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
                    Vector2 adjacentKey = key + direction;

                    // Verificar si la posición adyacente existe en el diccionario
                    if (gridManager.tiles.ContainsKey(adjacentKey))
                    {
                        adjacentTiles.Add(gridManager.tiles[adjacentKey]);
                        Debug.Log($"Casilla adyacente: {adjacentKey}, Tile: {gridManager.tiles[adjacentKey].name}");
                    }
                }

                // Aquí puedes usar la lista `adjacentTiles` como desees
                Debug.Log($"Total de casillas adyacentes (incluyendo la actual): {adjacentTiles.Count}");
            }
        }


        foreach (var entry in adjacentTiles)
        {
            if(randomNumber == entry.randomNumber)
            {
                string type = entry.GetType().Name;
                switch (type)
                {
                    case "ClayTile":
                        Debug.Log("El tile es de tipo: ClayTile");
                        UpdateResourceCountClay();
                        break;

                    case "MountainTile":
                        Debug.Log("El tile es de tipo: MountainTile");
                        UpdateResourceCountMountain();
                        break;

                    case "WheatTile":
                        Debug.Log("El tile es de tipo: WheatTile");
                        UpdateResourceCountWheat();
                        break;

                    case "IronTile":
                        Debug.Log("El tile es de tipo: IronTile");
                        UpdateResourceCountIron();
                        break;

                    case "WoolTile":
                        Debug.Log("El tile es de tipo: WoolTile");
                        UpdateResourceCountWool();
                        break;

                    case "WoodTile":
                        Debug.Log("El tile es de tipo: WoodTile");
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
