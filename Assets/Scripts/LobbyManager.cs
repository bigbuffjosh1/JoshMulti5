using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class LobbyManager : NetworkBehaviour
{
    public Button ReadyButton;
    public Button StartButton;
    public Button InviteButton;
    public Button QuitButton;
    public Button SwapRoleButton;

    public TMP_Text p1Text;
    public TMP_Text p2Text;

    public PlayerManager playerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReadyButton = GameObject.Find("Canvas/LobbyMenu/Ready").GetComponent<Button>();
        StartButton = GameObject.Find("Canvas/LobbyMenu/Start").GetComponent<Button>();
        SwapRoleButton = GameObject.Find("Canvas/LobbyMenu/Swap Role").GetComponent<Button>();
        InviteButton = GameObject.Find("Canvas/LobbyMenu/Invite").GetComponent<Button>();
        QuitButton = GameObject.Find("Canvas/LobbyMenu/Quit").GetComponent<Button>();

        p1Text = GameObject.Find("Canvas/LobbyMenu/Player1Text").GetComponent<TMP_Text>();
        p2Text = GameObject.Find("Canvas/LobbyMenu/Player2Text").GetComponent<TMP_Text>();



        ReadyButton.onClick.AddListener(ReadyButtonPressed);
        StartButton.onClick.AddListener(StartButtonPressed);

        playerManager = NetworkClient.localPlayer.GetComponent<PlayerManager>();
    }

    void ReadyButtonPressed()
    {
        playerManager.ToggleReady();
    }

    void StartButtonPressed()
    {
        if (!playerManager.isLocalPlayer) return;

        //playerManager.CmdRequestStartGame();
    }

    public void UpdateLobbyText()
    {

        PlayerManager p1 = null;
        PlayerManager p2 = null;

        int connectionNumber = 0;
        foreach (var conn in NetworkClient.spawned)
        {
            var playerManager = conn.Value.GetComponent<PlayerManager>();

            if (playerManager == null) Debug.Log("Player manager null line lobby manager");

            if (connectionNumber == 0)
            {
                p1 = playerManager;
            }
            else if (connectionNumber == 1)
            {
                p2 = playerManager;
            }
            connectionNumber++;

        }

        //Add waiting for player later if p1 | p2 ==null
        if (p1 != null)
            p1Text.text = $"Ready: {p1.ready}";
        if (p2 != null)
            p2Text.text = $"Ready: {p2.ready}";
    }

    //Host Logic Start Lobby ( I know it should be in lobby manager)...
    //[Command]
    //public void CmdRequestStartGame()
    //{
    //    if (!isServer) return;
    //    if (!CheckAllReady()) return;

    //    foreach (var conn in NetworkServer.connections.Values)
    //    {
    //        var player = conn.identity.GetComponent<PlayerSetup>();
    //        if (player != null)
    //        {
    //            player.RunInGameStartPlayer(); // this should be a [ClientRpc]
    //        }
    //    }
    //}
}
