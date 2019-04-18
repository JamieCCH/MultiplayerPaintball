using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PickupComponent : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [ClientRpc]
    void RpcPlayerCollision(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if(pc.hasAuthority)
        {
            pc.m_canAttack = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hasAuthority)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            RpcPlayerCollision(collision.gameObject);
            Debug.Log("Pickup collision w/ Player");
        }
    }
}
