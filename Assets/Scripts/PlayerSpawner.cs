using UnityEngine;
using Photon.Pun;
using System;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public static Action<Player> onPlayerSpawned;
    [SerializeField] private GameObject _playerPrefab;
    public Player SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, FindSpawnPoint(), Quaternion.identity);
        onPlayerSpawned?.Invoke(player.GetComponent<Player>());
        return player.GetComponent<Player>();
    }

    private Vector3 FindSpawnPoint()
    {
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        int side = UnityEngine.Random.Range(0, 4); // 0 - left; 1 - right; 2 - top; 3 - bottom;
        Vector3 spawnPos = Vector3.zero;
        switch (side)
        {
            case 0:
                spawnPos.x = bottomLeft.x + 2;
                spawnPos.y = UnityEngine.Random.Range(topRight.y - 2, bottomLeft.y + 2);
                break;
            case 1:
                spawnPos.x = topRight.x - 2;
                spawnPos.y = UnityEngine.Random.Range(topRight.y - 2, bottomLeft.y + 2);
                break;
            case 2:
                spawnPos.x = UnityEngine.Random.Range(topRight.x - 2, bottomLeft.x + 2);
                spawnPos.y = topRight.y - 2;
                break;
            case 3:
                spawnPos.x = UnityEngine.Random.Range(topRight.x - 2, bottomLeft.x + 2);
                spawnPos.y = bottomLeft.y + 2;
                break;
        }

        return spawnPos;
    }
}
