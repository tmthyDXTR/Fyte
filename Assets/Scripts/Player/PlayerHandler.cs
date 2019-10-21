using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private PhotonView PV;
    private PlayerMovement PM;
    private PlayerAttack PA;
    private PlayerHitBox PHB;
    private PlayerParryBox PPB;
    [HideInInspector] public Animator myAnim;
    public GameObject myStatPanel;
    public GameObject enemyStatPanel;

    [Header("Stats")]
    public int healthMax;
    public int health;
    public int staminaMax;
    public int stamina;

    public int staminaPerTick;
    public float staminaTickTime;
    private float staminaTicker = 0.000f;

    [Header("Bools")]
    public bool canRun = true;
    public bool canAttack = true;
    public bool canParry = true;
    public bool canJump = true;

    public bool isRunning;
    public bool isAttacking;
    public bool isParrying;
    public bool isJumping;
    public bool isHit = false;
    public bool isRegenStamina = false;
    public bool isDead = false;
    public bool enemyFound = false;
    public Transform enemy;


    [SerializeField] private bool initialized = false;


    void Start()
    {
        PV = GetComponent<PhotonView>();
        PM = GetComponent<PlayerMovement>();
        PA = GetComponent<PlayerAttack>();
        PHB = GetComponent<PlayerHitBox>();
        PPB = GetComponent<PlayerParryBox>();
        myAnim = GetComponent<Animator>();

        
        PV.RPC("RPC_InitStats", RpcTarget.AllBuffered, 10, 10);
        if (PV.IsMine)
        {
            myStatPanel = GameObject.Find("StatPanel_1");
            myStatPanel.GetComponent<PanelHandler>().PH = this;
            enemyStatPanel = GameObject.Find("StatPanel_2");
        }
    }

    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            if (canRun)
            {
                MovePlayer();
            }
            if (PA.attack1 && canAttack && !isAttacking)
            {
                Attack();
            }
            if (PHB.hitBox.isBusy && !isHit)
            {
                TakeDamage();
            }
            if (PPB.parry && canParry)
            {
                Parry();
            }
            if (PM.jump && canJump)
            { 
                Jump();
            }
            if (health <= 0 && !isDead)
            {
                Death();
            }
            if (!enemyFound)
            {
                SearchEnemyPlayer();
            }
            if (stamina < staminaMax && !isRunning && !isAttacking && !isJumping && !isParrying)
            {
                RegenStamina();
            }
            else
            {
                isRegenStamina = false;
            }
        }
    }

    private void RegenStamina()
    {
        isRegenStamina = true;
        staminaTicker += Time.deltaTime;
        if (staminaTicker >= staminaTickTime)
        {
            PV.RPC("RPC_AddStat", RpcTarget.AllBuffered, "Stamina", staminaPerTick);
            staminaTicker -= staminaTicker;
        }

    }

    private void Death()
    {
        PV.RPC("RPC_StopMovement", RpcTarget.All);
        PV.RPC("RPC_Death", RpcTarget.All);
    }

    private void Jump()
    {
        PV.RPC("RPC_ApplyJump", RpcTarget.All);
    }

    private void Parry()
    {
        PV.RPC("RPC_StopMovement", RpcTarget.All);
        PV.RPC("RPC_StartParry", RpcTarget.All);
    }

    private void TakeDamage()
    {
        PV.RPC("RPC_StopMovement", RpcTarget.All);
        PV.RPC("RPC_GetHit", RpcTarget.All, PHB.hitting.damage);
    }

    private void Attack()
    {        
        PV.RPC("RPC_StopMovement", RpcTarget.All);
        PV.RPC("RPC_StartAttack", RpcTarget.All);                   
    }

    private void MovePlayer()
    {        
        PM.Move();
        if (PM.currentVelocity > 0.01f)
        {            
            if (!myAnim.GetBool("IsRunning"))
            {
                isRunning = true;

                myAnim.SetBool("IsRunning", true);
            }            
        }                    
        else
        {
            PV.RPC("RPC_StopMovement", RpcTarget.All);
            isRunning = false;
        }
    }

    void SearchEnemyPlayer()
    {
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            Debug.Log("Waiting for other player");
        }
        else
        {            
            enemy = GameObject.Find("PlayerAvatar(Clone)").transform.GetChild(0);
            enemyStatPanel.GetComponent<PanelHandler>().PH = enemy.GetComponent<PlayerHandler>();
            enemyFound = true;           
        }


    }
    [PunRPC]
    void RPC_AddStat(string stat, int amount)
    {
        if (stat == "Health")
        {
            health += amount;
        }
        if (stat == "Stamina")
        {
            stamina += amount;
        }
        Debug.Log(PV.ViewID + " got " + amount + " " + stat);
    }
    [PunRPC]
    void RPC_RemoveStat(string stat, int amount)
    {
        if (stat == "Health")
        {
            health -= amount;
        }
        if (stat == "Stamina")
        {
            stamina -= amount;
        }
        Debug.Log(PV.ViewID + " lost " + amount + " " + stat);
    }

    [PunRPC]
    void RPC_Death()
    {
        myAnim.SetBool("IsRunning", false);
        myAnim.SetBool("IsAttacking", false);
        myAnim.SetBool("IsParrying", false);
        myAnim.SetBool("IsJumping", false);
        myAnim.SetBool("IsHit", false);
        myAnim.SetBool("IsDead", true);
        canRun = false;
        isRunning = false;
        canAttack = false;
        isAttacking = false;
        canParry = false;
        isParrying = false;
        canJump = false;
        isJumping = false;
        isHit = false;
        isDead = true;
    }

    [PunRPC]
    void RPC_InitStats(int hpStartValue, int staminaStartValue)
    {
        Debug.Log(PV.ViewID + " Stats Initialized");
        healthMax = hpStartValue;
        health = hpStartValue;
        staminaMax = staminaStartValue;
        stamina = staminaStartValue;
        initialized = true;
    }


}