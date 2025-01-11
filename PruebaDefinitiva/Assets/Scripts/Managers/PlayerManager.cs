using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;

    public static PlayerManager Instance;
    public List<Vector2> settlementsPositions;


    void Awake()
    {
        Instance = this;
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
