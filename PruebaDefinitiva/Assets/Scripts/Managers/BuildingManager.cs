using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();
        
    }

    public void StartPlacement(int ID)
    {
        if (GameManager.Instance.State == GameManager.GameState.FirstTurn)
        {

        }
        else
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
            placedGameObjectsPositions.Add(posV2);
        }

        return placedGameObjectsPositions;
    }

    public void FirstTurn()
    {

    }
}
