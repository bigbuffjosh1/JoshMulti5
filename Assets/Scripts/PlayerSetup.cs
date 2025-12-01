using UnityEngine;
using Mirror;
using Unity.Cinemachine;
using Mirror.Examples.AdditiveLevels;

public class PlayerSetup : NetworkBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerHead;
    public LobbyManager lobbyManager;
    public PlayerMovement playerMovement;

    private CinemachineCamera cinemaCam;
    private GameObject sceneCamera;
    private GameObject menuCanvas;
    public override void OnStartLocalPlayer()
    {
        lobbyManager = Instantiate(lobbyManager);
    }
    public void RunInGameStartPlayer()
    {
        //Game Starts Logic

        HideMenuCanvas();
        HideCursor();
        HideSceneCamera();
        SpawnInCinemaCam();
        GetComponent<PlayerManager>().inGame = true;
    }

    void SpawnInCinemaCam()
    {
        //Spawn in the camera prefab here!!
        GameObject cam = Instantiate(cameraPrefab);
        CameraFollowPlayer(cam);
        //cam.SetActive(true);
    }

    void HideSceneCamera()
    {
        GameObject.Find("SceneCamera").SetActive(false);
    }

    void CameraFollowPlayer(GameObject cam)
    {
        cinemaCam = cam.GetComponentInChildren<CinemachineCamera>();
        cinemaCam.Follow = playerHead.transform;

        //Assign Cam to Follow player's head in Player movement
        GetComponent<PlayerMovement>().cam = cinemaCam.transform;
    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void HideMenuCanvas()
    {
        menuCanvas = GameObject.Find("Canvas");
        if(menuCanvas!=null)
            menuCanvas.SetActive(false);
    }
}
