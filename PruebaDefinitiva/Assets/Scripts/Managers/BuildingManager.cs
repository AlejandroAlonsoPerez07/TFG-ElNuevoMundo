using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [SerializeField] GameObject mouseIndicator;
    [SerializeField] BuildingsDatabaseOS dataBase;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;
    private GridData floorData, furnitureData;
    public List<GameObject> placedGameObjects = new();
    private int selectedObjectIndex = -1;
    [SerializeField] private PreviewSystem preview;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    public List<Vector2> placedGameObjectsPositions = new();

    private int placedObjectsCount;

    // Gestion de recursos
    [SerializeField] private TMP_Text clayText;
    [SerializeField] private TMP_Text ironText;
    [SerializeField] private TMP_Text mountainText;
    [SerializeField] private TMP_Text wheatText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text woolText;

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
        newObject.transform.position = cellCenterPosition;
        newObject.layer = LayerMask.NameToLayer("Building");
        placedGameObjects.Add(newObject);
        GridData selectedData = dataBase.buildingData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        GetplacedGameObjectsPositions();

        selectedData.AddObjectAt(gridPosition,
            dataBase.buildingData[selectedObjectIndex].Size,
            dataBase.buildingData[selectedObjectIndex].ID,
            placedGameObjects.Count - 1);
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
            if(selectedData.CanPlaceObjectAt(gridPosition, dataBase.buildingData[selectedObjectIndex].Size))
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

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void OnParticleSystemStopped()
    {
        selectedObjectIndex = -1;
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
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
            }
            else
            {
                if (!placedGameObjectsPositions.Contains(posV2))
                {
                    placedObjectsCount++;
                    placedGameObjectsPositions.Add(posV2);
                }
            }
        }
        Debug.Log("contador de edificios puesto: " + placedGameObjectsPositions.Count);
        return placedGameObjectsPositions;
    }

    public void FirstTurn()
    {
        //Update();
        //GameManager.Instance.UpdateGameState(GameManager.GameState.BuildingPhase);
    }

    // Funciones de actualización
    void UpdateResourceCountClay(int ID)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = int.Parse(clayText.text);
        clayText.text = (currentCount - cost).ToString();
    }

    void UpdateResourceCountMountain(int ID)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = int.Parse(mountainText.text);
        mountainText.text = (currentCount - cost).ToString();
    }

    void UpdateResourceCountWheat(int ID)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = int.Parse(wheatText.text);
        wheatText.text = (currentCount - cost).ToString();
    }

    void UpdateResourceCountIron(int ID)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = int.Parse(ironText.text);
        ironText.text = (currentCount - cost).ToString();
    }

    void UpdateResourceCountWool(int ID)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = int.Parse(woolText.text);
        woolText.text = (currentCount - cost).ToString();
    }

    void UpdateResourceCountWood(int ID)
    {
        int cost = ID == 2 ? 2 : 1;
        int currentCount = int.Parse(woodText.text);
        woodText.text = (currentCount - cost).ToString();
    }

    public void UseResourcesToBuild(int ID)
    {
        switch (ID) 
        {
            case 0:
                UpdateResourceCountMountain(0);
                UpdateResourceCountIron(0);
            break;
            case 1:
                UpdateResourceCountClay(1);
                UpdateResourceCountWheat(1);
                UpdateResourceCountWood(1);
                UpdateResourceCountWool(1);
            break;
            case 2:
                UpdateResourceCountClay(2);
                UpdateResourceCountMountain(2);
                UpdateResourceCountIron(2);
                UpdateResourceCountWheat(2);
                UpdateResourceCountWood(2);
                UpdateResourceCountWool(2);
            break;

        }
    }

    private void CheckResourcesAmount()
    {
        int currentClayCount = int.Parse(clayText.text);
        int currentIronCount = int.Parse(ironText.text);
        int currentStoneCount = int.Parse(mountainText.text);
        int currentWheatCount = int.Parse(wheatText.text);
        int currentWoodCount = int.Parse(woodText.text);
        int currentWoolCount = int.Parse(woolText.text);

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
}
