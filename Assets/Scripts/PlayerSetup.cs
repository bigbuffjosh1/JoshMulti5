using UnityEngine;
using Mirror;
using Unity.Cinemachine;

public class PlayerSetup : NetworkBehaviour
{
    public GameObject cameraPrefab;
    public GameObject playerHead;

    private CinemachineCamera cinemaCam;
    public override void OnStartLocalPlayer()
    {
        GameObject cam = Instantiate(cameraPrefab);

        cinemaCam = cam.GetComponentInChildren<CinemachineCamera>();
        cinemaCam.Follow = playerHead.transform;

        GetComponent<PlayerMovement>().cam = cinemaCam.transform;
    }
}
