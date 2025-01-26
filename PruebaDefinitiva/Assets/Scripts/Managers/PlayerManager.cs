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
    [SerializeField] private GameObject helpPanel, victoryPanel;

    [SerializeField] private BasePlayer newPlayerPrefab;
    [SerializeField] public List<BasePlayer> playerList;
    [SerializeField] private Image displayPlayerColor;

    private int playerIndex, pointsToWin = 20;
    public List<Color> colors = new() { new Color(1f,0f,0f,1f), new Color(0.2424675f, 0.07115523f, 0.3867925f, 1f),
                                        new Color(0f,1f,0.4305303f,1f), new Color(0.6509434f, 0.5555875f, 0.2180046f, 1f)};

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
        Debug.Log("activo el boton de tirar dados");
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
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void NewPlayer(int index)
    {
        var newPlayer = Instantiate(newPlayerPrefab, Vector3Int.zero, Quaternion.identity);
        Debug.Log("El nuevo jugador: " + newPlayer.playerColor);
        newPlayer.playerColor = UpdateColorPlayer();
        playerList.Add(newPlayer);
        playerIndex = index - 1;
        displayPlayerColor.color = newPlayer.playerColor;
        Debug.Log("El indice del jugador actual en NewPlayer: " + playerIndex);
    }

    public void CheckVictory(int index)
    {
        playerIndex = index - 1;
        Debug.Log("playerIndex en checkVictory: " + playerIndex);
        if (playerList[playerIndex].totalPoints >= pointsToWin)
        {
            PopUpVictory();
            GameManager.Instance.UpdateGameState(GameManager.GameState.Victory);
        }
    }

    public void PopUpVictory()
    {
        victoryPanel.SetActive(true);
    }

    public void DisplayHelp()
    {
        helpPanel.SetActive(!helpPanel.activeSelf);
    }

    public void JustOneMoreTurn()
    {
        victoryPanel.SetActive(false);
        pointsToWin = 10000;
        GameManager.Instance.UpdateGameState(GameManager.GameState.BuildingPhase);
    }

    public Color UpdateColorPlayer()
    {
        Debug.Log("dentro de actualizar el color del jugador (colors.count) " + colors.Count);
        int random = Random.Range(0, colors.Count);
        Debug.Log("dentro de actualizar el color del jugador (random) " + random);
        Color colorSelected = colors[random];
        Debug.Log("dentro de actualizar el color del jugador (colorSelected) " + colorSelected);
        colors.RemoveAt(random);
        Debug.Log("dentro de actualizar el color del jugador (colors.count) despues de eliminar" + colors.Count);
        return colorSelected;
        
        
    }
}
