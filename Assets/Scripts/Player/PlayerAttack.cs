using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PhotonView PV;
    private PlayerHandler PH;

    [SerializeField] private BoxCollider weaponCol;
    
    [Header("Input")]
    public bool attack1;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        PH = GetComponent<PlayerHandler>();
    }

    void Update()
    {
        if (PV.IsMine)
        {
            GetInput();
        }
    }

    private void GetInput()
    {
        attack1 = Input.GetButtonDown("P1_Attack1");
    }

    #region Attack_1
    public void Attack1StartFrame()
    {
        Debug.Log("Attack1 Start Frame");
        if (PV.IsMine)
        {
            PV.RPC("RPC_SetWeaponActive", RpcTarget.All, true);
        }
    }
    public void Attack1EndFrame()
    {
        Debug.Log("Attack1 End Frame");
        if (PV.IsMine)
        {
            PV.RPC("RPC_SetWeaponActive", RpcTarget.All, false);
        }
    }
    public void Attack1End()
    {
        Debug.Log("Attack1 End");
        if (PV.IsMine)
        {
            PV.RPC("RPC_EndAttack", RpcTarget.All);
        }
    }
    #endregion

    [PunRPC]
    void RPC_SetWeaponActive(bool value)
    {
        if (value == true)
        {
            Debug.Log(PV.ViewID + " Weapon activated");
            weaponCol.enabled = true;
            //Cant jump while attack damage
            PH.canJump = false;
            PV.RPC("RPC_RemoveStat", RpcTarget.AllBuffered, "Stamina", 1);
        }
        else
        {
            Debug.Log(PV.ViewID + " Weapon deactivated");
            weaponCol.enabled = false;
            PH.canJump = true;
        }
    }
    [PunRPC]
    void RPC_StartAttack()
    {
        PH.isAttacking = true;
        PH.isRunning = false;
        PH.canAttack = false;
        PH.canRun = false;
        PH.canParry = false;
        PH.myAnim.SetBool("IsRunning", false);
        PH.myAnim.SetBool("IsAttacking", true);
    }
    
    [PunRPC] 
    void RPC_EndAttack()
    {
        PH.isAttacking = false;
        PH.canAttack = true;
        PH.canRun = true;
        PH.canParry = true;
        PH.myAnim.SetBool("IsAttacking", false);
    }
}
