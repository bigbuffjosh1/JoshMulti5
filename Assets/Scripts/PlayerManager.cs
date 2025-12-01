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
        //lobbyManager = FindFirstObjectByType<LobbyManager>();
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
        lobbyManager = FindFirstObjectByType<LobbyManager>().GetComponent<LobbyManager>();
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
    
}
