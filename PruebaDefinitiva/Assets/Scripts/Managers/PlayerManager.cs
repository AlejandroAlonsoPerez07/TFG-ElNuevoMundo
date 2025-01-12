using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;

    public static PlayerManager Instance;
    public List<Vector2> settlementsPositions;
    [SerializeField] private Button railButton, villageButton, cityButton, diceRollButton, passTurnButton;

    void Awake()
    {
        Instance = this;
    }

    public List<Vector2> GetSettlementsPositions()
    {
        foreach (var pos in buildingManager.placedGameObjectsPositions)
        {
            if (!settlementsPositions.Contains(pos))
            {
                settlementsPositions.Add(pos);
            }
        }
        Debug.Log("añado construccion a la lista: " + settlementsPositions.Count);
        return settlementsPositions;
    }

    public void PassTurn()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.DiceRoll);
    }

    public void ActiveDiceRollButton()
    {
        diceRollButton.enabled = true;
    }

    public void DeactiveDiceRollButton()
    {
        diceRollButton.enabled = false;
    }

    public void ActivePassTurnButton()
    {
        passTurnButton.enabled = true;
    }

    public void DeactivateButtonsOnDiceRollState()
    {
        railButton.enabled = false;
        villageButton.enabled = false;
        cityButton.enabled = false;
        passTurnButton.enabled = false;
    }
}
