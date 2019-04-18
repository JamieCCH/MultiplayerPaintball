using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour{

    public Button m_joinButton;
    public GameObject panel;
    public GameObject gameFullPanel;
    public GameObject countdownPanel;
    public Camera lobbyCamera;
    //private GameObject[] playerCamera;

    GameObject[] pc;
    public Text counter;
    int timer = 3;

    GameObject player;

    public int playerNum = 0;

    void Start()
    {
        m_joinButton.onClick.AddListener(AddPlayer);
        Time.timeScale = 1;
    }

    void AddPlayer()
    {
        ClientScene.AddPlayer(0);
        panel.SetActive(false);
    }

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
    public void RpcCheckPlayerNum()
    {
        playerNum = 1;
    }

    IEnumerator Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timer--;
        }
    }

    [ClientRpc]
    public void RpcCountdown()
    {
        countdownPanel.SetActive(true);
        StartCoroutine("Countdown");
    }

 
    void SetCanMove()
    {
        pc = GameObject.FindGameObjectsWithTag("Player");
        for (var i = 0; i < pc.Length; i++)
        {
            pc[i].GetComponent<PlayerController>().m_callMove = true;
            pc[i].GetComponent<PlayerController>().m_isLocalCamera = true;
        }
    }


    private void Update()
    {
        counter.text = ("" + timer);

        if (timer <= 0)
        {
            countdownPanel.SetActive(false);
            SetCanMove();
        }
    }



}
