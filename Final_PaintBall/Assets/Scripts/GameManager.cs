//////Jamie Chingchun Huang
//////101088322
//////FinalPaintBall
//////Apr 18, 2019

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
    public Text scoreText;

    GameObject[] pc;
   
    int timer = 3;
    int gameTimer = 183; //183 = 3mins

    public int playerNum = 0;

    public Dictionary<Color, int> PlayerScore;

    void Start()
    {
        m_joinButton.onClick.AddListener(AddPlayer);
        Time.timeScale = 1;

        PlayerScore = new Dictionary<Color, int>
        {
            { Color.red, 0 },
            { Color.green, 0 },
            { Color.blue, 0 }
        };
    }


    [ClientRpc]
    public void RpcShowScore(Color c, int s)
    {
        for (var i = 0; i < pc.Length; i++)
        {
            var player = pc[i].GetComponent<PlayerController>();
            if (player.m_startingColour == c && player.hasAuthority)
            {
                scoreText.text = "" + s;
                Debug.Log(c + " : " + s);
            }
        }
    }

    [Command]
    public void CmdCountScore(Color c)
    {
        PlayerScore[c]++;
        RpcShowScore(c, PlayerScore[c]);
    }
    
    public void SetScore(Color c)
    {
        CmdCountScore(c);
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
