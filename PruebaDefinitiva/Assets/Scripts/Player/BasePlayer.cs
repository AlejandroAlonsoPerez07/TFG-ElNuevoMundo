using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public List<Vector2> settlementsPositions;
    public List<int> resources = new() { 1, 1, 1, 1, 1, 1 };
    public int totalPoints = 0;
    public Color playerColor;

    private void Start()
    {
        playerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
    }
}
