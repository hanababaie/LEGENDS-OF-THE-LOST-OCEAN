using Mirror;
using UnityEngine;

public class CheckNetworkIdentity : MonoBehaviour
{
    void Awake()
    {
        NetworkIdentity ni = GetComponent<NetworkIdentity>();
        if (ni != null)
        {
            Debug.Log($"NetworkIdentity پیدا شد روی {gameObject.name}. IsServer: {ni.isServer}, IsClient: {ni.isClient}");
        }
        else
        {
            NetworkIdentity niParent = GetComponentInParent<NetworkIdentity>(true);
            if (niParent != null)
            {
                Debug.Log($"NetworkIdentity پیدا شد روی والد {niParent.gameObject.name}");
            }
            else
            {
                Debug.LogError($"NetworkIdentity پیدا نشد روی {gameObject.name} و والدینش!");
            }
        }
    }
}