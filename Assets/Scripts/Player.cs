using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour, IDamagable
{
    public AliveCounter aliveCounter;

    public float maxHp = 100;
    public float curHp;
    // public int coins;
    // public Action<Player> onDie;


    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePointTransform;

    private DynamicJoystick _dynamicJoystick;
    private PhotonView _view;

    private GUI_Game _gui;

    private Vector2 _bottomLeft;
    private Vector2 _topRight;

    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    private float _moveSpeed;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _dynamicJoystick = FindObjectOfType<DynamicJoystick>();
        _gui = FindObjectOfType<GUI_Game>();
        curHp = maxHp;
        PhotonNetwork.LocalPlayer.NickName = "Ubivator" + UnityEngine.Random.Range(1, 10);
        if (_view.IsMine == true)
        {
            GetComponent<SpriteRenderer>().material.color = Color.green;

            _gui.UpdateHpBar(maxHp, curHp);
            _gui.FireButton.onClick.AddListener(Fire);
        }
        else
            GetComponent<SpriteRenderer>().material.color = Color.red;

        _bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        _maxX = _topRight.x;
        _minX = _bottomLeft.x;

        _maxY = _topRight.y;
        _minY = _bottomLeft.y;

        _moveSpeed = Time.deltaTime * 2;
    }
    private void OnDestroy()
    {
        _gui.FireButton.onClick.RemoveListener(Fire);
    }


    void Update()
    {
        if (_view.IsMine == false)
            return;

        Vector3 nextPos = transform.position + (Vector3.right * _dynamicJoystick.Horizontal +
        Vector3.up * _dynamicJoystick.Vertical) * _moveSpeed;

        if (nextPos.x < _maxX && nextPos.x > _minX && nextPos.y < _maxY && nextPos.y > _minY)
            transform.position = nextPos;

        if (_dynamicJoystick.Vertical + _dynamicJoystick.Horizontal != 0)
        {
            float angle = Mathf.Atan2(_dynamicJoystick.Vertical, _dynamicJoystick.Horizontal) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void RecieveDamage(float damage)
    {
        curHp += damage;

        if (_view.IsMine == true)
        {
            _gui.UpdateHpBar(maxHp, curHp);

            if (curHp <= 0)
            {
                //onDie?.Invoke(this);
                PhotonNetwork.LocalPlayer.NickName = "rip";
                aliveCounter.DecreaseAlivePlayerCount();
                _gui.ShowLoseScreen();
                _view.RPC("SyncPlayerDestroy", RpcTarget.All);
            }
        }
    }

    public void Fire()
    {
        var newBullet = PhotonNetwork.Instantiate(_bulletPrefab.name, _firePointTransform.position, _firePointTransform.rotation).GetComponent<Bullet>();
        newBullet.creatorID = PhotonNetwork.LocalPlayer.ActorNumber;
        newBullet.damage = 10;
    }


    [PunRPC]
    private void SyncPlayerDestroy()
    {
        Destroy(this.gameObject);
    }
}
