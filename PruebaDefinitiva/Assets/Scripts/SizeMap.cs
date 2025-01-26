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
        GameManager.Instance.size = 11;
        GameManager.Instance.numberOfPlayers = Mathf.RoundToInt(numberOfPlayersSlider.value);
        Debug.Log("Pequeño " + GameManager.Instance.numberOfPlayers);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void CreateMediumMap()
    {
        GameManager.Instance.size = 17;
        GameManager.Instance.numberOfPlayers = Mathf.RoundToInt(numberOfPlayersSlider.value);
        Debug.Log("Medio " + GameManager.Instance.numberOfPlayers);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void CreateLargeMap()
    {
        GameManager.Instance.size = 25;
        GameManager.Instance.numberOfPlayers = Mathf.RoundToInt(numberOfPlayersSlider.value);
        Debug.Log("Grande " + GameManager.Instance.numberOfPlayers);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
