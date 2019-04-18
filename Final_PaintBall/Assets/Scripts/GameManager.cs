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
    public GameObject timesUpPanel;
    public GameObject gameoverPanel;
    public Text counter;
    public Text gamecounter;

    GameObject[] pc;
   
    int timer = 5;
    int gameTimer = 65; //185 = 3mins

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
            gameTimer--;
            print(gameTimer);
        }
    }

    [ClientRpc]
    public void RpcCountdown()
    {
        countdownPanel.SetActive(true);
        StartCoroutine("Countdown");
    }

    [ClientRpc]
    public void RpcShowLeftTime()
    {
        timesUpPanel.SetActive(true);
    }

    [ClientRpc]
    public void RpcGameOver()
    {
        gameoverPanel.SetActive(true);
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
        gamecounter.text = "" + gameTimer;

        if (timer <= 0)
        {
            countdownPanel.SetActive(false);
            SetCanMove();
        }

        if (gameTimer <= 60)
        {
            RpcShowLeftTime();
        }

        if(gameTimer == 0)
        {
            StopCoroutine("Countdown");
            RpcGameOver();
        }

    }



}
