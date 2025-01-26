using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;

    public static event Action<GameState> ChangeState;
    public int size;
    public int numberOfPlayers;
    public int currentPlayer;
    private int turnCounter = 0;
    private bool firstTurnPhase = true;

    [SerializeField] private TMP_Text displayGameState;
    [SerializeField] private TMP_Text displayPlayer, displayPlayerPoints;
    

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateGameState(GameState.CreatingGame);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        if(displayGameState == null || displayPlayer == null)
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag("DisplayGameState");
            displayGameState = targetObject.GetComponent<TMP_Text>();
            GameObject targetObjectPlayer = GameObject.FindGameObjectWithTag("DisplayPlayer");
            displayPlayer = targetObjectPlayer.GetComponent<TMP_Text>();
        }
        displayGameState.text = newState.ToString();
        displayPlayer.text = "Jugador " + currentPlayer.ToString();
        switch (newState)
        {
            case GameState.CreatingGame:
                break;
            case GameState.GenerateGrid:
                Debug.Log("entro en el estado generar grid");
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.BuildingPhase:
                Debug.Log("entro en el estado construyendo");
                ResourceManager.Instance.UpdateInterfaceResourcesOnInventory(currentPlayer);
                PlayerManager.Instance.ActivePassTurnButton();
                PlayerManager.Instance.DeactiveDiceRollButton();
                BuildingManager.Instance.UpdateCurrentPlayer(currentPlayer);
                BuildingManager.Instance.Update();
                PlayerManager.Instance.CheckVictory(currentPlayer);
                break;
            case GameState.PlayerTurn:
                if (currentPlayer >= numberOfPlayers)
                {
                    currentPlayer = 1;
                }
                else
                {
                    currentPlayer++;
                }
                Debug.Log("Turno del jugador: " + currentPlayer);

                if (firstTurnPhase)
                {
                    
                    PlayerManager.Instance.NewPlayer(currentPlayer);
                    this.UpdateGameState(GameState.FirstTurn);
                    turnCounter++;
                    if(turnCounter == numberOfPlayers)
                        firstTurnPhase = false;
                }
                else
                {
                    this.UpdateGameState(GameState.DiceRoll);
                }
                break;
            case GameState.FirstTurn:
                Debug.Log("Estado de primer turno");
                ResourceManager.Instance.LoadResourcesOnInterface(currentPlayer);
                PlayerManager.Instance.DeactivePassTurnButton();
                PlayerManager.Instance.DeactiveDiceRollButton();
                BuildingManager.Instance.FirstTurn(currentPlayer);
                break;
            case GameState.DiceRoll:
                Debug.Log("Estado de tirando dados");
                PlayerManager.Instance.CleanResourcesObtained();
                ResourceManager.Instance.LoadResourcesOnInterface(currentPlayer);
                PlayerManager.Instance.ActiveDiceRollButton();
                PlayerManager.Instance.DeactivateButtonsOnDiceRollState();
                break;
            case GameState.EndTurn:
                Debug.Log("Estado de finalizar turno");
                PlayerManager.Instance.CheckVictory(currentPlayer);
                break;
            case GameState.Victory:
                Debug.Log("Estado de VICTORIA del jugador: " + currentPlayer);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        ChangeState?.Invoke(newState);
    }

    public enum GameState
    {
        CreatingGame = 0,
        GenerateGrid = 1,
        FirstTurn = 2,
        DiceRoll = 3,
        BuildingPhase = 4,
        EndTurn = 5,
        PlayerTurn = 6,
        Victory = 7
    }
}
