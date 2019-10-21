using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParryBox : MonoBehaviour
{
    private PhotonView PV;
    [SerializeField] public ParryBoxHandler parryBox;
    [SerializeField] private PlayerHandler PH;

    public bool parry;
    public WeaponInfo hitting;


    void Start()
    {
        PV = GetComponent<PhotonView>();
        PH = GetComponent<PlayerHandler>();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        parry = Input.GetButtonDown("P1_Parry");
    }

    public void ParryStartFrame()
    {
        Debug.Log("Parry Start Frame");
        if (PV.IsMine)
        {
            PV.RPC("RPC_SetParryBoxActive", RpcTarget.All, true);
            PV.RPC("RPC_SetHitBoxActive", RpcTarget.All, false);
        }
    }
    public void ParryEndFrame()
    {
        Debug.Log("Parry End Frame");
        if (PV.IsMine)
        {
            PV.RPC("RPC_SetParryBoxActive", RpcTarget.All, false);
            PV.RPC("RPC_SetHitBoxActive", RpcTarget.All, true);

        }
    }
    public void ParryEnd()
    {
        //Debug.Log("Parry End");
        //if (PV.IsMine)
        //{
        //    PV.RPC("RPC_EndParry", RpcTarget.All);
        //}
    }    

    [PunRPC]
    void RPC_StartParry()
    {
        //Debug.Log(PV.ViewID + " got hit by " + hitting.name + " for " + hitting.damage + " Damage");
        PH.canParry = false;
        PH.canAttack = false;
        PH.canRun = false;
        PH.isParrying = true;
        PH.isRunning = false;
        PH.isAttacking = false;
        PH.myAnim.SetBool("IsRunning", false);
        PH.myAnim.SetBool("IsParrying", true);
        hitting = null;
        StartCoroutine(WaitForParry());
    }

    private IEnumerator WaitForParry()
    {
        yield return new WaitForSeconds(1.0f);
        //parryBox.isBusy = false;
        PV.RPC("RPC_EndParry", RpcTarget.All);
    }

    [PunRPC]
    void RPC_EndParry()
    {
        PH.canParry = true;
        PH.isParrying = false;
        PH.canAttack = true;
        PH.canRun = true;
        PH.myAnim.SetBool("IsParrying", false);
    }

    [PunRPC]
    void RPC_SetParryBoxActive(bool value)
    {
        if (value == true)
        {
            Debug.Log(PV.ViewID + " Parry Box activated");
            parryBox.parryBoxCol.enabled = true;
        }
        else
        {
            Debug.Log(PV.ViewID + " Parry Box deactivated");
            parryBox.parryBoxCol.enabled = false;
        }
    }
}
