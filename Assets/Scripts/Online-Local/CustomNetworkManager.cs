using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System.IO;
public class CustomNetworkManager : NetworkManager
{
    [System.Serializable]
    public class PlayerSetup
    {
        public GameObject playerPrefab;
        public Transform spawnPoint;
    }

    [System.Serializable]
    public class ScenePlayerSetup
    {
        public string sceneName;
        public List<PlayerSetup> playerSetups = new List<PlayerSetup>();
    }

    public List<ScenePlayerSetup> sceneSetups = new List<ScenePlayerSetup>();

    private List<PlayerSetup> currentPlayerSetups;
    private int connectionIndex = 0;

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        string justSceneName = Path.GetFileNameWithoutExtension(sceneName);
        Debug.Log("Scene Changed To: " + justSceneName);

        foreach (var setup in sceneSetups)
        {
            if (setup.sceneName == justSceneName)
            {
                currentPlayerSetups = setup.playerSetups;
                connectionIndex = 0;
                Debug.Log("Setup found for scene: " + justSceneName);
                break;
            }
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (currentPlayerSetups == null || currentPlayerSetups.Count == 0)
        {
            Debug.LogError("No player setups defined for this scene!");
            return;
        }

        // اگر بیشتر از تعداد تعریف‌شده بازیکن وصل شدن
        if (connectionIndex >= currentPlayerSetups.Count)
        {
            Debug.LogWarning("Too many players joined, ignoring extra connections.");
            return;
        }

        PlayerSetup setup = currentPlayerSetups[connectionIndex];

        GameObject player = Instantiate(setup.playerPrefab, setup.spawnPoint.position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);

        connectionIndex++;
    }
}