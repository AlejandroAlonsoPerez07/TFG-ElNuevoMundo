using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Conectado con el server");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log("Jugador añadido al server " + numPlayers);
        
        PlayerController mfs = conn.identity.GetComponent<PlayerController>(); // Obtengo la instancia de cada jugador
        /*
        mfs.SetVida(10);
        mfs.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1)); // el ultimo 1 es el alpha
        */
    }
}
