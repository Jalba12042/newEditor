using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour
{
    [Tooltip("The player prefab to spawn.")]
    [SerializeField] private GameObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // Only the server should handle spawning
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned in PlayerSpawner.");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, GetSpawnPosition(clientId), Quaternion.identity);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    private Vector3 GetSpawnPosition(ulong clientId)
    {
        // Example: Assign different spawn points based on client ID
        float offset = clientId * 2.0f;
        return new Vector3(offset, 0, 0); // Adjust as needed
    }

    public override void OnDestroy()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayer;
        }
    }
}
