using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer tileRenderer; // Sigue siendo privada, pero las clases hijas pueden acceder a ella
    [SerializeField] private GameObject highLight;

    public virtual void Init(int x, int y)
    {

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
}
