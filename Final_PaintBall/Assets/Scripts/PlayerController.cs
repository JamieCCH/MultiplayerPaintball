//////Jamie Chingchun Huang
//////101088322
//////FinalPaintBall
//////Apr 18, 2019

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]

public class PlayerController : NetworkBehaviour {

    private Rigidbody m_rb = null;

    public bool m_canAttack = false;
    public bool m_colorchanged = false;
    public bool m_callMove = false;
    public bool m_isLocalCamera = false;

    public float m_speed = 10.0f;
    public float m_bulletSpeed = 35.0f;
    private float attackCooldownTime = 0.5f;
    private float currentAttackTimer = -0.25f;
    private float colorChangeBackTime = 0.5f;
    private float currentColorTime = -0.25f;

    public Transform m_bulletTransform;
    public GameObject m_bullet = null;
    public GameObject pickup = null;
    public Camera playerCamera = null;
    public GameObject glasses = null;


    public int m_score = 0;

    [SyncVar]
    public Color m_startingColour;


    void Start()
    {
        playerCamera.enabled = false;
        m_rb = GetComponent<Rigidbody>();
        GetComponent<Renderer>().material.color = m_startingColour;
        Vector3 spawnPos = transform.position + (transform.forward * 3.5f);
        GameObject _pickup = Instantiate(pickup, spawnPos, transform.rotation);
        NetworkServer.Spawn(_pickup);
    }

    void ChangeColor(Color c)
    {
        if (currentColorTime < 0.0f)
        {
            GetComponent<Renderer>().material.color = c;  //glasses
            m_colorchanged = true;
            currentColorTime = colorChangeBackTime;
        }
    }

    void UpdateColor()
    {
        if (currentColorTime < 0.0f)
        {
            return;
        }

        Color color = GetComponent<Renderer>().material.color; //glasses
        Vector3 sourceColour = new Vector3(color.r, color.g, color.b);
        Vector3 destColour = new Vector3(m_startingColour.r, m_startingColour.g, m_startingColour.b);
        //Vector3 destColour = new Vector3(1.0f, 1.0f, 1.0f); //glasses color
        currentColorTime -= Time.deltaTime;
        float ratio = 1.0f - Mathf.Clamp(currentColorTime / attackCooldownTime, 0.0f, 1.0f);
        Vector3 vColour = Vector3.Lerp(sourceColour, destColour, ratio);
        color.r = vColour.x;
        color.g = vColour.y;
        color.b = vColour.z;
        GetComponent<Renderer>().material.color = color;    //glasses
    }

    [ClientRpc]
    void RpcColor(Color c)
    {
        //local
        ChangeColor(c);
    }

    [Command]
    void CmdColor(Color c)
    {
        //local
        ChangeColor(c);
        //broadcast
        RpcColor(c);
    }

    void Attack()
    {
        if (m_canAttack && currentAttackTimer < 0.0f)
        {
            GameObject bullet = Instantiate(m_bullet, m_bulletTransform.position, transform.rotation);
            Rigidbody rb = bullet.AddComponent<Rigidbody>();
            rb.velocity = bullet.transform.forward * m_bulletSpeed;
            bullet.GetComponent<Renderer>().material.color = m_startingColour;
            NetworkServer.Spawn(bullet);
            Destroy(bullet, 1.0f);
            currentAttackTimer = attackCooldownTime;
        }
    }

    void UpdateAttack()
    {
        if (currentAttackTimer < 0.0f)
        {
            return;
        }
        currentAttackTimer -= Time.deltaTime;
    }


    [ClientRpc]
    void RpcAttack()
    {
        //local attack
        Attack();
    }

    [Command]
    void CmdAttack()
    {
        //local attack
        Attack();
        //broadcast attack
        RpcAttack();
    }

    //striclty for replicas
    void ReplicaUpdate()
    {
       UpdateAttack();
       UpdateColor();
    }

    void Update()
    { 
        if (!hasAuthority)
        {
            ReplicaUpdate();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isServer)
            {
                RpcAttack();
            }
            else
            {
                CmdAttack();
            }
        }

        UpdateAttack();
        UpdateColor();

        if (m_callMove)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 forward = moveVertical * transform.forward * m_speed;
            Vector3 strafe = moveHorizontal * transform.right;
            m_rb.velocity = forward + strafe;
            transform.Rotate(0, moveHorizontal, 0);
        }

        if(m_isLocalCamera)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
            playerCamera.enabled = true;
        }
    }

    void SendBulletColor(Color c)
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.SetScore(c);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pickup"))
        {
            m_canAttack = true;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Color col = collision.gameObject.GetComponent<Renderer>().material.color;
            CmdColor(col);
            Destroy(collision.gameObject);
            SendBulletColor(col);
        }
    }
}
