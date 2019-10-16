using Photon.Pun;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private PhotonView PV;
    Transform target;
    Animator anim;
    bool isAttacking = false;
    PlayerController player;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        player = transform.GetComponent<PlayerController>();
        target = player.target;
    }

    public void EndAttackFast()
    {
        if (!player.canRun)
        {
            player.canRun = true;
            player.isAttacking = false;
            player.canAttack = true;
        }
    }
    public void EndAttackFrame()
    {
        if (!player.canRun)
        {
            PV.RPC("RPC_SetWeaponActive", RpcTarget.All, false);
        }
    }
}
