using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer tileRenderer; // Sigue siendo privada, pero las clases hijas pueden acceder a ella
    [SerializeField] private GameObject highLight;
    [SerializeField] protected TMP_Text numberText; // Referencia al Text para mostrar el número
    
    public int randomNumber;

    public virtual void Init(int x, int y)
    {
        randomNumber = GenerateRandomNumber();
        UpdateTileNumber();
        Debug.Log($"Tile {this.GetType()} número aleatorio: {randomNumber}");
    }

    void OnMouseEnter()
    {
        highLight.SetActive(true);
        Debug.Log("true" + this.name);
    }

    void OnMouseExit()
    {
        highLight.SetActive(false);
        Debug.Log("false" + this.name);
    }

    int GenerateRandomNumber()
    {
        // Elegir aleatoriamente entre los dos rangos
        if (Random.value < 0.5f) // 50% de probabilidad
        {
            return Random.Range(2, 7); // Rango 2-6 (el límite superior es exclusivo)
        }
        else
        {
            return Random.Range(8, 13); // Rango 8-12
        }
    }
    private void UpdateTileNumber()
    {
        if (numberText != null)
        {
            numberText.text = randomNumber.ToString(); // Asigna el número generado al Text
        }
    }

    public int GetRandomNumber()
    {
        return randomNumber;
    }
}
