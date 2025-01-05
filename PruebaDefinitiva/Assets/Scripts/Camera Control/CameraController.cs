using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineVirtualCamera topDownCamera;
    [SerializeField] private float cameraSpeed = 10f;
    [SerializeField] private float cameraZoomSpeed = 1f;
    [SerializeField] private float cameraZoomMin = 5f;
    [SerializeField] private float cameraZoomMax = 100f;
    [SerializeField] private float cameraZoomDefault = 30f;

    private Coroutine panCoroutine;
    private Coroutine zoomCoroutine;

    public void OnPanChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (panCoroutine != null)
            {
                StopCoroutine(panCoroutine);
            }
            panCoroutine = StartCoroutine(ProcessPan(context));
        }
        else if (context.canceled)
        {
            if (panCoroutine != null)
            {
                StopCoroutine(panCoroutine);
            }
        }
    }
    public void OnZoomChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            zoomCoroutine = StartCoroutine(ProcessZoom(context));
        }
        else if (context.canceled)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
        }   
    }

    public IEnumerator ProcessPan(InputAction.CallbackContext context)
    {
        while (true)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            Debug.Log("Moving: " + inputVector);

            Vector3 moveVector = new Vector3(inputVector.x, 0, inputVector.y);
            cameraTarget.transform.position += moveVector * cameraSpeed * Time.deltaTime;

            yield return null;
        }
    }
    
    public IEnumerator ProcessZoom(InputAction.CallbackContext context)
    {
        while (true)
        {
            float zoomInput = context.ReadValue<float>();
            Debug.Log("Zooming: " + zoomInput);

            float zoomAmount = topDownCamera.m_Lens.FieldOfView + zoomInput * cameraZoomSpeed * Time.deltaTime;
            Debug.Log("Zoom amount: " + zoomAmount);
            topDownCamera.m_Lens.FieldOfView = Mathf.Clamp(zoomAmount, cameraZoomMin, cameraZoomMax);

            yield return null;
        }
    }
    /*
    private CinemachineVirtualCamera GetCamera(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.TopDown;
                return topDownCamera;
            default:
                return null;
        }
    }*/
}
