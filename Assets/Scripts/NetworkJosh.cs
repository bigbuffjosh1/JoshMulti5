using Mirror;
using UnityEngine;

public class NetworkJosh : NetworkManager
{

    public GameObject lobbyManagerPrefab;
    private GameObject lobbyManagerInstance;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn); // spawns the player

        // Only spawn the LobbyManager for the first player (the host)
        if (lobbyManagerInstance == null)
        {
            lobbyManagerInstance = Instantiate(lobbyManagerPrefab);
            NetworkServer.Spawn(lobbyManagerInstance);
        }
    }
}
