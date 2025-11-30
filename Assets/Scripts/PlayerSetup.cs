using UnityEngine;
using Mirror;
using Unity.Cinemachine;

public class PlayerSetup : NetworkBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerHead;

    private CinemachineCamera cinemaCam;
    private GameObject menuCanvas;
    public override void OnStartLocalPlayer()
    {
        //Spawn in the camera prefab here!!
        GameObject cam = Instantiate(cameraPrefab);

        CameraFollowPlayer(cam);

        //Hide the menu canvas
        HideMenuCanvas();
        //Also Hide Cursor
        HideCursor();
        //Hide the first camera in the scene
        HideSceneCamera();
    }

    void CameraFollowPlayer(GameObject cam)
    {
        cinemaCam = cam.GetComponentInChildren<CinemachineCamera>();
        cinemaCam.Follow = playerHead.transform;

        //Assign Cam to Follow player's head in Player movement
        GetComponent<PlayerMovement>().cam = cinemaCam.transform;
    }

    void HideSceneCamera()
    {
        GameObject sceneCamera = GameObject.Find("SceneCamera");
        sceneCamera.SetActive(false);
    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void HideMenuCanvas()
    {
        menuCanvas = GameObject.Find("Canvas");
        menuCanvas.SetActive(false);
    }
}
