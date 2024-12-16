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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void CreateMediumMap()
    {
        GameManager.Instance.size = 9;
        Debug.Log("Medio");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void CreateLargeMap()
    {
        GameManager.Instance.size = 11;
        Debug.Log("Grande");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
