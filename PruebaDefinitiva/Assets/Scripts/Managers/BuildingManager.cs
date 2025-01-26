using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.PlayerSettings;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [SerializeField] GameObject mouseIndicator;
    [SerializeField] BuildingsDatabaseOS dataBase;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;
    [SerializeField] private PreviewSystem preview;

    private GridData floorData, furnitureData;
    private int selectedObjectIndex = -1;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    public List<GameObject> placedGameObjects = new();
    public List<Vector2> placedGameObjectsPositions = new();
    public List<Vector2> allPlacedBuildingsPositions = new(); // Aqui se guardan todas las posiciones de todos los edificios colocados por todos los jugadores

    private int placedObjectsCount;
    private int playerIndex;
    private float currentRotation;

    // Gestion de recursos
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerManager playerManager;

    [SerializeField] private TMP_Text clayText;
    [SerializeField] private TMP_Text ironText;
    [SerializeField] private TMP_Text mountainText;
    [SerializeField] private TMP_Text wheatText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text woolText;
    [SerializeField] private TMP_Text displayPlayerPoints;

    [SerializeField] private Button railButton, villageButton, cityButton, diceRollButton, passTurnButton;

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();
        CheckResourcesAmount();
    }
    private void FixedUpdate()
    {
        CheckResourcesAmount();
        if (GameManager.Instance.State == GameManager.GameState.FirstTurn) 
        {
            Debug.Log("Estoy actualizando en el primer turno");
            if (placedObjectsCount == 2)
            {
                passTurnButton.enabled = true;
                Debug.Log("Se han añadido dos estructuras");
                GameManager.Instance.UpdateGameState(GameManager.GameState.EndTurn);
                return;
            }
        }
    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = dataBase.buildingData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(dataBase.buildingData[selectedObjectIndex].prefab,
            dataBase.buildingData[selectedObjectIndex].Size);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        inputManager.OnRotate += RotateBuilding;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;
        //CheckResourcesAmount();
        GameObject newObject = Instantiate(dataBase.buildingData[selectedObjectIndex].prefab);
        Vector3 cellCenterPosition = grid.GetCellCenterWorld(gridPosition); // obtenemos el centro de la casilla correspondiente a sus coordenadas del mundo
        Debug.Log("La posicion del objeto colocado: " + cellCenterPosition.y);
        cellCenterPosition.y = 0.3f;
        newObject.transform.position = cellCenterPosition;
        newObject.layer = LayerMask.NameToLayer("Building");
        newObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        ApplyPlayerColorOnBuildings(newObject, playerManager.playerList[playerIndex].playerColor);
        //newObject.GetComponent<Renderer>().material.color = playerManager.playerList[playerIndex].playerColor;
        placedGameObjects.Add(newObject);
        Debug.Log("placedGameObjects: " + placedGameObjects.Count);
        GridData selectedData = dataBase.buildingData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        
        GetplacedGameObjectsPositions();
        Debug.Log("Mis objetos colocados: " + selectedData.ToString());
        /*
        selectedData.AddObjectAt(gridPosition,
            dataBase.buildingData[selectedObjectIndex].Size,
            dataBase.buildingData[selectedObjectIndex].ID,
            placedGameObjects.Count - 1);
        */
        Debug.Log("placedGameObjects: " + placedGameObjects.Count);
        Debug.Log("Antes de entrar a descontar");
        preview.UpdatePosition(cellCenterPosition, false);
        
        UseResourcesToBuild(dataBase.buildingData[selectedObjectIndex].ID);
        StopPlacement();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // Validar si se puede colocar en la cuadrícula en la que apunta
        GridData selectedData = dataBase.buildingData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        // Si estamos en el caso de los "caminos" hacemos esto
        if (selectedData == floorData)
            //if(selectedData.CanPlaceObjectAt(gridPosition, dataBase.buildingData[selectedObjectIndex].Size))
                return true;

        if (!selectedData.CanPlaceObjectAt(gridPosition, dataBase.buildingData[selectedObjectIndex].Size))
            return false;

        // Obtener la posición en el mundo para el centro de la celda
        Vector3 cellCenterPosition = grid.GetCellCenterWorld(gridPosition);

        // Verificar colisiones con otros edificios dentro del radio y en la capa de Building
        Collider[] colliders = Physics.OverlapBox(cellCenterPosition, new Vector3(4f, 0.1f, 4f), // Cambia según el tamaño del BoxColider
        Quaternion.identity,
        LayerMask.GetMask("Building"));
        foreach (var collider in colliders)
        {
            if (collider.gameObject != null)
            {
                Debug.Log($"Edificio detectado dentro del radio: {collider.gameObject.name}");
                return false;
            }
        }

        return true;
    }

    private void RotateBuilding()
    {
        Debug.Log("entro en RotateBuilding si pulso espacio");
        currentRotation = preview.RotateBuildingPreview();
        Debug.Log("valor de currentRotation al pulsar espacio: " + currentRotation);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnRotate -= RotateBuilding;
        lastDetectedPosition = Vector3Int.zero;
        currentRotation = 0;
    }

    private void OnParticleSystemStopped()
    {
        selectedObjectIndex = -1;
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnRotate -= RotateBuilding;
        lastDetectedPosition = Vector3Int.zero;

    }

    public void Update()
    {
        if (selectedObjectIndex < 0)
            return;

        CheckResourcesAmount();
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Consigo el centro de la casilla en funcion de su posicion
        Vector3 cellCenterPosition = grid.GetCellCenterWorld(gridPosition); 

        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(cellCenterPosition, placementValidity);
            lastDetectedPosition = gridPosition;
        }
    }

    public List<Vector2> GetplacedGameObjectsPositions()
    {
        foreach (var furniture in placedGameObjects)
        {
            Vector2 posV2 = new Vector2(grid.WorldToCell(furniture.transform.position).x, grid.WorldToCell(furniture.transform.position).y);
            if(furniture.CompareTag("Rail"))
            {
                placedObjectsCount++;
                playerManager.playerList[playerIndex].totalPoints += dataBase.buildingData[selectedObjectIndex].Points;
                displayPlayerPoints.text = playerManager.playerList[playerIndex].totalPoints.ToString() + " pts";
            }
            else
            {
                if (!playerManager.playerList[playerIndex].settlementsPositions.Contains(posV2))
                {
                    placedObjectsCount++;
                    allPlacedBuildingsPositions.Add(posV2);
                    playerManager.playerList[playerIndex].settlementsPositions.Add(posV2);
                    playerManager.playerList[playerIndex].totalPoints += dataBase.buildingData[selectedObjectIndex].Points;
                    displayPlayerPoints.text = playerManager.playerList[playerIndex].totalPoints.ToString() + " pts";
                }
            }
        }

        placedGameObjects.Clear();
        Debug.Log("contador de edificios puesto: " + playerManager.playerList[playerIndex].settlementsPositions.Count);
        return playerManager.playerList[playerIndex].settlementsPositions;
    }

    public void FirstTurn(int index)
    {
        playerIndex = index - 1;
        placedObjectsCount = 0;
        clayText.text = playerManager.playerList[playerIndex].resources[0].ToString();
        ironText.text = playerManager.playerList[playerIndex].resources[1].ToString();
        mountainText.text = playerManager.playerList[playerIndex].resources[3].ToString();
        wheatText.text = playerManager.playerList[playerIndex].resources[2].ToString();
        woodText.text = playerManager.playerList[playerIndex].resources[4].ToString();
        woolText.text = playerManager.playerList[playerIndex].resources[5].ToString();
    }

    // Funciones de actualización
    void UpdateResourceCountClay(int ID, int playerIndex)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = playerManager.playerList[playerIndex].resources[0];
        clayText.text = (currentCount - cost).ToString();
        playerManager.playerList[playerIndex].resources[0] -= cost;
        
    }

    void UpdateResourceCountMountain(int ID, int playerIndex)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = playerManager.playerList[playerIndex].resources[2];
        mountainText.text = (currentCount - cost).ToString();
        playerManager.playerList[playerIndex].resources[2] -= cost;
    }

    void UpdateResourceCountWheat(int ID, int playerIndex)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = playerManager.playerList[playerIndex].resources[3];
        wheatText.text = (currentCount - cost).ToString();
        playerManager.playerList[playerIndex].resources[3] -= cost;
    }

    void UpdateResourceCountIron(int ID, int playerIndex)
    {
        int cost = ID == 2 ? 2 : 1;
        //cost = dataBase.buildingData[ID].ResourcesCost[0];
        int currentCount = playerManager.playerList[playerIndex].resources[1];
        ironText.text = (currentCount - cost).ToString();
        playerManager.playerList[playerIndex].resources[1] -= cost;
    }

    void UpdateResourceCountWool(int ID, int playerIndex)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = playerManager.playerList[playerIndex].resources[5];
        woolText.text = (currentCount - cost).ToString();
        playerManager.playerList[playerIndex].resources[5] -= cost;
    }

    void UpdateResourceCountWood(int ID, int playerIndex)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = playerManager.playerList[playerIndex].resources[4];
        woodText.text = (currentCount - cost).ToString();
        playerManager.playerList[playerIndex].resources[4] -= cost;
    }

    public void UseResourcesToBuild(int ID)
    {
        switch (ID) 
        {
            case 0:
                UpdateResourceCountMountain(0, playerIndex);
                UpdateResourceCountIron(0, playerIndex);
            break;
            case 1:
                UpdateResourceCountClay(1, playerIndex);
                UpdateResourceCountWheat(1, playerIndex);
                UpdateResourceCountWood(1, playerIndex);
                UpdateResourceCountWool(1, playerIndex);
            break;
            case 2:
                UpdateResourceCountClay(2, playerIndex);
                UpdateResourceCountMountain(2, playerIndex);
                UpdateResourceCountIron(2, playerIndex);
                UpdateResourceCountWheat(2, playerIndex);
                UpdateResourceCountWood(2, playerIndex);
                UpdateResourceCountWool(2, playerIndex);
            break;

        }
    }

    private void CheckResourcesAmount()
    {
        Debug.Log("mi indice es: " + playerIndex);
        int currentClayCount = playerManager.playerList[playerIndex].resources[0];
        int currentIronCount = playerManager.playerList[playerIndex].resources[1];
        int currentStoneCount = playerManager.playerList[playerIndex].resources[2];
        int currentWheatCount = playerManager.playerList[playerIndex].resources[3];
        int currentWoodCount = playerManager.playerList[playerIndex].resources[4];
        int currentWoolCount = playerManager.playerList[playerIndex].resources[5];

        if (currentIronCount > 0 && currentStoneCount > 0)
        {
            Debug.Log("hay recursos rail");
            railButton.enabled = true;

        }
        else
        {
            railButton.enabled = false;
        }

        if (currentClayCount > 0 && currentWheatCount > 0 && currentWoodCount > 0 && currentWoolCount > 0)
        {
            Debug.Log("hay recursos pueblo");
            villageButton.enabled = true;
        }
        else
        {
            villageButton.enabled = false;
        }

        if (currentClayCount > 1 && currentWheatCount > 1 && currentWoodCount > 1 && currentWoolCount > 1 && currentIronCount > 1 && currentStoneCount > 1) 
        {
            cityButton.enabled = true;
        }
        else
        {
            cityButton.enabled = false;
        }
    }

    public void UpdateCurrentPlayer(int index)
    {
        playerIndex = index - 1;
    }

    private void ApplyPlayerColorOnBuildings(GameObject parent, Color color)
    {
        
        Transform baseTransform = parent.GetComponentInChildren<Transform>().Find("Base");
        Debug.Log("mi base es: " + baseTransform);
        if (baseTransform != null)
        {
            Renderer baseRenderer = baseTransform.GetComponent<Renderer>();
            if (baseRenderer != null)
            {
                baseRenderer.material.color = color;
            }
        }
    }
    /*
    public void UpdatePlayerPoints(int index)
    {
        displayPlayerPoints.text = playerManager.playerList[playerIndex].totalPoints.ToString() + " pts";
    }*/
}
