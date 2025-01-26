using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private GameManager gameManager;

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

    [SerializeField] private Image displayPlayerColor;
    [SerializeField] private TMP_Text displayPlayerPoints;

    private int randomNumber, playerIndex;
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
        GetAdjacentsTiles();
        GameManager.Instance.UpdateGameState(GameManager.GameState.BuildingPhase);
    }

    void GetAdjacentsTiles()
    {
        foreach (var pos in buildingManager.allPlacedBuildingsPositions) // Se recorren todos los edificios sin importar el jugador
        {
            for(int i = 0; i < playerManager.playerList.Count; i++)
            {
                if (playerManager.playerList[i].settlementsPositions.Contains(pos))
                {
                    playerIndex = i;
                    Debug.Log("playerIndex en resourceManager: " + playerIndex);
                    if (gridManager.tiles.ContainsKey(pos))
                    {
                        Debug.Log("detecto la casilla con la ciudad en la pos: " + pos);
                        for(int x = (int)pos.x - 1; x <= pos.x + 1; x++) 
                        {
                            for(int z = (int)pos.y - 1; z <= pos.y + 1; z++)
                            {
                                Vector2 adjacentTile = new Vector2(x, z);
                                Debug.Log("Todas las casillas adyacentes: " + adjacentTile);
                                if (gridManager.tiles[adjacentTile].randomNumber == randomNumber)
                                {
                                    Debug.Log("añado la casilla adyacente con el numero que ha salido");
                                    adjacentTiles.Add(gridManager.tiles[adjacentTile]);
                                    Debug.Log("casillas con el numero que ha salido: " + adjacentTiles.Count);
                                }    
                            }
                        }
                    }
                    UpdateResourceCount(adjacentTiles);
                    adjacentTiles.Clear();
                }
            }
        }
    }

    void UpdateResourceCount(List<Tile> adjacentTiles)
    {
        int clayCount = 0;
        int ironCount = 0;
        int stoneCount = 0;
        int wheatCount = 0;
        int woodCount = 0;
        int woolCount = 0;
        Debug.Log("dentro de updateResourceCount jugador actual: " + playerIndex);
        Debug.Log("dentro de updateResourceCount casillas con el numero que ha salido: " + adjacentTiles.Count);
        foreach (var entry in adjacentTiles)
        {
            string type = entry.GetType().Name;
            switch (type)
            {
                case "ClayTile":
                    clayCount++;
                    UpdateResourceCountClay(clayCount, playerIndex);
                break;

                case "MountainTile":
                    stoneCount++;
                    UpdateResourceCountMountain(stoneCount, playerIndex);
                break;

                case "WheatTile":
                    wheatCount++;
                    UpdateResourceCountWheat(wheatCount, playerIndex);
                break;

                case "IronTile":
                    ironCount++;
                    UpdateResourceCountIron(ironCount, playerIndex);
                break;

                case "WoolTile":
                    woolCount++;
                    UpdateResourceCountWool(woolCount, playerIndex);
                break;

                case "WoodTile":
                    woodCount++;
                    UpdateResourceCountWood(woodCount, playerIndex);
                break;

                default:
                    Debug.Log("El tipo del tile no coincide con ninguno de los casos definidos.");
                break;
            }
        }
    }

    void CleanAllLists()
    {
        tilesWithSettlementsAndAdjacents.Clear();
        adjacentTiles.Clear();
    }
    // Funciones de actualización
    void UpdateResourceCountClay(int count, int playerIndex)
    {
        int currentCount = playerManager.playerList[playerIndex].resources[0];
        clayText.text = (currentCount + 1).ToString();
        clayObtainedText.text = "+ " + count.ToString();
        playerManager.playerList[playerIndex].resources[0] += 1;
    }

    void UpdateResourceCountMountain(int count, int playerIndex)
    {
        int currentCount = playerManager.playerList[playerIndex].resources[2];
        mountainText.text = (currentCount + 1).ToString();
        mountainObtainedText.text = "+ " + count.ToString();
        playerManager.playerList[playerIndex].resources[2] += 1;
    }

    void UpdateResourceCountWheat(int count, int playerIndex)
    {
        int currentCount = playerManager.playerList[playerIndex].resources[3];
        wheatText.text = (currentCount + 1).ToString();
        wheatObtainedText.text = "+ " + count.ToString();
        playerManager.playerList[playerIndex].resources[3] += 1;
    }

    void UpdateResourceCountIron(int count, int playerIndex)
    {
        int currentCount = playerManager.playerList[playerIndex].resources[1];
        ironText.text = (currentCount + 1).ToString();
        ironObtainedText.text = "+ " + count.ToString();
        playerManager.playerList[playerIndex].resources[1] += 1;
    }

    void UpdateResourceCountWool(int count, int playerIndex)
    {
        int currentCount = playerManager.playerList[playerIndex].resources[5];
        woolText.text = (currentCount + 1).ToString();
        woolObtainedText.text = "+ " + count.ToString();
        playerManager.playerList[playerIndex].resources[5] += 1;
    }

    void UpdateResourceCountWood(int count, int playerIndex)
    {
        int currentCount = playerManager.playerList[playerIndex].resources[4];
        woodText.text = (currentCount + 1).ToString();
        woodObtainedText.text = "+ " + count.ToString();
        playerManager.playerList[playerIndex].resources[4] += 1;
    }

    public void LoadResourcesOnInterface(int currentPlayer)
    {
        playerIndex = currentPlayer - 1;
        Debug.Log("Estoy en cargar recursos: " + playerIndex);
        clayText.text = playerManager.playerList[playerIndex].resources[0].ToString();
        ironText.text = playerManager.playerList[playerIndex].resources[1].ToString();
        mountainText.text = playerManager.playerList[playerIndex].resources[2].ToString();
        wheatText.text = playerManager.playerList[playerIndex].resources[3].ToString();
        woodText.text = playerManager.playerList[playerIndex].resources[4].ToString();
        woolText.text = playerManager.playerList[playerIndex].resources[5].ToString();

        displayPlayerColor.color = playerManager.playerList[playerIndex].playerColor;
        displayPlayerPoints.text = playerManager.playerList[playerIndex].totalPoints.ToString() + " pts";
    }
}
