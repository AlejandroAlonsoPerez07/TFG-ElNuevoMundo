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
    [SerializeField] private GameObject helpPanel, victoryPanel, resourceObtainedPanel;

    [SerializeField] private BasePlayer newPlayerPrefab;
    [SerializeField] public List<BasePlayer> playerList;
    [SerializeField] private Image displayPlayerColor, displayPlayerColorVictory;
    
    private int playerIndex, pointsToWin = 5;
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
    public void DeactivePassTurnButton()
    {
        passTurnButton.enabled = false;
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
        // Recorro todos los paneles
        foreach (Transform panel in resourceObtainedPanel.transform)
        {
            for(int i = 1; i < panel.childCount; i++) // Recorro todos los hijos de los paneles menos el primero
            {
                // Reseteo todos los recursos de cada panel a 0
                Transform resource = panel.GetChild(i);
                resource.GetChild(0).GetComponent<TMP_Text>().text = "+ 0";
            }
        }
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
        UpdatePanelCrownColor(newPlayer.playerColor, playerIndex);
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
        displayPlayerColorVictory.color = playerList[playerIndex].playerColor;
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
        Debug.Log("dentro de actualizar el color del jugador (playerIndex)" + playerIndex);
        
        return colorSelected;
    }

    public void UpdatePanelCrownColor(Color playerColor, int playerIndex)
    {
        // Obtengo el panel correspondiente al panel padre dado el indice
        Transform panel = resourceObtainedPanel.transform.GetChild(playerIndex);
        panel.gameObject.SetActive(true);
        // Obtengo dentro del panel, el objeto que se llama en la jerarquía
        Transform corona = panel.Find("CrownColorPlayer");
        corona.GetComponent<Image>().color = playerColor;
    }
}
