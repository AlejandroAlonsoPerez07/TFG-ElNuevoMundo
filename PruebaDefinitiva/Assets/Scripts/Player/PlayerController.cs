using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private CinemachineVirtualCamera camera;
    private void Start()
    {
        if (isLocalPlayer)
        {
            camera = FindObjectOfType<CinemachineVirtualCamera>();
            camera.enabled = true;
            /*
            camera.Follow = transform;
            camera.LookAt = transform;
            */
        }
        else
        {
            camera.enabled = false;
        }
    }
    private void Update()
    {
        if (isLocalPlayer) // nos devuelve solo el de la maquina local
        {
            // shoot
            if (Input.GetMouseButtonDown(0))
            {
                //CmdFire();
            }
        }
    }
    private void FixedUpdate()
    {
        if (isLocalPlayer) // nos devuelve solo el de la maquina local
        {
        /*
            Vector3 inputAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            camera.velocity = inputAxis * 500 * Time.fixedDeltaTime;

            Rotate();*/
        }
    }
}
