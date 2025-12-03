using Mirror;
using UnityEngine;

public class HidePlayerLayer : NetworkBehaviour
{
    [Tooltip("Layer to assign to hide from local player")]
    public string hideLayerName = "HideFromPlayer";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (!isLocalPlayer) return; // only hide for your own player

        int layer = LayerMask.NameToLayer(hideLayerName);
        if (layer == -1)
        {
            Debug.LogError($"Layer '{hideLayerName}' does not exist!");
            return;
        }

        SetLayerRecursively(gameObject, layer);
    }
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
