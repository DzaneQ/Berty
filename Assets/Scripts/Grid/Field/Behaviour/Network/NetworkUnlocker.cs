using Berty.Grid.Field.Behaviour;
using Unity.Netcode;
using UnityEngine;

public class NetworkUnlocker : NetworkBehaviour
{
    private FieldBehaviour toUnlock;

    private void Awake()
    {
        toUnlock = GetComponent<FieldBehaviour>();
    }

    public override void OnNetworkSpawn()
    {
        toUnlock.enabled = true;
        Destroy(this);
    }
}
