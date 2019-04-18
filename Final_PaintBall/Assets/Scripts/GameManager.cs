using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour{

    public Button m_joinButton;
    public GameObject panel;
    public GameObject gameFullPanel;

    //[SyncVar]
    public int playerNum = 0;

    void Start()
    {
        m_joinButton.onClick.AddListener(AddPlayer);
        //panel = GameObject.Find("StartPanel");
    }

    void AddPlayer()
    {
        ClientScene.AddPlayer(0);
        //m_joinButton.gameObject.SetActive(false);
        panel.SetActive(false);
    }

    //void CheckPlayerCount()
    //{
    //    if (playerCount > 1)
    //    {
    //        print("playerCount>=3");
    //        panel.SetActive(true);
    //        gameFullPanel.SetActive(true);
    //    }
    //}

    [ClientRpc]
    public void RpcGameFullNotif()
    {
        if(playerNum == 0)
        {
            panel.SetActive(true);
            gameFullPanel.SetActive(true);
        }
    }

    [ClientRpc]
    public void RpcSetPlayerNum()
    {
        playerNum = 1;
    }


    //[ClientRpc]
    //void RpcCheckPlayer()
    //{
    //    //local
    //    CheckPlayerCount();
    //}

    //[Command]
    //void CmdCheckPlayer()
    //{
    //    //local
    //    CheckPlayerCount();
    //    //broadcast
    //    //RpcCheckPlayer();
    //}


    private void Update()
    {
        //print("local "+isLocalPlayer);
       
        //if (!isServer)
        //{
        //    CheckPlayerCount();
        //}
        //RpcCheckPlayer();
        //CheckPlayerCount();
    }
}
