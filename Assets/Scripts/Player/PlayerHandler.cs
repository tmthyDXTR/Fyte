using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private PhotonView PV;
    private PlayerMovement PM;
    private PlayerAttack PA;
    private PlayerHitBox PHB;
    private Animator myAnim;

    [Header("Stats")]
    public int healthMax;
    public int health;
    [Header("Bools")]
    public bool canRun = true;
    public bool isRunning;
    public bool canAttack = true;
    public bool isAttacking;
    public bool isHit = false;

    [SerializeField] private bool initialized = false;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        PM = GetComponent<PlayerMovement>();
        PA = GetComponent<PlayerAttack>();
        PHB = GetComponent<PlayerHitBox>();
        myAnim = GetComponent<Animator>();

        if (PV.IsMine)
        {
            PV.RPC("InitStats", RpcTarget.All, 100);
            initialized = true;
        }
    }

    void Update()
    {
        if (PV.IsMine)
        {
            if (canRun)
            {
                MovePlayer();
            }
            Attack();
            if (PHB.hitting != null && !isHit)
            {
                PV.RPC("RPC_GetHit", RpcTarget.All, PHB.hitting.damage);
            }
        }

    }

    private void Attack()
    {
        if (PA.attack1 && canAttack && !isAttacking)
        {
            isAttacking = true;
            isRunning = false;
            canAttack = false;
            canRun = false;
            myAnim.SetBool("IsRunning", false);
            myAnim.SetBool("IsAttacking", true);
            myAnim.Play("2Hand-Sword-Attack1");
        }
    }

    private void MovePlayer()
    {        
        PM.Move();
        if (PM.currentVelocity > 0.01f)
        {
            isRunning = true;
            myAnim.SetBool("IsRunning", true);
        }                    
        else
        {
            isRunning = false;
            myAnim.SetBool("IsRunning", false);
        }
    }

    [PunRPC]
    void InitStats(int hp)
    {
        Debug.Log(PV.ViewID + " Stats Initialized");
        healthMax = hp;
        health = hp;
    }
}