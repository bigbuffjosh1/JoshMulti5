using UnityEngine;
using Mirror;
using Unity.Cinemachine;

public class PlayerSetup : NetworkBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerHead;

    private GameObject sceneCamera;
    private CinemachineCamera cinemaCam;
    private GameObject menuCanvas;
    public override void OnStartLocalPlayer()
    {
        //Handle the camera
        GameObject cam = Instantiate(cameraPrefab);

        sceneCamera = GameObject.Find("SceneCamera");
        sceneCamera.SetActive(false);

        cinemaCam = cam.GetComponentInChildren<CinemachineCamera>();
        cinemaCam.Follow = playerHead.transform;

        //Assign Cam to Follow player's head in Player movement
        GetComponent<PlayerMovement>().cam = cinemaCam.transform;

        //Hide the menu canvas
        menuCanvas = GameObject.Find("Canvas");
        menuCanvas.SetActive(false);

    }
}
