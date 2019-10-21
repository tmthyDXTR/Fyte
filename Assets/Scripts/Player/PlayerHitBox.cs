using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private PhotonView PV;
    [SerializeField] public HitBoxHandler hitBox;
    [SerializeField] private PlayerHandler PH;
    
    public WeaponInfo hitting;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PH = GetComponent<PlayerHandler>();
    }

    [PunRPC] 
    void RPC_GetHit(int damage)
    {
        Debug.Log(PV.ViewID + " got hit by " + hitting.name + " for " + hitting.damage + " Damage");
        //PH.health -= damage;
        PV.RPC("RPC_RemoveStat", RpcTarget.OthersBuffered, "Health", damage);
        PH.isHit = true;
        PH.canAttack = false;
        PH.canRun = false;
        PH.isRunning = false;
        PH.isAttacking = false;
        PH.myAnim.SetBool("IsHit", true);
        hitting = null;
        StartCoroutine(WaitForHit());
    }

    private IEnumerator WaitForHit()
    {
        yield return new WaitForSeconds(0.5f);
        hitBox.isBusy = false;
        PH.isHit = false;
        PH.myAnim.SetBool("IsHit", false);
        PH.canAttack = true;
        PH.canRun = true;
    }

    [PunRPC]
    void RPC_SetHitBoxActive(bool value)
    {        
            hitBox.hitBox.enabled = value;
    }
}
