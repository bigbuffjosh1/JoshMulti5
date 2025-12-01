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
    public override void OnStartLocalPlayer()
    {
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        //lobbyManager.SetLocalPlayerManager(this);
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
        if (!isLocalPlayer) return;
        lobbyManager.UpdateLobbyText();
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
