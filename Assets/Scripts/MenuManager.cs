using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public TMP_InputField addressField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostButton.onClick.AddListener(HostButtonPressed);
        joinButton.onClick.AddListener(JoinButtonPressed);
    }

    void HostButtonPressed()
    {
        //start host
        NetworkManager.singleton.StartHost();
        Debug.Log("Hosting");
    }

    void JoinButtonPressed()
    {
        if(addressField != null)
        {
            //start the client
            NetworkManager.singleton.networkAddress = addressField.text;
            NetworkManager.singleton.StartClient();
            Debug.Log("Joining");
        }
    }
}
