using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [SerializeField] GameObject mouseIndicator;
    [SerializeField] BuildingsDatabaseOS dataBase;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;
    private GridData floorData, furnitureData;
    private List<GameObject> placedGameObjects = new();
    private int selectedObjectIndex = -1;
    [SerializeField] private PreviewSystem preview;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

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
        StopPlacement();
        selectedObjectIndex = dataBase.buildingData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        //gridVisualization.SetActive(true);
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
        //gridPosition.x += 1;
        //gridPosition.y += 0;
        //gridPosition.z += 1;
        Debug.Log("posicion edificio: " + gridPosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        GameObject newObject = Instantiate(dataBase.buildingData[selectedObjectIndex].prefab);
        Vector3 cellCenterPosition = grid.GetCellCenterWorld(gridPosition); // obtenemos el centro de la casilla correspondiente a sus coordenadas del mundo
        newObject.transform.position = cellCenterPosition;
        placedGameObjects.Add(newObject);
        GridData selectedData = dataBase.buildingData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        
        selectedData.AddObjectAt(gridPosition,
            dataBase.buildingData[selectedObjectIndex].Size,
            dataBase.buildingData[selectedObjectIndex].ID,
            placedGameObjects.Count - 1);
        preview.UpdatePosition(cellCenterPosition, false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = dataBase.buildingData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, dataBase.buildingData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        //gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void OnParticleSystemStopped()
    {
        selectedObjectIndex = -1;
        //gridVisualization.SetActive(false);
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
        Debug.Log(mousePosition + " mousePosition(Building Manager)");
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        Vector3 cellCenterPosition = grid.GetCellCenterWorld(gridPosition); // consigo el centro de la casilla en funcion de su posicion

        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(cellCenterPosition, placementValidity);
            lastDetectedPosition = gridPosition;
        }
    }
}
