using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Lobby : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputFieldJoin;
    public TMP_InputField inputFieldCreate;
    public GameObject popupPanel;
    public TMP_Text popupText;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(inputFieldCreate.text, roomOptions);
        //PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputFieldJoin.text);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            popupText.text = "≈хала!";
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnCreatedRoom()
    {
        popupPanel.SetActive(true);
        popupText.text = "ќжидание другого игрока...";
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            PhotonNetwork.LoadLevel("Game");
    }
}
