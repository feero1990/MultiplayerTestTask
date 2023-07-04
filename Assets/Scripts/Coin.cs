using Photon.Pun;
using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool isCollected = false;
    public PhotonView view;
    public Action onCollected;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollected = true;
        view.RPC("SyncSetActive", RpcTarget.All);
        onCollected?.Invoke();
    }

    [PunRPC]
    public void SyncSetActive()
    {
        this.gameObject.SetActive(false);
    }
}
