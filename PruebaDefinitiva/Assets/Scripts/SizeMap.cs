using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SizeMap : MonoBehaviour
{
    [SerializeField] private Slider numberOfPlayersSlider;
    public void CreateSmallMap()
    {
        GameManager.Instance.size = 7;
        GameManager.Instance.numberOfPlayers = Mathf.RoundToInt(numberOfPlayersSlider.value);
        Debug.Log("Pequeño " + GameManager.Instance.numberOfPlayers);
        //GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
    }
    public void CreateMediumMap()
    {
        GameManager.Instance.size = 9;
        GameManager.Instance.numberOfPlayers = Mathf.RoundToInt(numberOfPlayersSlider.value);
        Debug.Log("Medio " + GameManager.Instance.numberOfPlayers);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
    }
    public void CreateLargeMap()
    {
        GameManager.Instance.size = 11;
        GameManager.Instance.numberOfPlayers = Mathf.RoundToInt(numberOfPlayersSlider.value);
        Debug.Log("Grande " + GameManager.Instance.numberOfPlayers);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
    }
}
