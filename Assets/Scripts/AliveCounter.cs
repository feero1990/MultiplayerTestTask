using Photon.Pun;
using System;
using UnityEngine;

public class AliveCounter : MonoBehaviourPunCallbacks
{
    [SerializeField] private GUI_Game _gui;
    private int alivePlayerCount;

    public int AlivePlayerCount { get; }
    public Action<int> onAliveCounChanged;

    private void Start()
    {
        IncreaseAlivePlayerCount();
    }

    [PunRPC]
    private void ChangeAliveCount(int value)
    {
        alivePlayerCount += value;
        photonView.RPC("SyncAlivePlayerCount", RpcTarget.Others, alivePlayerCount);
        photonView.RPC("UpdateAlivePlayerCount", RpcTarget.All);
        onAliveCounChanged?.Invoke(alivePlayerCount);
    }

    public void IncreaseAlivePlayerCount()
    {
        photonView.RPC("ChangeAliveCount", RpcTarget.MasterClient, 1);
    }
    public void DecreaseAlivePlayerCount()
    {
        photonView.RPC("ChangeAliveCount", RpcTarget.MasterClient, -1);
    }

    [PunRPC]
    private void UpdateAlivePlayerCount()
    {
        _gui.UpdateAlivePlayersText(alivePlayerCount);
    }
    [PunRPC]
    private void SyncAlivePlayerCount(int newCount)
    {
        alivePlayerCount = newCount;
    }
}
