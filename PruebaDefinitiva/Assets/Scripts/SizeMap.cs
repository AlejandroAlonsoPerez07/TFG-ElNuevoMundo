using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SizeMap : MonoBehaviour
{
    public void CreateSmallMap()
    {
        GameManager.Instance.size = 7;
        Debug.Log("Pequeño");
        //GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
    }
    public void CreateMediumMap()
    {
        GameManager.Instance.size = 9;
        Debug.Log("Medio");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
    }
    public void CreateLargeMap()
    {
        GameManager.Instance.size = 11;
        Debug.Log("Grande");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //GameManager.Instance.UpdateGameState(GameManager.GameState.GenerateGrid);
    }
}
