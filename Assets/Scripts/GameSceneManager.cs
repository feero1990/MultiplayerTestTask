using Photon.Pun;
using UnityEngine;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private GUI_Game _gui;
    [SerializeField] private AliveCounter _aliveCounter;

    private bool _isGameStarted = false;

    private void Start()
    {
        Player player = _playerSpawner.SpawnPlayer();
        player.aliveCounter = _aliveCounter;

        _aliveCounter.onAliveCounChanged += (int value) =>
        {
            if (value > 1 && _isGameStarted == false)
                _isGameStarted = true;
            else if (value == 1 && _isGameStarted == true)
                photonView.RPC("SyncShowWinScreen", RpcTarget.All);
            // Debug.Log(value);
        };
    }

    [PunRPC]
    private void SyncShowWinScreen()
    {
        _gui.ShowWinScreen();
    }
}
