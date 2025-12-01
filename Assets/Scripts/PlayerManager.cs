using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnReadyChanged))]
    public bool ready;

    [SyncVar]
    public bool inGame;

    private LobbyManager lobbyManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        inGame = false;
    }
    public override void OnStartClient()
    {
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        if (lobbyManager != null) Debug.Log("Found lobby Manager!");
        lobbyManager.SetLocalPlayerManager(this);
        Debug.Log("after");
    }

    public void ToggleReady()
    {
        if (!isLocalPlayer) return;

        CmdToggleReady();
    }

    [Command]
    void CmdToggleReady()
    {
        ready = !ready;

        // manually update host UI immediately
        if (isServer)
            OnReadyChanged(ready, !ready); // call hook manually
    }

    void OnReadyChanged(bool oldValue, bool newValue)
    {
        if (lobbyManager != null)
            lobbyManager.UpdateLobbyText();
        else
            Debug.Log("Manager null");
    }

    bool CheckAllReady()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            var player = conn.identity.GetComponent<PlayerManager>();
            if (!player.ready) return false;
        }
        return true;
    }
    
    //Host Logic Start Lobby ( I know it should be in lobby manager)...
    [Command]
    public void CmdRequestStartGame()
    {
        if (!isServer) return;
        if (!CheckAllReady()) return;

        foreach (var conn in NetworkServer.connections.Values)
        {
            var player = conn.identity.GetComponent<PlayerSetup>();
            if (player != null)
            {
                player.RunInGameStartPlayer(); // this should be a [ClientRpc]
            }
        }
    }
}
