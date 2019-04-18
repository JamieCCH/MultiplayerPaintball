using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BulletComponent : NetworkBehaviour
{
    void Start()
    {
    }

    [ClientRpc]
    void RpcPlayerCollision(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();

        //print(pc.hasAuthority + " score: " + pc.score);

        if (!pc.hasAuthority)
        {
            //++pc.score;
            //pc.scoreTxt.text = "Score: " + pc.score;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!hasAuthority)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            RpcPlayerCollision(collision.gameObject);
            Debug.Log("Bullet collision w/ Player");
        }
    }
}
