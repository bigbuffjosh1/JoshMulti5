using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public Button ReadyButton;
    public Button StartButton;
    public Button InviteButton;
    public Button QuitButton;
    public Button SwapRoleButton;

    public TMP_Text p1Text;
    public TMP_Text p2Text;

    private PlayerManager playerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReadyButton.onClick.AddListener(ReadyButtonPressed);
        StartButton.onClick.AddListener(StartButtonPressed);
        //InviteButton.onClick.AddListener(InviteButtonPressed);
        //QuitButton.onClick.AddListener(QuitButtonPressed);
        //SwapRoleButton.onClick.AddListener(SwapRoleButtonPressed);
    }

    //This is called when playerManager loads in!
    public void SetLocalPlayerManager(PlayerManager player)
    {
        playerManager = player;

        //UpdateLobbyText();
    }
    void ReadyButtonPressed()
    {
        playerManager.ToggleReady();
    }

    void StartButtonPressed()
    {
        if (!playerManager.isLocalPlayer) return;

        playerManager.CmdRequestStartGame();
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
}
