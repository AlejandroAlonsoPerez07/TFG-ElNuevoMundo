using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;
    [SerializeField] private GameObject cellIndicador;
    private GameObject previewObject;
    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialInstance;
    private Renderer cellIndicatorRenderer;

    [SerializeField] private GameObject actionRadiusPrefab;
    private GameObject actionRadiusIndicator;
    public float currentRotation = 0f;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicador.SetActive(false);
        cellIndicatorRenderer = cellIndicador.GetComponentInChildren<Renderer>();
        // Inicializa el radio de acción
        if (actionRadiusPrefab != null)
        {
            actionRadiusIndicator = Instantiate(actionRadiusPrefab);
            actionRadiusIndicator.SetActive(false);
        }
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        //cellIndicador.SetActive(true);
        // Configura el radio de acción
        if (actionRadiusIndicator != null)
        {
            actionRadiusIndicator.transform.localScale = new Vector3(1.2f, 0.01f, 1.2f);
            actionRadiusIndicator.SetActive(true);
        }
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicador.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicador.SetActive(false);
        Destroy(previewObject);

        if (actionRadiusIndicator != null)
        {
            actionRadiusIndicator.SetActive(false);
        }
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
        // Mueve el radio de acción junto con el objeto
        MoveActionRadius(position);
        //RotateBuilding();
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
        // Feedback recibido del plano de accion
        if (actionRadiusIndicator != null)
        {
            Renderer radiusRenderer = actionRadiusIndicator.GetComponent<Renderer>();
            if (radiusRenderer != null)
            {
                radiusRenderer.material.color = c;
            }
        }
    }

    // Mueve el cursor en funcion de la casilla en la que nos encontramos
    private void MoveCursor(Vector3 position)
    {
        cellIndicador.transform.position = position;
    }

    // Mueve la previsualizacion del edificio en funcion de la posicion del raton
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    // Desplaza el radio la posicion recibida del raton
    private void MoveActionRadius(Vector3 position)
    {
        if (actionRadiusIndicator != null)
        {
            actionRadiusIndicator.transform.position = new Vector3(position.x, position.y, position.z);
        }
    }

    public float RotateBuildingPreview()
    {
        currentRotation += 45f;
        if(currentRotation == 360)
        {
            currentRotation = 0;
        }
        previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        return currentRotation;
    }
}
