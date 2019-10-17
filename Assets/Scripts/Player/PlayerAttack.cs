using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PhotonView PV;
    private PlayerHandler PH;
    private Animator myAnim;

    [SerializeField] private BoxCollider weaponCol;
    
    [Header("Input")]
    public bool attack1;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        PH = GetComponent<PlayerHandler>();
        myAnim = GetComponent<Animator>();
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
        PH.isAttacking = false;
        PH.canAttack = true;
        PH.canRun = true;
        myAnim.SetBool("IsAttacking", false);
    }
    #endregion

    [PunRPC]
    void RPC_SetWeaponActive(bool value)
    {
        if (value == true)
        {
            Debug.Log(PV.ViewID + " Weapon activated");
            weaponCol.enabled = true;
        }
        else
        {
            Debug.Log(PV.ViewID + " Weapon deactivated");
            weaponCol.enabled = false;
        }
    }
}
