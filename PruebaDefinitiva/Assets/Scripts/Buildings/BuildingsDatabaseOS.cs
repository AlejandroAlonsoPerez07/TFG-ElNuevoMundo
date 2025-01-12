using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingsDatabaseOS : ScriptableObject
{
    public List<BuildingData> buildingData;
}

[Serializable]
public class BuildingData
{
    [field: SerializeField]

    public string Name { get; private set; }
    [field: SerializeField]

    public int ID { get; private set; }
    [field: SerializeField]

    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]

    public List<int> ResourcesCost { get; private set; } = new ();
    [field: SerializeField]

    /*
    public float actionRadius { get; private set; }
    [field: SerializeField]*/
    public GameObject prefab { get; private set; }
}
