using Photon.Pun;
using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    //public HitData hitData;
    public int creatorID;
    public float damage;

    private Collider2D _collider;
    private PhotonView _view;

    [PunRPC]
    private void SyncBullet(int creatorID, float damage)
    {
        this.damage = damage;
        this.creatorID = creatorID;
    }

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _collider = GetComponent<Collider2D>();
        _view.RPC("SyncBullet", RpcTarget.All, creatorID, damage);

        StartCoroutine(DestroyCoroutine());
    }

    private void Update()
    {
        transform.Translate(Vector2.right * 4 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        IDamagable victim = collision.gameObject.GetComponent<IDamagable>();

        if (victim != null)
        {
            if (collision.gameObject.GetComponent<PhotonView>().Owner.ActorNumber != creatorID)
            {
                victim.RecieveDamage(-damage);
            }
        }

        _view.RPC("SyncDestroy", RpcTarget.All);
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(5);
        _view.RPC("SyncDestroy", RpcTarget.All);
    }
    [PunRPC]
    private void SyncDestroy()
    {
        Destroy(this.gameObject);
    }
}
