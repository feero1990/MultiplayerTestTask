using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Game : MonoBehaviour
{

    [SerializeField] private Image _hpBarFillImage;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _alivePlayersText;
    [SerializeField] private Button _fireButton;

    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;

    public Button FireButton { get; private set; }

    private void Awake()
    {
        FireButton = _fireButton;
    }

    public void UpdateHpBar(float maxHp, float curHp)
    {
        _hpBarFillImage.fillAmount = curHp / maxHp;
    }
    public void UpdateCoinsBar(int curCoins)
    {
        _coinsText.text = curCoins.ToString();
    }

    public void ShowLoseScreen()
    {
        _losePanel.SetActive(true);
    }
    public void ShowWinScreen()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        _winPanel.SetActive(true);
        foreach (var p in players)
        {
            // Debug.Log(p.NickName);
            if (p.NickName != "rip")
            {
                if (p.NickName.Length > 9)
                    _winPanel.GetComponentInChildren<TMP_Text>().text =
              "Пебедил игрок: " + p.NickName.Substring(0, 9) + "\n Собрано монет: " + p.NickName.Substring(9);
                else
                    _winPanel.GetComponentInChildren<TMP_Text>().text =
              "Пебедил игрок: " + p.NickName.Substring(0, 9) + "\n Собрано монет: 0";
            }

        }
    }

    public void UpdateAlivePlayersText(int newCount)
    {
        _alivePlayersText.text = "Alive players: " + newCount.ToString();
    }
}
