using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Coin> _coinList;
    [SerializeField] private GUI_Game _gui;
    private int _coins;
   
    private void Start()
    {
        _gui.UpdateCoinsBar(0);
        foreach (var coin in _coinList)
        {
            coin.onCollected += () =>
            {
                _coins++;
                _gui.UpdateCoinsBar(_coins);
                kek(_coins);
            };
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // if (PhotonNetwork.IsMasterClient)
        {
            foreach (var coin in _coinList)
                if (coin.isCollected == true)
                    coin.view.RPC("SyncSetActive", RpcTarget.Others);
        }
    }
   
    private void kek(int value)
    {

        string nick = PhotonNetwork.LocalPlayer.NickName;
        nick = nick.Substring(0, 9) + value.ToString();
        PhotonNetwork.LocalPlayer.NickName = nick;
        //Debug.Log(nick);
    }
}
