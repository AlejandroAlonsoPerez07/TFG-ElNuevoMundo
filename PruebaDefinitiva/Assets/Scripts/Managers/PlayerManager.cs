using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] GameManager gameManager;

    public static PlayerManager Instance;
    [SerializeField] private Button railButton, villageButton, cityButton, diceRollButton, passTurnButton;
    [SerializeField] private TMP_Text clayObtainedText, mountainObtainedText, wheatObtainedText, ironObtainedText, woolObtainedText, woodObtainedText;

    [SerializeField] private BasePlayer newPlayerPrefab;
    [SerializeField] public List<BasePlayer> playerList;

    private int playerIndex;

    void Awake()
    {
        Instance = this;
    }

    public List<Vector2> GetSettlementsPositions()
    {

        Debug.Log("El indice del jugador actual en GetSettlement: " + playerIndex);
        foreach (var pos in buildingManager.placedGameObjectsPositions)
        {
            if (!playerList[playerIndex].settlementsPositions.Contains(pos))
            {
                playerList[playerIndex].settlementsPositions.Add(pos);
            }
        }
        Debug.Log("añado construccion a la lista: " + playerList[playerIndex].settlementsPositions.Count);
        return playerList[playerIndex].settlementsPositions;
    }

    public void PassTurn()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.PlayerTurn);
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

    public void CleanResourcesObtained()
    {
        clayObtainedText.text = "+ 0";
        mountainObtainedText.text = "+ 0";
        wheatObtainedText.text = "+ 0";
        ironObtainedText.text = "+ 0";
        woolObtainedText.text = "+ 0";
        woodObtainedText.text = "+ 0";
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void NewPlayer(int index)
    {
        var newPlayer = Instantiate(newPlayerPrefab, Vector3Int.zero, Quaternion.identity);
        playerList.Add(newPlayer);
        playerIndex = index - 1;
        Debug.Log("El indice del jugador actual en NewPlayer: " + playerIndex);
    }
}
