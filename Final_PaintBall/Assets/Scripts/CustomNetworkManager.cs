using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    int maxPlayerNum = 3;        //require: 3
    int currentPlayerNum = 0;
    bool isGamefull = false;
    float x;
    float y;
    float z;
    Vector3 pos;
    Vector3[] posArr;

    public GameObject gameManager;
    GameObject StartPanel;

    Color red = Color.red;
    Color green = Color.green;
    Color blue = Color.blue;

    List<Color> colorList;
    Dictionary<Color, int> PlayerScore;
   

    public override void OnStartServer()
    {
        base.OnStartServer();
        //Debug.Log("OnStartServer");
        colorList = new List<Color>() { red, green, blue };

        //PlayerScore = new Dictionary<Color, int>
        //{
        //    { Color.red, 3 },
        //    { Color.green, 4 },
        //    { Color.blue, 0 }
        //};
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("OnServerConnect");
        gameManager.GetComponent<GameManager>().m_joinButton.gameObject.SetActive(true);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        gameManager.GetComponent<GameManager>().m_joinButton.gameObject.SetActive(true);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //Debug.Log("colorList" + colorList.Count);
        //Debug.Log("left:"+conn.playerControllers[0].gameObject.GetComponent<PlayerController>().m_startingColour);

        var left = conn.playerControllers[0].gameObject.GetComponent<PlayerController>().m_startingColour;
        colorList.Insert(0,left);

        if (currentPlayerNum > 0 && currentPlayerNum <= maxPlayerNum)
        {
            currentPlayerNum--;
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //x = Random.Range(-20.0f, 20.0f);
        y = 0.5f;
        //z = Random.Range(-25.0f, 5.0f);
        //pos = new Vector3(x, y, z);
       
        posArr = new[] {
             new Vector3(0f, y, -22f),
             new Vector3(-20f, y, 5f),
             new Vector3(20f, y, -5f),
             };


        currentPlayerNum++;

        if (currentPlayerNum <= maxPlayerNum)
        {
            isGamefull = false;
            GameObject player = (GameObject)Instantiate(playerPrefab, posArr[currentPlayerNum-1],transform.rotation);
            player.GetComponent<PlayerController>().m_startingColour = colorList[0];
            colorList.RemoveAt(0);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
         
            gameManager.GetComponent<GameManager>().RpcCheckPlayerNum();
        }
        else
        {
            isGamefull = true;
        }

        if (currentPlayerNum == maxPlayerNum)
        {
            gameManager.GetComponent<GameManager>().RpcCountdown();
        }

        var playerCounts = gameManager.GetComponent<GameManager>().playerNum;
        print("playerNum " + gameManager.GetComponent<GameManager>().playerNum);

        if (isGamefull && playerCounts == 1)
        {
            gameManager.GetComponent<GameManager>().RpcGameFullNotif();
        }

       
    }
    
}
