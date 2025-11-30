using Edgegap;
using Mirror;
using Steamworks;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public TMP_InputField addressField;

    //Steam stuff
    private Callback<LobbyCreated_t> randomGiberishVar1;
    private Callback<LobbyEnter_t> randomGiberishVar2;


    private void Awake()
    {
        randomGiberishVar1 = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        randomGiberishVar2 = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostButton.onClick.AddListener(HostButtonPressed);
        joinButton.onClick.AddListener(JoinButtonPressed);
    }

    void HostButtonPressed()
    {
        //start steam lobby
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, NetworkManager.singleton.maxConnections);
    }


    //Raw joining without steam friend list just steam64id
    void JoinButtonPressed()
    {
        if(addressField != null)
        {
            NetworkManager.singleton.networkAddress = addressField.text;

            // Load the game scene (blocking)
            SceneManager.LoadScene("GameScene");

            // Start Mirror client
            NetworkManager.singleton.StartClient();
            Debug.Log("Joining host via Steam invite: " + addressField.text);
        }
    }
    
    //host makes lobby
    void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult == EResult.k_EResultOK)
        {
            CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);

            Debug.Log("Lobby created successfully! Lobby ID: " + callback.m_ulSteamIDLobby);
            SteamMatchmaking.SetLobbyData(lobbyID, "HostName", SteamFriends.GetPersonaName());
            SteamMatchmaking.SetLobbyData(lobbyID, "HostSteamID", SteamUser.GetSteamID().m_SteamID.ToString());

            // Load the game scene (blocking)
            SceneManager.LoadScene("GameScene");

            // Start Mirror host after scene loads
            NetworkManager.singleton.StartHost();
        }
        else
        {
            Debug.LogError("Failed to make lobby : " + callback.m_eResult);
        }
    }

    //client joins lobby (prolly thru friend invite)
    void OnLobbyEntered(LobbyEnter_t callback)
    {
        CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);

        // Read the host SteamID stored in the lobby metadata
        string hostIDString = SteamMatchmaking.GetLobbyData(lobbyID, "HostSteamID");

        SceneManager.LoadScene("GameScene");

        //Join with Mirror
        NetworkManager.singleton.networkAddress = hostIDString;
        NetworkManager.singleton.StartClient();
        Debug.Log("Joining host via Steam invite: " + hostIDString);
    }
}
