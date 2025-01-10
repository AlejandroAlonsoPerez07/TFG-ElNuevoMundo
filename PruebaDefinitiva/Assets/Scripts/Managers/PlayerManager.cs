using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] Grid grid;

    public static PlayerManager Instance;
    private int randomNumber;
    public List<Vector2> settlementsPositions;


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public List<Vector2> GetSettlementsPositions()
    {
        foreach (var pos in buildingManager.placedGameObjectsPositions)
        {
            settlementsPositions.Add(pos);
        }
        return settlementsPositions;
    }
}
