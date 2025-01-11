using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;

    public static event Action<GameState> ChangeState;
    public int size;

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
        switch (newState)
        {
            case GameState.CreatingGame:
                break;
            case GameState.GenerateGrid:
                Debug.Log("entro en el estado generar grid");
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.BuildingPhase:
                Debug.Log("entro en el estado spawn heroes");
                BuildingManager.Instance.Update();
                break;
            case GameState.FirstTurn:
                Debug.Log("Estado de primer turno");
                BuildingManager.Instance.Update();
                break;
            case GameState.DiceRoll:
                break;
            case GameState.EndTurn:
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
        EndTurn = 5
    }
}
