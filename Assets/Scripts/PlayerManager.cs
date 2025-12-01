using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnReadyChanged))]
    public bool ready;

    private LobbyManager lobbyManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        lobbyManager.SetLocalPlayerManager(this);
    }

    [Command]
    public void ToggleReady()
    {
        ready = !ready;
    }

    void OnReadyChanged(bool oldValue, bool newValue)
    {
        lobbyManager.UpdateLobbyText();
    }
}
