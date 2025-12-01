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
        //lobbyManager = GetComponent<PlayerSetup>().lobbyManager;
    }

    public void ToggleReady()
    {
        CmdToggleReady();
    }

    [Command]
    void CmdToggleReady()
    {
        ready = !ready;
    }

    void OnReadyChanged(bool oldValue, bool newValue)
    {
        lobbyManager = LobbyManager.instance;
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
